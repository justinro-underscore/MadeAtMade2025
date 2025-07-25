using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OnCollisionWithDWall : MonoBehaviour
{
    [SerializeField] float dWallSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayController>(out PlayController playController))
        {
            //Destroy(collision.gameObject);
            Debug.Log("Game Over BB!");
            GameController.instance.ChangeState(GameState.LOSE);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * dWallSpeed * Time.deltaTime;
    }
}
