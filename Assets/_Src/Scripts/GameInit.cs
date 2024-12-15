// Author: 
// Created Date: 19/07/2024
// Update Time: 19/07

using System;
using Cysharp.Threading.Tasks;
using Template.Defines;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    enum State
    {
        ShowSplashScreen,
        Scene,
        Completed
    }

    private State _state;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
     
        SetState(State.ShowSplashScreen);
    }
    
    private async void SetState(State state)
    {
        _state = state;
        switch (_state)
        {
            case State.ShowSplashScreen:
                //TODO: Show splash screen task
                // await GameDependencies.Initialize();
                SetState(State.Scene);
                break;
            case State.Scene:
                LoadScene().Forget();
                break;
        }
    }

    private async UniTask LoadScene()
    {
        await SceneManager.LoadSceneAsync(TypeSceneName.Lobby.ToString(), LoadSceneMode.Single);
        SetState(State.Completed);
    }
}