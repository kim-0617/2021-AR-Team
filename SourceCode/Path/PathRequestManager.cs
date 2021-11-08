using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
/**/
public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    static PathRequestManager instance;
    public PathFinding pathfinding;
    bool isProcessingPath;
    private void Awake()
    {
        instance = this;
        if (pathfinding == null)
        {
            pathfinding = GetComponent<PathFinding>();
        }
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, UnityAction<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public UnityAction<Vector3[], bool> callback;

        public PathRequest(Vector3 nstart, Vector3 nend, UnityAction<Vector3[], bool> ncallback)
        {
            pathStart = nstart;
            pathEnd = nend;
            callback = ncallback;
        }
    }
}