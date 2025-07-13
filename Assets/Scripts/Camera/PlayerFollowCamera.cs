using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    PlayController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<PlayController>();
        Debug.Log(controller.name);

        if(controller == null)
        {
            Debug.Log("No player brah!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = controller.transform.position - Vector3.forward*30;
    }
}
