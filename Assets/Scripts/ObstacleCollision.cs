using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObstacleHealth obstacleHealth = gameObject.GetComponent<ObstacleHealth>();

        if (collision.gameObject.CompareTag("Player"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerDamage");
            collision.gameObject.GetComponent<PlayController>().RepairEntryState();
            obstacleHealth.OnDamage(5);
            
        }
        else if (collision.gameObject.CompareTag("Cannonball"))
        {
            
            Debug.Log("Hit this obstacle " + gameObject.name);
            obstacleHealth.OnDamage(1);
            Destroy(collision.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/EnemyDamage");
            
        }
    }
}
