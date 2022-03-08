using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameOverCutSceneScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private LEVEL lvl;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += VideoPlayEnd;
        lvl = LEVEL.GetInstance();
        videoPlayer.loopPointReached += lvl.GameEndingCutSceneEnd;
    }

    public void PlayGameEndingCutscene() 
    {
        videoPlayer.Play();
        print("End cutscene is playing");
    }

    private void VideoPlayEnd(VideoPlayer source) 
    {
        gameObject.SetActive(false);
        BEER.GetInstance().SetPlayState();

    }

}
