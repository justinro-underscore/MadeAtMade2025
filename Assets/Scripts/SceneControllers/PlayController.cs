using System.Collections.Generic;
using Unity.Mathematics;
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

    private float targetAngle;
    

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
                    targetAngle += rotationSpeed *Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    targetAngle -= rotationSpeed * Time.deltaTime;
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
                transform.localEulerAngles = new Vector3(0, 0, targetAngle);
              //  transform.position += transform.up * moveSpeed * Time.deltaTime;
                break;
            case PlayerState.repair:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    //Testing States
                    playerState = PlayerState.moving;
                    Debug.Log("we In yippie");
                  //  transform.localEulerAngles.z = 20;
                }
                break;
            case PlayerState.fire:

      
                //12
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Instantiate(cannonBall, transform.position,Quaternion.Euler(0, 0,0));
                    playerState = PlayerState.moving;
                }
                //1
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 330));
                    playerState = PlayerState.moving;
                }
                //2
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 300));
                    playerState = PlayerState.moving;
                }
                //3
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 270));
                    playerState = PlayerState.moving;
                }
                //4
                if (Input.GetKeyDown(KeyCode.T))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 240));
                    playerState = PlayerState.moving;
                }
                //5
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 210));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 180));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 150));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 120));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 90));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 60));
                    playerState = PlayerState.moving;
                }
                if (Input.GetKeyDown(KeyCode.RightBracket))
                {
                    Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, 30));
                    playerState = PlayerState.moving;
                }





                break;
        } 
       
     

    }

    override protected void SceneFixedUpdate()
    {
    }
}
