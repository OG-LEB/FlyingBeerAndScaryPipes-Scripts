using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    private static SavingSystem instance;
    private GAME_ASSETS assets;
    private SoundManager sound;

    public static SavingSystem GetInstance() 
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
        assets = GAME_ASSETS.GET_ASSETS();
        sound = assets.GetSoundManagerLink();
    }


    public int LoadHightScore() 
    {
        return PlayerPrefs.GetInt("HightScore");
    }

    public void SaveHightScore(int hightscore) 
    {
        PlayerPrefs.SetInt("HightScore", hightscore);
        PlayerPrefs.Save();
    }

    public void SaveSoundState() 
    {
        int value = 0;
        if (sound.muteSound) { value = 1; }
        else if (!sound.muteSound) { value = 0; }
        PlayerPrefs.SetInt("MuteSoundBool", value);
        PlayerPrefs.Save();
    }

    public void LoadSoundState() 
    {
        int value = PlayerPrefs.GetInt("MuteSoundBool");
        if (value == 0) { sound.muteSound = false; }
        else if (value == 1) { sound.muteSound = true; }
    }

    
    public void SaveSceneNumber() 
    {
        PlayerPrefs.SetInt("SceneNubmer", LEVEL.GetInstance().locationNumber);
    }

    public int LoadSceneNumber() 
    {
        if (PlayerPrefs.GetInt("SceneNubmer") == 0) return 1;
        else
            return PlayerPrefs.GetInt("SceneNubmer");
    }

    public bool LoadGameEndBoolean()
    {
        if (PlayerPrefs.GetInt("GameEndBool") == 1) return true;
        else return false;
    }

    public int LoadBeerSkinNum() 
    {
        if (PlayerPrefs.GetInt("PlayerSkinNum") == 2) return 2;
        else return 1;
    }

}

