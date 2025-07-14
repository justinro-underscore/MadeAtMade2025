using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.ChangeState(GameState.END);
        }

    }
}
