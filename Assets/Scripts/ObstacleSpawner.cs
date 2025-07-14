using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject staticObstacleSmall;
    public GameObject staticObstacleMedium;
    public GameObject staticObstacleLarge;
    //public Vector2 staticObstacleSmallSize;
    //public int staticSmallWidth;
    //public int staticSmallHeight;
    private CapsuleCollider2D staticObstacleSmallCollider;
    public Vector2 levelSize;

    public GameObject endIsland;
    public Transform endIslandSpawn;
    public CircleCollider2D startingArea;
    public Transform obstacleHolder;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 endPos = new Vector2(UnityEngine.Random.Range(-levelSize.x / 2, levelSize.x / 2), UnityEngine.Random.Range(400, levelSize.y / 2));
        //while (startingArea.bounds.Contains(endPos))
        //{
        //    endPos = new Vector2(UnityEngine.Random.Range(-levelSize.x / 2, levelSize.x / 2), UnityEngine.Random.Range(levelSize.y, levelSize.y - 100));
        //    print("finding new end island location");
        //}
        endIslandSpawn.position = endPos;
        Instantiate(endIsland, endIslandSpawn.position, endIslandSpawn.rotation, obstacleHolder);

        staticObstacleSmallCollider = staticObstacleLarge.GetComponent<CapsuleCollider2D>();
        for (int x = Mathf.RoundToInt(-levelSize.x/2); x < levelSize.x/2; x += Mathf.RoundToInt(staticObstacleSmallCollider.size.x))
        {
            for (int y = Mathf.RoundToInt(-levelSize.y/2); y < levelSize.y/2; y += Mathf.RoundToInt(staticObstacleSmallCollider.size.y))
            {
                if (UnityEngine.Random.value > 0.999 && !startingArea.bounds.Contains(new Vector2(x, y)))
                {
                    Vector2 pos = new Vector2(x, y);
                    int obsType = UnityEngine.Random.Range(0, 3);
                    switch (obsType)
                    {
                        case 0: //small static obstacle
                            Instantiate(staticObstacleSmall, pos, Quaternion.identity, obstacleHolder);
                            break;
                        case 1: //medium static obstacle
                            Instantiate(staticObstacleMedium, pos, Quaternion.identity, obstacleHolder);
                            break;
                        case 2: //landmass/large static obstacle
                            Instantiate(staticObstacleLarge, pos, Quaternion.identity, obstacleHolder);
                            break;
                        default:
                            print("error! no such obstacle type!");
                            break;
                    }


                }
            }
        }
        deleteInside(endIsland);
        GameObject bigCentralObstacle = Instantiate(staticObstacleLarge, new Vector2(0, 200), Quaternion.identity, obstacleHolder);
        deleteInside(bigCentralObstacle);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deleteInside(GameObject obj)
    {
        Collider[] colliderList = Physics.OverlapSphere(obj.transform.position, obj.transform.localScale.x);
        foreach (var colliderInside in colliderList)
        {
            Destroy(colliderInside.gameObject);
        }
    }
}
