using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 이동 경로를 따라 자취를 남기는 오브젝트를 이동시킨다. */
public class PathVisualizer : MonoBehaviour
{
    private Transform target;
    public float speed = 0.2f;
    Vector3[] path;
    int targetIndex;
    PathVisualizerManager pathVisualizerManager;


    private void Start()
    {
        target = WorldCenter.instance.centerObject.transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        pathVisualizerManager = PathVisualizerManager.instance;

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
        Destroy(gameObject);
        pathVisualizerManager.RestartPathVisual();
    }
}
