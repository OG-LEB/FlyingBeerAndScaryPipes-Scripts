using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LEVEL : MonoBehaviour
{
    private static LEVEL instance;
    [SerializeField] InterstitialAd _interstiticalAd;
    [SerializeField] BannerAd bannerAD;

    public static LEVEL GetInstance()
    {
        return instance;
    }

    private const float CAM_ORTH_SIZE = 20;
    private const float PIPE_BODY_WEIGHT_CONST = 4;
    private const float PIPE_HEAD_HALF_HEIGHT = 0.6f;
    private const float PIPE_SPEED = 15; 
    private const float PIPE_DEAD_X_POS = -20;
    private const float PIPE_SPAWN_X_POS = 20;
    private const float BEER_X_POS = 0f;
    private const float GAP_SIZE_START = 21f;

    private List<Pipe> List_Pipes;
    private float SpawnTimer;
    private float SpawnTimerMax;
    private float gapSize;
    private int PipesCount;
    [SerializeField] int PipesPassCount; //Score
    private State GameState;
    private RestartWin restartWin;
    private StartWin startWin;
    private int HightScore;
    private SavingSystem saveSystem;
    private PlayWin playwin;
    private AnimationHandler anim;
    [SerializeField] private RestartAnimationScript restartAnim;
    public int locationNumber = 1;
    private GameOverCutSceneScript videoPlayer;
    private bool isGameEnded;

    [Header("Level Sprites")]
    public Image BackgroundSprite;
    public Sprite CurrentPipeHeadSprite;
    public Sprite CurrentPipeBodySprite;
    public string[] LocName;
    private string currentLocation;

    [SerializeField] Text PipesCountFromText;
    public void SetPipesCountForTest() 
    {
        PipesPassCount = Convert.ToInt32(PipesCountFromText.text);
        Debug.Log("Changed !!");
    }
    private enum State
    {
        Playing,
        Pause
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;//
        instance = this;
        List_Pipes = new List<Pipe>();
        SpawnTimer = SpawnTimerMax = 1; // Интервал спавна труб
        gapSize = 10;
        PipesCount = 0;
    }
    private void Start()
    {
        gapSize = GAP_SIZE_START;
        GameState = State.Pause;
        restartWin = RestartWin.GetInstance();
        startWin = StartWin.GetInstance();
        saveSystem = SavingSystem.GetInstance();
        playwin = PlayWin.GetInstance();
        anim = AnimationHandler.GetInstance();
        videoPlayer = GAME_ASSETS.GET_ASSETS().GetGameOverCutSceneScript();

        isGameEnded = saveSystem.LoadGameEndBoolean();
        LoadSceneSprites();
        GameAwake();
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPos) 
    {
        CreatePipe(gapY - gapSize * 0.5f , xPos, true); //
        CreatePipe(CAM_ORTH_SIZE * 2 - gapY - gapSize * 0.5f -5  , xPos, false); //
        PipesCount++;
        CheckDifficulty();
    }

    private void CreatePipe(float height, float xPosition, bool Bottom) 
    {
        //set head
        Transform pipeHead = Instantiate(GAME_ASSETS.GET_ASSETS().pfPipeHead);
        float PipeHeadYPosition;
        if (Bottom)
        {
            PipeHeadYPosition = -CAM_ORTH_SIZE + height - PIPE_HEAD_HALF_HEIGHT + 5; //
        } else
        {
            PipeHeadYPosition = CAM_ORTH_SIZE - height + PIPE_HEAD_HALF_HEIGHT ;
        }
        pipeHead.position = new Vector2(xPosition, PipeHeadYPosition);
        pipeHead.GetComponent<SpriteRenderer>().sprite = CurrentPipeHeadSprite;
        
        //set body
        Transform pipeBody = Instantiate(GAME_ASSETS.GET_ASSETS().pfPipeBody);
        float PipeBodyYPosition;
        if (Bottom)
        {
            PipeBodyYPosition = -CAM_ORTH_SIZE + 4; //
        }else 
        {
            PipeBodyYPosition = CAM_ORTH_SIZE ;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector2(xPosition, PipeBodyYPosition);
        pipeBody.GetComponent<SpriteRenderer>().sprite = CurrentPipeBodySprite;

        pipeBody.GetComponent<SpriteRenderer>().size = new Vector2(PIPE_BODY_WEIGHT_CONST, height);
        BoxCollider2D BodyCollider = pipeBody.GetComponent<BoxCollider2D>();
        BodyCollider.size = new Vector2(PIPE_BODY_WEIGHT_CONST, height);
        BodyCollider.offset = new Vector2(0, height * 0.5f);

        Pipe pipe = new Pipe(pipeHead,pipeBody,Bottom);
        List_Pipes.Add(pipe);
    }

    public int GetPipesPassCount() 
    {
        return PipesPassCount;
    }

    private void PipesMovement() 
    {
        for(int i = 0; i < List_Pipes.Count; i++)
        {
            Pipe pipe = List_Pipes[i];
            bool isBeerToTheRightOfThePipe = pipe.GetXPosition() > BEER_X_POS;
            pipe.Moving();
            if (isBeerToTheRightOfThePipe && pipe.GetXPosition() <= BEER_X_POS)
            {
                if (pipe.IsBottom())
                {
                    PipesPassCount ++; //
                    SoundManager.GetInstance().PlayPipePassSound();
                }
            }
            if (pipe.GetXPosition() < PIPE_DEAD_X_POS) 
            {
                pipe.Suicide();
                List_Pipes.Remove(pipe);
                i--; 
            }
        }
    }

    int checkValue = 0;
    private void CheckDifficulty() 
    {
        // Set gap size
        if (PipesPassCount % 10 == 0 && PipesPassCount > checkValue && gapSize > 11f)
        {
            checkValue = PipesPassCount;
            gapSize -= 1f;
        }

        
        if(PipesPassCount % 50 == 0 && PipesPassCount != ((locationNumber * 50) - 50) && PipesPassCount < 550)
        {
            ChangeLocationNumber();
        } 
    }

    
    private void ChangeLocationNumber() 
    {
        locationNumber++;
        saveSystem.SaveSceneNumber();
        gapSize = GAP_SIZE_START; // При смене локации размер труб обнуляется
        anim.PlayTranslateLocSpriteAnimation(); //Запускаем анимацию смены локации
                                      //LoadSceneSprites(); в Animation Event)
    }
    
    private void PipesSpawn() 
    {
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer >= SpawnTimerMax) 
        {
            SpawnTimer = 0;

            float EdgeLimit = 5;
            float minHeight = gapSize * 0.5f + EdgeLimit ; //
            float maxHeight = (CAM_ORTH_SIZE * 2) - minHeight - EdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POS);
            
        }
    }
    private void Update()
    {
        if (GameState == State.Playing)
        {
            PipesMovement();
            PipesSpawn();
        }
    }

    public void GameStart() 
    {
        GameState = State.Playing;
        startWin.StartGame();
        playwin.Show();
        anim.PlayLocationTextAnimation(currentLocation);
        bannerAD.LoadBanner(); // Loading banner
    }

    private void GameAwake() // When the game awake
    {
        startWin.SeTHightScore(saveSystem.LoadHightScore()); // загрука HightScore в начале игры
        HightScore = saveSystem.LoadHightScore(); //set the HightScore varible   
        restartAnim.Hide();


    }
    public void GameOver(bool trap) 
    {
        GameState = State.Pause;
        playwin.Hide();
        restartWin.GameOver();
        restartWin.SetHightScore(HightScore);
        restartWin.SetEarnedScore(PipesPassCount, trap);
        if (PipesPassCount > HightScore)
        {
            NewHightScore();
        }
        saveSystem.SaveSceneNumber(); //Сохраняем номер локации
        int failedGames = PlayerPrefs.GetInt("FailedGames");
        failedGames++;
        if (failedGames >= 20)
        {
            _interstiticalAd.ShowAD();
            PlayerPrefs.SetInt("FailedGames", 0);
        }
        else 
        {
            PlayerPrefs.SetInt("FailedGames", failedGames);
        }
    }

    private void NewHightScore() 
    {
        HightScore = PipesPassCount;
        saveSystem.SaveHightScore(HightScore);
        restartWin.NewHightScore();
    }


    public void PauseGame() 
    {
        GameState = State.Pause;
    }

    public void UnpauseGame() 
    {
        GameState = State.Playing;
    }

    public void Restart() 
    {
        saveSystem.SaveSoundState();
        restartAnim.Show();
    }

    public void LoadSceneRestarting() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Scene");
    }

    public bool _isPlaying() 
    {
        if (GameState == State.Pause) return false;
        else if (GameState == State.Playing) return true;
        else return false;
    }

    public void TranslateLocationSprites() //Для анимации перехода локаций(Animation Event)
    {
        LoadSceneSprites(); 

        for (int i = 0; i < List_Pipes.Count; i++) 
        {
            Pipe p = List_Pipes[i];
            p.Suicide();
            List_Pipes.Remove(p);
            i--; 

        }

    }

    void OnApplicationQuit() 
    {
        PlayerPrefs.SetInt("FailedGames", 0);
    }

    
    private void LoadSceneSprites() 
    {
        GAME_ASSETS assets = GAME_ASSETS.GET_ASSETS();
        BEER beer = assets.GetBeer();
        locationNumber = saveSystem.LoadSceneNumber();
        switch (locationNumber)
        {
            case 1:
                BackgroundSprite.sprite = assets.BackgroundSprite[0];
                currentLocation = LocName[0];
                PipesPassCount = 0;
                break;
            case 2:
                BackgroundSprite.sprite = assets.BackgroundSprite[1];
                currentLocation = LocName[1];
                PipesPassCount = 50;
                break;
            case 3:
                BackgroundSprite.sprite = assets.BackgroundSprite[2];
                currentLocation = LocName[2];
                PipesPassCount = 100;
                break;
            case 4:
                BackgroundSprite.sprite = assets.BackgroundSprite[3];
                currentLocation = LocName[3];
                PipesPassCount = 150;
                break;
            case 5:
                BackgroundSprite.sprite = assets.BackgroundSprite[4];
                currentLocation = LocName[4];
                PipesPassCount = 200;
                break;
            case 6:
                BackgroundSprite.sprite = assets.BackgroundSprite[5];
                currentLocation = LocName[5];
                PipesPassCount = 250;
                break;
            case 7:
                BackgroundSprite.sprite = assets.BackgroundSprite[6];
                currentLocation = LocName[6];
                PipesPassCount = 300;
                break;
            case 8:
                BackgroundSprite.sprite = assets.BackgroundSprite[7];
                currentLocation = LocName[7];
                PipesPassCount = 350;
                break;
            case 9:
                BackgroundSprite.sprite = assets.BackgroundSprite[8];
                currentLocation = LocName[8];
                PipesPassCount = 400;
                break;
            case 10:
                BackgroundSprite.sprite = assets.BackgroundSprite[9];
                currentLocation = LocName[9];
                PipesPassCount = 450;
                break;
            case 11:
                BackgroundSprite.sprite = assets.BackgroundSprite[10];
                currentLocation = LocName[10];
                PipesPassCount = 500;

                print(isGameEnded + " isgameended");
                if (isGameEnded == false)
                {
                    GameEndingCutScene();
                }
                break;
        }
        anim.PlayLocationTextAnimation(currentLocation);
    }

    private void GameEndingCutScene() 
    {
        videoPlayer.PlayGameEndingCutscene();
        GameState = State.Pause;
        BEER.GetInstance().SetPauseState();
    }

    public void GameEndingCutSceneEnd(VideoPlayer source) 
    {
        if (PipesPassCount > HightScore)
        {
            NewHightScore();
        }
        saveSystem.SaveSceneNumber(); //Сохраняем номер локации
        PlayerPrefs.SetInt("GameEndBool", 1); // Saving bool isGameEnding;
        LoadSceneRestarting();
    }

    /*
     * Отдельный класс для создания трубы
     */
    private class Pipe 
    {
        private Transform pipeHeadTr;
        private Transform pipeBodyTr;
        private bool isBottom;

        public Pipe (Transform pipeHeadTr, Transform pipeBodyTr, bool isBottom) 
        {
            this.pipeHeadTr = pipeHeadTr;
            this.pipeBodyTr = pipeBodyTr;
            this.isBottom = isBottom;
        }

        public bool IsBottom() 
        {
            return isBottom;
        }
        public void Moving() 
        {
            pipeHeadTr.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * PIPE_SPEED;
            pipeBodyTr.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * PIPE_SPEED;
        }

        public float GetXPosition()     
        {
            return pipeBodyTr.transform.position.x;
        }

        
        public void Suicide() 
        {
            Destroy(pipeHeadTr.gameObject);
            Destroy(pipeBodyTr.gameObject);
        }
    }
}
