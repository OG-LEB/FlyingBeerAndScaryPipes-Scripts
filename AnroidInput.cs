using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnroidInput : MonoBehaviour 
{
    private static AnroidInput instance;
    public static AnroidInput GetInstance() 
    {
        return instance;
    }

    private BEER beer;
    private bool PauseButtonPush = false; // Решение проблемы с нажатием кнопки паузы и прыжком одновременно



    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        beer = BEER.GetInstance();
    }

    private void Update()
    {
        CheckJumpTouch();
    }

    private void CheckJumpTouch() 
    {
        if (Input.GetMouseButtonDown(0) && !PauseButtonPush)
        {
            beer.Jump();

        }
    }
    public void SetPausePushBoolean(bool value) 
    {
        PauseButtonPush = value;
    }
}
