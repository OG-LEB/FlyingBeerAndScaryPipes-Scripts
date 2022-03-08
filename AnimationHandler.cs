using UnityEngine;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour
{
    private static AnimationHandler instance;

    [SerializeField] private Animator beer;
    [SerializeField] private Animator NewHightScoreTitle;
    [SerializeField] private Animator GameOverWin;
    [SerializeField] private Animator BackgroundSprite;
    [SerializeField] private Animator PauseWin;
    [SerializeField] private Animator StartWin;
    [SerializeField] private Animator PlayWin;
    [SerializeField] private Animator SettingsWin;
    [SerializeField] private Animator RestartAnimation;
    [SerializeField] private Animator LocationTextAnimator;
    [SerializeField] private Animator TranslateLocImg;

    public static AnimationHandler GetInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void PlayBeerClickAnimation() 
    {
        float procent = Random.Range(0, 100);
        if (procent <= 1)
        {
            beer.SetTrigger("Click_flip");
        }
        if (procent == 2 || procent == 3 ) 
        {
            beer.SetTrigger("Click_Backflip");
        }
        else
        {
            beer.SetTrigger("Click");
        }
    }

    public void PlayNewHighScoreAnimation() 
    {
        NewHightScoreTitle.SetTrigger("NewScore");
    }

    public void StopBeerAnimation() 
    {
        beer.StartPlayback();
    }

    public void PlayGameOverWinShowAnimation() 
    {
        GameOverWin.SetTrigger("Show");
    }

    public void PlayBackGroundCrashAnimation() 
    {
        BackgroundSprite.SetTrigger("Crash");
    }

    public void PlayPauseWinShowAnimation() 
    {
        PauseWin.SetTrigger("Show");
    }

    public void PlayPauseWinHideAnimation() 
    {
        PauseWin.SetTrigger("Hide");
    }

    public void PlayStartWinHideAnimation() 
    {
        StartWin.SetTrigger("Hide");
    }

    public void PlayPlayWinShowAnimation() 
    {
        PlayWin.SetTrigger("Show");
    }
    public void PlayBackGroundGameAwakeAnimation() 
    {
        BackgroundSprite.SetTrigger("GameAwake");
    }

    public void PlayStartWinShowAnimation() 
    {
        StartWin.SetTrigger("Show");
    }

    public void PlayGameOverWinHideAnimation() 
    {
        GameOverWin.SetTrigger("Hide");
    }

    public void PlaySettingsWinShowAnimation() 
    {
        SettingsWin.SetTrigger("Show");
    }

    public void PlaySettingsWinHideAnimation()
    {
        SettingsWin.SetTrigger("Hide");
    }

    public void PlayRestartAnimationShow() 
    {
        RestartAnimation.SetTrigger("Show");
    }

    public void PlayRestartAnimationHide()
    {
        RestartAnimation.SetTrigger("Hide");
    }

    
    public void PlayLocationTextAnimation(string LocText) 
    {
        LocationTextAnimator.GetComponent<Text>().text = "Location:\n"+LocText;
        LocationTextAnimator.SetTrigger("Prekol");
    }

    public void PlayTranslateLocSpriteAnimation() 
    {
        TranslateLocImg.SetTrigger("Translate_Loc");
    }
    
}
