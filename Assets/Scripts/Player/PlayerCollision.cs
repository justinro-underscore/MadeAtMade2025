using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayController controller;

    private void Start()
    {
        controller = gameObject.GetComponent<PlayController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I hit... " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            controller.RepairEntryState();
        }
    }
}
