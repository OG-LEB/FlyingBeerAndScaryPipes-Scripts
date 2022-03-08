using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEER : MonoBehaviour
{
    private const float JUMP_AMOUNT = 50f;

    private GAME_ASSETS assets;
    private Rigidbody2D rb;
    private static BEER instance;
    private LEVEL lvl;
    private State BeerState;
    private SoundManager sound;
    private AnimationHandler anim;
    private SavingSystem saveSystem;

    private SpriteRenderer spriteRender;
    private Sprite Beer_Fly_0, Beer_Fly_1;
    //Timer to change sprite
    private float timer = 0;
    private const float timerMax = 0.2f;
    private bool firstSprite;
    private bool LocTranslateBool = false;

    [Header("Skin's")]
    [SerializeField] Sprite[] Player_skins;
    private int SkinNum; 

    public static BEER GetInstance() 
    {
        return instance;
    }

    private enum State 
    {
        WaitingToStart,
        Playing,
        Dead
    }

    private void Awake()
    {
        instance = this;
        assets = GAME_ASSETS.GET_ASSETS();
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        GameAwake();
    }

    private void Start()
    {
        lvl = assets.GetLevelLink();
        sound = SoundManager.GetInstance();
        anim = AnimationHandler.GetInstance();
        SetFirstSprite();
        saveSystem = SavingSystem.GetInstance();
        SkinNum = saveSystem.LoadBeerSkinNum(); // Загружаем номер скина
        LoadSkins(); // Загружаем скины
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }                                            
        if ((transform.position.y >= 23 || transform.position.y <= -18) && BeerState == State.Playing)
        { 
            Dead(true); 
        }

        switch (BeerState) 
        {
            case State.WaitingToStart:
                rb.bodyType = RigidbodyType2D.Static;
                break;
            case State.Playing:
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case State.Dead:
                rb.bodyType = RigidbodyType2D.Static;
                break;
        }
    }

    private void SetFirstSprite() 
    {
        spriteRender.sprite = Beer_Fly_0;
        firstSprite = true;
    }

    private void SetSecondSprite()
    {
        spriteRender.sprite = Beer_Fly_1;
        firstSprite = false;
    }

    private void FixedUpdate()
    {
        BeerAnimation();
    }

    private void BeerAnimation() 
    {
        if (BeerState == State.Playing)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0;
                if (firstSprite)
                {
                    SetSecondSprite();
                }
                else if (!firstSprite)
                {
                    SetFirstSprite();
                }
            }
        }
    }

    private void GameAwake() 
    {
        BeerState = State.WaitingToStart;

    }
    private void StarGame() 
    {
        BeerState = State.Playing;
        lvl.GameStart();
    }

    public void PauseGame() 
    {
        BeerState = State.Dead;
    }

    public void UnpauseGame()
    {
        BeerState = State.Playing;
    }

    public void Jump() 
    {
        switch (BeerState)
        {
            case State.WaitingToStart:
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = Vector2.up * JUMP_AMOUNT;
                sound.PlayFlySound();
                anim.PlayBeerClickAnimation();
                StarGame();
                break;
            case State.Playing:
                rb.velocity = Vector2.up * JUMP_AMOUNT;
                sound.PlayFlySound();
                anim.PlayBeerClickAnimation();
                break;
            case State.Dead:
                break;
        }

    }

    public void SetTranslateLocBoolean(bool value) 
    {
        LocTranslateBool = value;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (BeerState == State.Playing && !LocTranslateBool)
            Dead(false);
    }

    private void Dead(bool trap) 
    {
        BeerState = State.Dead;
        lvl.GameOver(trap);
        SoundManager.GetInstance().PlayDeadSound();
        anim.StopBeerAnimation();
        anim.PlayBackGroundCrashAnimation();
    }

    public void LoadSprites(Sprite FirstSprite, Sprite SecondSprite) 
    {
        Beer_Fly_0 = FirstSprite;
        Beer_Fly_1 = SecondSprite;
    }

    public void SetPauseState() 
    {
        BeerState = State.Dead;
    }

    public void SetPlayState() 
    {
        BeerState = State.Playing;
    }

    public void ChangeSkin() 
    {
        if (SkinNum == 1) 
        { 
            Beer_Fly_0 = Player_skins[2]; 
            Beer_Fly_1 = Player_skins[3];
            SkinNum = 2;
        }
        else if (SkinNum == 2) 
        {
            Beer_Fly_0 = Player_skins[0]; 
            Beer_Fly_1 = Player_skins[1];
            SkinNum = 1;
        }
        spriteRender.sprite = Beer_Fly_0; //Cause the start sprite is empty, it solve the problem
        PlayerPrefs.SetInt("PlayerSkinNum", SkinNum);
        print("Saved skin num - " + SkinNum);
    }

    private void LoadSkins() 
    {
        if (SkinNum == 1)
        {
            Beer_Fly_0 = Player_skins[0];
            Beer_Fly_1 = Player_skins[1];
        }
        else if (SkinNum == 2)
        {
            Beer_Fly_0 = Player_skins[2];
            Beer_Fly_1 = Player_skins[3];
        }
        spriteRender.sprite = Beer_Fly_0;
        print("Loaded skin num - " + SkinNum);
    }
    
}
