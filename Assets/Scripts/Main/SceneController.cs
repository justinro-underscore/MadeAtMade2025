using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: one day maybe we can have these as the dropdown value for levels
public static class Scenes {
    public const string Base = "BaseScene";
    public const string Play = "PlayScene";
    public const string Player = "Player";
    public const string DeathWallScene = "DeathWallScene";
    public const string ObstaclesSceneCF = "ObstaclesSceneCF";
    public const string EndScene = "EndScene";
    public const string MainMenu = "MainMenu";
    public const string LoseScene = "LoseScene";
}

public class SceneController : MonoBehaviour {
    private IDictionary<string, GameState> gameSceneStateMapping = new Dictionary<string, GameState>() {
        {Scenes.Play, GameState.PLAY},
        {Scenes.Player,GameState.PLAY},
        {Scenes.DeathWallScene, GameState.PLAY},
        {Scenes.EndScene, GameState.END},
        {Scenes.MainMenu, GameState.MENU},
        {Scenes.LoseScene, GameState.LOSE}
    };

    public static SceneController instance = null;

    public List<string> activeScenes { get; private set; }

    public void Initialize() {
        activeScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            activeScenes.Add(scene.name);
        }
    }

    public List<string> GetActiveScenes() {
        return activeScenes.GetRange(1, activeScenes.Count - 1);
    }

    public void UnloadScene(string sceneName) {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void LoadScene(string sceneName, bool async=true) {
        if (async) {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public bool IsSceneLoaded(string sceneName) {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene != null && scene.isLoaded;
    }

    public GameState GetCurrSceneGameState()
    {
        string currSceneName = activeScenes[^1];
        if (gameSceneStateMapping.ContainsKey(currSceneName))
        {
            return gameSceneStateMapping[currSceneName];
        }
        Debug.LogError($"No game state associated with scene {{{currSceneName}}}");
        return GameState.UNKNOWN;
    }
}