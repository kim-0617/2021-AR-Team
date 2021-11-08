using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*박스를 트래킹해서 등록하거나 리셋할 수 있는 스크립트*/
public class TrackingModeUI : MonoBehaviour
{
	public Text trackingBoxNum;
	public Text savedBoxNum;
	public Button addBoxButton;

	public TrackingObjectHandler[] trackingObjectHandlers;

	BoxListManager boxListManager;

	private GameObject box;

	private void Start()
	{
		boxListManager = BoxListManager.instance;
	}

	private void Update()
	{
		ChangeText();
	}

	public void ChangeText() // 트래킹중인 박스나 저장된 박스가 몇개인가 알려주는 UI
	{
		trackingBoxNum.text = "Tracking Box : " + boxListManager.countTracked;
		savedBoxNum.text = "Saved Box : " + boxListManager.CountSavedBox();
	}
	public void AddBox() // 트래킹중인 박스를 저장한다
	{
		foreach (TrackingObjectHandler handler in trackingObjectHandlers)
		{
			box = handler.GetSpawnObject();
			if (box != null)
			{
				boxListManager.AddBoxInList(box);
				handler.ResetSpawnObject();
			}
			
		}
	}

	public void ResetBoxList() // 저장된 박스를 모두 해제한다.
	{
		boxListManager.ResetAll();
		foreach (TrackingObjectHandler handler in trackingObjectHandlers)
		{
			box = handler.GetSpawnObject();
			if (box != null)
			{
				if (!handler.GetisTracked())
				{
					handler.ResetSpawnObject();
				}
			}
			
		}
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

}
