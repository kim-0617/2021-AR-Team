using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class TrackingBox : MonoBehaviour
{
    public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    }
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;
  
    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    protected bool m_CallbackReceivedOnce = false;

    [Header("Prefab Set")]
    public GameObject slime_prefab;     //등록프리팹
    public GameObject turtle_prefab;    //등록프리팹
    public GameObject WorldCenter;  //세계중심(캡슐)이되는 오브젝트 (0,0,0)
    private GameObject slime;       //슬라임은 WorldCenter(캡슐)기준으로 생성
    private GameObject turtle;      //터틀은 트래킹오브젝트(MultiTarget) 기준으로 생성
   
    [Header("Position Set")]
    public Vector3 pos_slime;               //캡슐기준 슬라임 설치위치 조정값
    public Vector3 pos_turtle;              //트래킹큐브기준 터틀 설치위치 조정값
    private Vector3 lastPos;        //트래킹의 마지막위치


    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        lastPos = new Vector3(99,99,99);
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.UnregisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        m_PreviousStatus = statusChangeResult.PreviousStatus;
        m_NewStatus = statusChangeResult.NewStatus;

        Debug.LogFormat("Trackable {0} {1} -- {2}",
            mTrackableBehaviour.TrackableName,
            mTrackableBehaviour.CurrentStatus,
            mTrackableBehaviour.CurrentStatusInfo);

        HandleTrackableStatusChanged();
    }

    protected virtual void HandleTrackableStatusChanged()
    {
        if (!ShouldBeRendered(m_PreviousStatus) &&
            ShouldBeRendered(m_NewStatus))
        {
            OnTrackingFound();                  //트래킹상태
            SpawnSlime(slime_prefab);           //슬라임
            SpawnTurtleOnCube(turtle_prefab);   //터틀(트래킹하는 상자위에 생성하고 
                                                    //트래킹이 중간에 실패하면 실패하기직전위치에 그대로 위치시킨다
        }
        else if (ShouldBeRendered(m_PreviousStatus) &&
                 !ShouldBeRendered(m_NewStatus))
        {
            OnTrackingLost();       
            if (slime != null)      //트래킹을 놓치면 slime제거 
            {
                Destroy(slime);
            }
            if (turtle != null)     //터틀은 트래킹 마지막위치에 고정소환
            {
                Destroy(turtle);
                if (lastPos != Vector3.zero)       
                {
                    turtle = (GameObject)Instantiate(turtle_prefab, lastPos, Quaternion.identity); //트래킹끝나기 마지막 위치에 고정스폰시킴
                    Debug.Log("Tracking Lost Turtle Spawn!");
                }
            }
        }
        else
        {
            if (!m_CallbackReceivedOnce && !ShouldBeRendered(m_NewStatus))
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
                if (slime != null)      //트래킹 놓치면 slime제거
                {
                    Destroy(slime);
                }
                if (turtle != null)     //터틀은 트래킹 마지막위치에 고정소환
                {
                    Destroy(turtle);
                    if (lastPos != Vector3.zero)
                    {
                        turtle = (GameObject)Instantiate(turtle_prefab, lastPos, Quaternion.identity); //트래킹끝나기 마지막 위치에 고정스폰시킴
                        Debug.Log("Tracking Lost Turtle Spawn!");
                    }
                }
            }
        }

        m_CallbackReceivedOnce = true;
    }

    protected bool ShouldBeRendered(TrackableBehaviour.Status status)
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED)
        {
            // always render the augmentation when status is DETECTED or TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                // also return true if the target is extended tracked
                return true;
            }
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED ||
                status == TrackableBehaviour.Status.LIMITED)
            {
                // in this mode, render the augmentation even if the target's tracking status is LIMITED.
                // this is mainly recommended for Anchors.
                return true;
            }
        }

        return false;
    }

    protected virtual void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
        }

        if (OnTargetFound != null)
            OnTargetFound.Invoke();
    }

    protected virtual void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }

        if (OnTargetLost != null)
            OnTargetLost.Invoke();
    }


    void SpawnSlime(GameObject prefab)      //슬라임 생성
    {
        if (slime != null)      //기존 오브젝트가있으면 기존에있던 오브젝트 제거 후 새로생성  (중복생성안되도록)
        {
            Destroy(slime);
            slime = (GameObject)Instantiate(prefab, GetSpawn_WorldPosition(), Quaternion.identity); //세계중심(캡슐)기준
            Debug.Log("Spawn Slime again!");
        }
		else
        {                       //기존 오브젝트 없으면 새로생성
            slime = (GameObject)Instantiate(prefab, GetSpawn_WorldPosition(), Quaternion.identity);
            Debug.Log("Spawn New Slime!");
        }
    }
    void SpawnTurtleOnCube(GameObject prefab)      //터틀생성
    {
        lastPos = GetSpawn_TrackingPosition();      //tracking좌표기준 생성지점 저장
        if (turtle != null)      //기존 오브젝트가있으면 기존에있던 오브젝트 제거 후 새로생성  (중복생성안되도록)
        {
            Destroy(turtle);
            turtle = (GameObject)Instantiate(prefab, lastPos, Quaternion.identity); 
            Debug.Log("Spawn Turtle again!");
        }
        else
        {                       //기존 오브젝트 없으면 새로생성
            turtle = (GameObject)Instantiate(prefab, lastPos, Quaternion.identity);
            Debug.Log("Spawn New Turtle!");
        }
    }

    public Vector3 GetSpawn_WorldPosition()
    {
        return WorldCenter.transform.position + pos_slime;
    }
    public Vector3 GetSpawn_TrackingPosition()
    {
        return transform.position + pos_turtle;
    }

}
