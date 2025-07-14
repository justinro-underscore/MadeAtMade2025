using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleHealth : MonoBehaviour, IHealth
{
    public int maxHealth;
    private int health = 1;

    //private CapsuleCollider2D selfCollider;selfCollider.

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //selfCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void damage(int damage) //does this need to be public for the interface to work?
    {
        health -= damage;
        if (health == 0)
        {
            noHealth();
        }
    }

    public void noHealth()
    {
        Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject.tag == "cannonball")
        {
            damage(1);
            Destroy(collidingObject);
        }
    }

}
