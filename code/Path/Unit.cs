using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*몬스터의 이동경로와 속도에 대한 정보가 정의된 스크립트*/
public class Unit : MonoBehaviour
{
    
    private Transform target;
    public float speed = 0.2f; // 이동속도
    Vector3[] path;
    int targetIndex;

	private void Start()
    {
        target = WorldCenter.instance.centerObject.transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); // 코어로 가는 최단 경로로 이동
    } 

    private void Update()
    {
       
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    Endpath();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            //Debug.Log(path[targetIndex]);
            transform.rotation = Quaternion.LookRotation(-(transform.position - currentWaypoint).normalized);   //이동방향 바라보기
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
           
            yield return null;

        }
    }

    void Endpath()
    {
        PlayerStats.Lives--; // 몬스터가 path의 끝에 도달하게되면 Life 감소
        RoundSpawner.MonstersAlive--;
        Destroy(gameObject);

    }

}