using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAME_ASSETS : MonoBehaviour
{
    private static GAME_ASSETS ASSETS_OBJECT;

    public static GAME_ASSETS GET_ASSETS() //Возвращение статичного объекта, для доступа к нему в любом скрипте [ static = доступ везде ]
    {
        return ASSETS_OBJECT;
    }

    [SerializeField] private LEVEL lvl; 
    public LEVEL GetLevelLink()   
    {
        return lvl;  
    }

    [SerializeField] private SettingWindow settings;
    public SettingWindow GetSettingsLink()
    {
        return settings;
    }
    [SerializeField] private PauseWin pauseWin;
    public PauseWin GetPauseWinLink()
    {
        return pauseWin;
    }
    [SerializeField] private StartWin startWin;
    public StartWin GetStartWinLink()
    {
        return startWin;
    }
    [SerializeField] private AnroidInput input;
    public AnroidInput GetAndroidInputLink()
    {
        return input;
    }
    [SerializeField] private AnimationHandler anim;
    public AnimationHandler GetAnimationHandlerLink()
    {
        return anim;
    }
    [SerializeField] private SoundManager sound;
    public SoundManager GetSoundManagerLink()
    {
        return sound;
    }
    [SerializeField] private SavingSystem saveSystem;
    public SavingSystem GetSaveSystemLink()
    {
        return saveSystem;
    }
    [SerializeField] private GameOverCutSceneScript cutSceneScript;
    public GameOverCutSceneScript GetGameOverCutSceneScript() 
    {
        return cutSceneScript;
    }

    [SerializeField] private BEER beer;

    public BEER GetBeer()
    {
        return beer;
    }

    private void Awake()
    {
        ASSETS_OBJECT = this;
    }

    public Transform pfPipeHead; // PipeHead Prefub
    public Transform pfPipeBody; // PupeBody Prefub 
    public Sprite[] BackgroundSprite;
    
}
