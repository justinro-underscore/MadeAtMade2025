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

    //Cannon
    [SerializeField]
    private GameObject cannonBall;
    //Move Variables.
    [SerializeField]
    private float rotationSpeed = 90f;
    [SerializeField]
    private float moveSpeed = 2f;

    private float shipAngle;

    //State Machine
    private PlayerState playerState = PlayerState.moving;
    public static PlayController Instance { get; private set; }

    protected void Awake()
    {
        if (!Instance)
            Instance = this;

        else
            Destroy(gameObject);
    }
   
    override protected async void SceneUpdate()
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
                if (Input.GetKeyDown(KeyCode.O))
                {
                    //Testing States
                    playerState = PlayerState.fire;
                }
                transform.localEulerAngles = new Vector3(0, 0, shipAngle);
                //  transform.position += transform.up * moveSpeed * Time.deltaTime;
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

                Instantiate(cannonBall, transform.position, transform.rotation);
                
                playerState = PlayerState.moving;
                transform.localEulerAngles.z = 20;

               
                break;
        } 
       
     

    }

    override protected void SceneFixedUpdate()
    {
    }
}
