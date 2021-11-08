using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
/*트래킹과 저장이 완료되면 관련 UI를 OFF 한다*/
public class TrackingModeChangeUI : MonoBehaviour
{
	public GameObject trackingModeUI;

	public Button modeChangeButton;
	public Text modeStatus;
	private bool mode = true;

	public List<GameObject> multiTargets;
	public GameObject ImgTarget;

	public Material tracking_BoxMaterial;		//테두리
	public Material inGame_BoxMaterial;         //DepthMask

	public Vector3 tracking_BoxScale = new Vector3(0.051f,0.051f,0.051f);
	public Vector3 inGame_BoxScale = new Vector3(0.049f, 0.049f, 0.049f);

	public AGrid grid;


	public void ModeChange()	//모드변경 버튼이 눌리면 
	{
		if (mode)
		{   //true->false

			modeStatus.text = "Off";
			mode = false;
			trackingModeUI.SetActive(false);

			foreach (GameObject target in multiTargets)
			{
				target.GetComponent<TrackingObjectHandler>().isActive = false;
			}
			ImgTarget.GetComponent<TrackingBoardHandler>().isActive = false;
			grid.HideGrid();
			BoxListManager.instance.ChangeMaterial(inGame_BoxMaterial); // 박스 재질 변경 
			BoxListManager.instance.ChangeScale(inGame_BoxScale);

			return;
		}
		else
		{   //false->true
			modeStatus.text = "On";
			mode = true;
			trackingModeUI.SetActive(true);

			foreach (GameObject target in multiTargets)
			{
				target.GetComponent<TrackingObjectHandler>().isActive = true;
				target.GetComponent<TrackingObjectHandler>().RespawnObject();
			}

			
			ImgTarget.GetComponent<TrackingBoardHandler>().isActive = true;
			grid.ShowGrid();

			BoxListManager.instance.ChangeMaterial(tracking_BoxMaterial);
			BoxListManager.instance.ChangeScale(tracking_BoxScale);

			return;
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
