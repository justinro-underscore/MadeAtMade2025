using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObstacleHealth obstacleHealth = gameObject.GetComponent<ObstacleHealth>();

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayController>().RepairEntryState();
            obstacleHealth.OnDamage(5);
        }
        else if (collision.gameObject.CompareTag("Cannonball"))
        {
            Debug.Log("Hit this obstacle " + gameObject.name);
            obstacleHealth.OnDamage(1);
        }
    }
}
