using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;
/*트래킹하는 보드에 관한 오퍼레이션이 정의된 스크립트*/
public class TrackingBoardHandler : MonoBehaviour
{
    public enum TrackingStatusFilter
    {
        Tracked
    }
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked;

    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    protected bool m_CallbackReceivedOnce = false;

    [Header("Prefab Set")]
    public GameObject spawnPrefab;    //등록프리팹
    public Transform worldCenter;   //세계중심이되는 오브젝트 (0,0,0)
    private GameObject spawnObject;  //생성한 오브젝트

    [Header("Position Set")]
    public Vector3 buildPosition;   //트래킹위치 기준 설치위치 조정값
    private Vector3 lastPos;        //트래킹의 마지막위치
    private Quaternion lastRotation;
    public Vector3 worldCenterMovePos;

    private bool isTracked = false;
    public bool isActive = true;
    public AGrid grid;

    public BoardScanUI boardScanUI;

    private void Update()
    {
        if (isActive)
        {
            if (isTracked)
            {
                if (MoveSpawnObject())                                //Trakcing중일때는 실시간으로 위치변경
                {
                    SetLastPosition();
                }
            }
        }
        else
        {
           
        }
    }

    protected virtual void Start()
    {
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
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
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
            // always render the augmentation when status is DETECTED or TRACKED, regardless of filter
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
        if (isActive)
        {
            SpawnObjectOnBoard(spawnPrefab);     //트래킹하는 보드위에 생성하고 
                                                 //트래킹이 중간에 실패하면 실패하기직전위치에 그대로 위치시킨다
            grid.CreateGrid();
        }
    }

    protected virtual void OnTrackingLost()
    {
        isTracked = false;
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
        if (isActive)
        {
            MoveSpawnObject();
        }
    }



    void SpawnObjectOnBoard(GameObject prefab)      //오브젝트 생성
    {
        SetLastPosition();
        if (!MoveSpawnObject())            //기존 오브젝트 없으면 활성화
        {
            ActivateSpawnObject();
            Debug.Log("Spawn New Object!");
            MoveWorldCenter();
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
            MoveWorldCenter();
            return true;
        }
        return false;
    }
    private void MoveWorldCenter()
    {
        worldCenter.position = transform.position + worldCenterMovePos;
        worldCenter.rotation = lastRotation;
        WorldCenter.instance.centerObject.transform.position = worldCenter.position;
        WorldCenter.instance.centerObject.transform.rotation = worldCenter.rotation;
    }
    public void ActivateSpawnObject()
    {
        if (!spawnPrefab.activeSelf)
        {
            spawnPrefab.SetActive(true);
            spawnObject = spawnPrefab;
            spawnObject.transform.position = transform.position + buildPosition;
            spawnObject.transform.rotation = transform.rotation;
            boardScanUI.ScanSuccess();
        }
        else
        {
            spawnObject = spawnPrefab;
            spawnObject.transform.position = transform.position + buildPosition;
            spawnObject.transform.rotation = transform.rotation;
        }
    }

}
