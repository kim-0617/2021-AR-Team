using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;
/*트래킹하는 오브젝트에 대한 오퍼레이션이 정의된 스크립트*/
public class TrackingObjectHandler : MonoBehaviour
{
    public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
    }
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked;

    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;

    protected TrackableBehaviour mTrackableBehaviour;       
    protected TrackableBehaviour.Status m_PreviousStatus;   //이전 상태값
    protected TrackableBehaviour.Status m_NewStatus;        //새로운 상태값
    protected bool m_CallbackReceivedOnce = false;

    [Header("Prefab Set")]
    public GameObject spawnPrefab;    //등록프리팹
    private GameObject spawnObject;  //트래킹오브젝트(MultiTarget) 기준으로 생성
   
    [Header("Position Set")]
    public Vector3 buildPosition;   //트래킹위치 기준 설치위치 조정값
    private Vector3 lastPos;        //트래킹의 마지막위치
    private Quaternion lastRotation;

    private bool isTracked = false;
    public bool isActive = true;

    BoxListManager boxListManager;

    private void Update()
    {
        if (isActive)
        {
            if (isTracked)
            {
                if (MoveSpawnObject())                //Trakcing중일때는 실시간으로 위치변경
                {
                    SetLastPosition();
                }
            }
        }
        else
        {
            ResetSpawnObject();
        }
    }

    protected virtual void Start()
    {
        boxListManager = BoxListManager.instance;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
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

	private void OnEnable()
	{
        boxListManager.countTracked = 0;
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
        }
        else if (ShouldBeRendered(m_PreviousStatus) &&
                 !ShouldBeRendered(m_NewStatus))
        {
            OnTrackingLost();
        }
        else
        {
            if (!m_CallbackReceivedOnce && !ShouldBeRendered(m_NewStatus))
            {
                OnTrackingLost();
            }
        }

        m_CallbackReceivedOnce = true;
    }

    protected bool ShouldBeRendered(TrackableBehaviour.Status status)
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED)
        {
            return true;
        }

        return false;
    }

    protected virtual void OnTrackingFound()
    {
        isTracked = true;
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);
           
            foreach (var component in rendererComponents)
                component.enabled = true;
            foreach (var component in colliderComponents)
                component.enabled = true;
            foreach (var component in canvasComponents)
                component.enabled = true;

            //-----------------------------------------------------------------------------------------
            if (isActive)
            {
                SpawnObjectOnCube(spawnPrefab);     //트래킹하는 상자위에 생성하고 
                                                    //트래킹이 중간에 실패하면 실패하기직전위치에 그대로 위치시킨다
                boxListManager.CountTrackingBox(true);  //Tracking box count +
            }
        }
        if (OnTargetFound != null)
            OnTargetFound.Invoke();

       
    }

    protected virtual void OnTrackingLost()
    {
        isTracked = false;
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            foreach (var component in rendererComponents)
                component.enabled = false;
            foreach (var component in colliderComponents)
                component.enabled = false;
            foreach (var component in canvasComponents)
                component.enabled = false;

            //-----------------------------------------------------------------------------------------
            if (isActive)
            {
                MoveSpawnObject();

                if (m_CallbackReceivedOnce)
                    boxListManager.CountTrackingBox(false);  //count해주는 함수// 음수일경우 0부터시작
            }
        }
        if (OnTargetLost != null)
            OnTargetLost.Invoke();

    }

    void SpawnObjectOnCube(GameObject prefab)      //오브젝트 생성
    {
        SetLastPosition();
        if (!MoveSpawnObject())            //기존 오브젝트 없으면 새로생성
        {
            spawnObject = (GameObject)Instantiate(prefab, lastPos, lastRotation);
            Debug.Log("Spawn New Object!");
        }
    }

    private void SetLastPosition()
    {
        lastPos = transform.position + buildPosition;
        lastRotation = transform.rotation;
    }

    private bool MoveSpawnObject()
    {
        if (spawnObject != null)
        {
            spawnObject.transform.position = lastPos;
            spawnObject.transform.rotation = lastRotation;
            return true;
        }
        return false;
    }

    public GameObject GetSpawnObject()
    {
        if (spawnObject != null)
        {
            return spawnObject;
        }
        return null;
    }

    public void ResetSpawnObject()
    {
        if (spawnObject != null)
        {
            Destroy(spawnObject);
        }
    }
    public void RespawnObject()
    {
        if (isActive){
            if (isTracked)
            {
                if (spawnObject == null)
                {
                    SpawnObjectOnCube(spawnPrefab);
                }
            }
        }
    }
    public bool GetisTracked()
    {
        return isTracked;
    }

}
