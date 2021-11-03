using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MoveARSessionPivot : MonoBehaviour
{

    public Transform worldCenter;
	public ARRaycastManager raycastManager;
    public ARSessionOrigin arSessionOrigin; // 중심 변경 메소드를 사용할 SessionOrigin 변수
	private List<ARRaycastHit> hits = new List<ARRaycastHit>();


	private void Update()
	{
		if (Input.touchCount == 0)
		{

			return;
		}
		Touch touch = Input.GetTouch(0);

		if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
		{
			Pose hitPose = hits[0].pose;
			MovePivotToTrackingPose(hitPose);
		}
	}

	public void MovePivotToTrackingPose(Pose pose)
    {
        arSessionOrigin.MakeContentAppearAt(worldCenter, pose.position, pose.rotation);   //현재 위치를 변경
    }
}
