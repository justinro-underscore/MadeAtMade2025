using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBall : MonoBehaviour
{
    [SerializeField]
    private float movespeed = 10f;
    // Update is called once per frame
    void Update()
    {

        transform.position += transform.up * movespeed * Time.deltaTime;
    }
}
