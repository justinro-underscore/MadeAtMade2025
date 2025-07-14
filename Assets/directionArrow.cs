using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class directionArrow : MonoBehaviour
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
        Vector3 directionVector = new Vector2(0, 0);
        Vector3 targetVector = target.position - gameObject.transform.position;
        //float stepValue = 

        Vector3.RotateTowards(directionVector, targetVector, 7, 0.0f);
        transform.LookAt(directionVector, new Vector3(0, 0, 1));
    }
}
