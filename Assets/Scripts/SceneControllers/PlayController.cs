using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine; 
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

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
    [SerializeField] private float moveSpeed;

    private float maxMoveSpeed = 25f;

    private float targetAngle;

    private float repairCount = 0;
    

    //State Machine
    private PlayerState playerState = PlayerState.moving;
    public PlayerState PlayerState { get; set; }

    List<IEnumerator> lerpCoroutineQueue = new List<IEnumerator> ();
    Coroutine currentLerpCoroutine = null;

    private float lerpDuration = 1.0f;
    public static PlayController Instance { get; private set; }

    protected void Awake()
    {
        if (!Instance)
            Instance = this;

        else
            Destroy(gameObject);

        moveSpeed = maxMoveSpeed;
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

                    QueueLerpCoroutine(playerState, moveSpeed,0);
                    repairCount++;
                }
                if(Input.GetKeyDown(KeyCode.O))
                {
                    playerState = PlayerState.fire;
                    QueueLerpCoroutine(playerState, moveSpeed, 0);
                }

                if(moveSpeed == maxMoveSpeed)
                {
                    transform.localEulerAngles = new Vector3(0, 0, targetAngle);
                    transform.position += transform.up * moveSpeed * Time.deltaTime;
                }
                break;
            case PlayerState.repair:
                //Testing States
                if (Input.GetKeyDown(KeyCode.P) && repairCount < 10)
                {
                    repairCount++;
                }
                else if(repairCount == 10)
                {
                    repairCount = 0;
                    playerState = PlayerState.moving;
                    QueueLerpCoroutine(playerState, 0, maxMoveSpeed);
                    Debug.Log("we In yippie");
                }
                break;
            case PlayerState.fire:

      
                //12
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    FireFunction(0);
                }
                //1
                if (Input.GetKeyDown(KeyCode.W))
                {
                    FireFunction(330);
                }
                //2
                if (Input.GetKeyDown(KeyCode.E))
                {
                    FireFunction(300);
                }
                //3
                if (Input.GetKeyDown(KeyCode.R))
                {
                    FireFunction(270);
                }
                //4
                if (Input.GetKeyDown(KeyCode.T))
                {
                    FireFunction(240);
                }
                //5
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    FireFunction(210);
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    FireFunction(180);
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    FireFunction(150);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    FireFunction(120);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    FireFunction(90);
                }
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                {
                    FireFunction(60);
                }
                if (Input.GetKeyDown(KeyCode.RightBracket))
                {
                    FireFunction(30);
                }

                break;
        }
    }

    void FireFunction(float fireAngle)
    {
        Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, fireAngle));
                QueueLerpCoroutine(PlayerState.moving, 0, maxMoveSpeed);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot");
    }

    IEnumerator Lerp(PlayerState entryPlayerState, float startSpeed, float endSpeed)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            Debug.Log(moveSpeed);
            transform.localEulerAngles = new Vector3(0, 0, targetAngle);
            transform.position += transform.up * moveSpeed * Time.deltaTime;

            moveSpeed = Mathf.Lerp(startSpeed, endSpeed, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        if (entryPlayerState == PlayerState.moving)
        {
            moveSpeed = maxMoveSpeed;
            playerState = PlayerState.moving;
        }
        else
        {
            moveSpeed = 0;
        }

        lerpCoroutineQueue.RemoveAt(0);

        if(lerpCoroutineQueue.Count > 0)
        {
            StartCoroutine(lerpCoroutineQueue[0]);
        }
    }

    private void QueueLerpCoroutine(PlayerState entryPlayerState, float startSpeed, float endSpeed)
    {
        lerpCoroutineQueue.Add(Lerp(entryPlayerState, startSpeed, endSpeed));

       if(currentLerpCoroutine == null)
        {
            StartCoroutine(lerpCoroutineQueue[0]);
        }
    }

    override protected void SceneFixedUpdate()
    {
    }
}