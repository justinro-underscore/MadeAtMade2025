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
    }
   
    override protected async void SceneUpdate()
    {
        switch (playerState)
        {
            
            case PlayerState.moving:
            RuntimeManager.StudioSystem.setParameterByName("PlayerState", 0.0f);
                if (Input.GetKey(KeyCode.A))
                {
                    targetAngle += rotationSpeed *Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    targetAngle -= rotationSpeed * Time.deltaTime;
                }
                if(Input.GetKeyDown(KeyCode.Alpha0))
                {
                    playerState = PlayerState.fire;
                    RuntimeManager.StudioSystem.setParameterByName("PlayerState", 2.0f);
                    //QueueLerpCoroutine(playerState, moveSpeed, 0);
                    moveSpeed = 0;
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
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Repair");
                    Debug.Log($"Repair:{repairCount}");
                }
                else if(repairCount == 10)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Repaired");
                    repairCount = 0;
                    playerState = PlayerState.moving;
                    QueueLerpCoroutine(playerState, 0, maxMoveSpeed);
                }
                break;
            case PlayerState.fire:

      
                //12
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    FireFunction(30);
                }
                //1
                if (Input.GetKeyDown(KeyCode.W))
                {
                    FireFunction(330);
                }
              

                break;
        }
    }

    private void FireFunction(float fireAngle)
    {
        Instantiate(cannonBall, transform.position, Quaternion.Euler(0, 0, fireAngle));
        //QueueLerpCoroutine(PlayerState.moving, 0, maxMoveSpeed);
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
            lerpCoroutineQueue.Clear();
        }
        moveSpeed = maxMoveSpeed;
        playerState = PlayerState.moving;
        Debug.Log(moveSpeed);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot");
    }

    private IEnumerator Lerp(PlayerState entryPlayerState, float startSpeed, float endSpeed)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
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

    public void RepairEntryState()
    {
        playerState = PlayerState.repair;
        RuntimeManager.StudioSystem.setParameterByName("PlayerState", 1.0f);

        QueueLerpCoroutine(playerState, moveSpeed, 0);
        repairCount++;
    }

    private void QueueLerpCoroutine(PlayerState entryPlayerState, float startSpeed, float endSpeed)
    {
        lerpCoroutineQueue.Add(Lerp(entryPlayerState, startSpeed, endSpeed));

       if(currentLerpCoroutine == null)
        {
            StartCoroutine(lerpCoroutineQueue[0]);
        }
    }
}