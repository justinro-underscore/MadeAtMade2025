using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 
using UnityEngine.UI;

public class PlayController : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }
    


    public float rotationSpeed = 90f;
    public float moveSpeed = 2f;
    
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
      
       
       if (Input.GetKey(KeyCode.A))
        {
            // Rotate left around Z axis
            transform.Rotate(0,0 , rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Rotate left around Z axis
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        transform.position += transform.up * moveSpeed * Time.deltaTime;

    }

    override protected void SceneFixedUpdate()
    {
    }
}
