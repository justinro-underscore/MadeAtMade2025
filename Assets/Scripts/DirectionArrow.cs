using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionVector = new Vector3(0, 0, 0);
        Vector3 targetVector = (target.position - transform.position).normalized;
        float stepValue = 1.0f * Time.deltaTime;

        //directionVector = Vector3.RotateTowards(transform.forward, targetVector, 100, 0.0f);
        //print(transform.up + ", " + directionVector + ", " + targetVector);
        transform.up = targetVector;//, Vector3.forward * -1); //rotation = Quaternion.LookRotation(directionVector); //
    }
}
