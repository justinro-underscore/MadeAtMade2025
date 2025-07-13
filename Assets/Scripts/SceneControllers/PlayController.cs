using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 
using UnityEngine.UI;

public enum PlayerState
{
    moving,
    repair,
    fire,
}
public class PlayController : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }
  
    //Move Variables.
    public float rotationSpeed = 90f;
    public float moveSpeed = 2f;

    private PlayerState playerState = PlayerState.moving;
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
        switch (playerState)
        {
            case PlayerState.moving:
                if (Input.GetKey(KeyCode.A))
                {
                    // Rotate left around Z axis
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    // Rotate left around Z axis
                    transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                   //Testing States
                   playerState = PlayerState.repair;
                }


                transform.position += transform.up * moveSpeed * Time.deltaTime;
                break;
            case PlayerState.repair:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    //Testing States
                    playerState = PlayerState.moving;
                    Debug.Log("we In yippie");
                }
                break;
            case PlayerState.fire:
                break;
        } 
       
     

    }

    override protected void SceneFixedUpdate()
    {
    }
}
