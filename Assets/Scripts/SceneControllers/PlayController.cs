using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayController : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }

    public static PlayController Instance { get; private set; }

    protected void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    override protected void SceneUpdate()
    {
    }

    override protected void SceneFixedUpdate()
    {
    }
}
