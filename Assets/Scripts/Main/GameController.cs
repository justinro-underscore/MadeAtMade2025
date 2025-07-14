using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    UNKNOWN,
    MENU,
    PLAY,
    END,
    LOSE
}

public class GameController : MonoBehaviour {
    public static GameController instance = null;

    [SerializeField] private SceneController sceneController;

    public GameState currGameState { get; private set; }

    public void Awake() {
        if ( null == instance ) {
            instance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }

        // Initialize the scene controller
        sceneController.Initialize();

        // Ensure that the base scene exists
        if (!sceneController.activeScenes.Contains(Scenes.Base))
        {
            // TODO Does this work?
            Debug.Break();
        }
        // This means that only the base scene exists, and we should create the main menu
        if (sceneController.activeScenes.Count == 1)
        {
            sceneController.LoadScene(Scenes.Play, false);
            currGameState = GameState.MENU; // Directly assign because LoadScene takes a sec
        }
        else
        {
            currGameState = sceneController.GetCurrSceneGameState();
        }
    }

    public void ChangeState(GameState newGameState)
    {
        bool handled = true;
        string nextSceneName = null;
        switch (currGameState)
        {
            case GameState.MENU:
                if (newGameState == GameState.PLAY)
                {
                    sceneController.UnloadScene(Scenes.MainMenu);
                    //sceneController.UnloadScene(Scenes.Player);
                    //sceneController.UnloadScene(Scenes.DeathWallScene);
                    nextSceneName = Scenes.Play;
                }
                else
                {
                    handled = false;
                }
                break;
            case GameState.PLAY:
                if (newGameState == GameState.END)
                {
                    sceneController.UnloadScene(Scenes.Play);
                    //sceneController.UnloadScene(Scenes.Player);
                    //sceneController.UnloadScene(Scenes.DeathWallScene);
                    nextSceneName = Scenes.EndScene;
                }
                else
                {
                    handled = false;
                }
                break;
            case GameState.LOSE:
                if (newGameState == GameState.MENU)
                {
                    sceneController.UnloadScene(Scenes.LoseScene);
                    //sceneController.UnloadScene(Scenes.Player);
                    //sceneController.UnloadScene(Scenes.DeathWallScene);
                    nextSceneName = Scenes.MainMenu;
                }
                else
                {
                    handled = false;
                }
                break;
            case GameState.END:
                if (newGameState == GameState.MENU)
                {
                    sceneController.UnloadScene(Scenes.EndScene);
                    //sceneController.UnloadScene(Scenes.Player);
                    //sceneController.UnloadScene(Scenes.DeathWallScene);
                    nextSceneName = Scenes.MainMenu;
                }
                else
                {
                    handled = false;
                }
                break;
            default:
                handled = false;
                break;
        }
        if (!handled)
        {
            Debug.LogError($"Unknown game flow: {currGameState} -> {newGameState}");
            return;
        }

        if (nextSceneName != null)
        {
            sceneController.LoadScene(nextSceneName, false); // TODO Look into if we need to use async
        }
        Debug.Log($"Changing game state to {newGameState}");
        currGameState = newGameState;
    }

    // DO NOT USE UNLESS YOU KNOW WHAT YOU'RE DOING
    public void UnloadSceneDangerously(string sceneName)
    {
        sceneController.UnloadScene(sceneName);
    }

    public bool IsSceneLoaded(string sceneName)
    {
        return sceneController.IsSceneLoaded(sceneName);
    }
}
