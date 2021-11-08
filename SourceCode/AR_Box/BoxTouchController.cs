using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Box가 터치되었을 때 상호작용을 관리한다. */
public class BoxTouchController : MonoBehaviour
{

	BuildManager buildManager;
	private void Start()
	{
		buildManager = BuildManager.instance;
	}

	private void Update()  
	{
		if (Input.touchCount == 0)
		{
			return;
		}
		else if (Input.touches[0].phase == TouchPhase.Began)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				var selection = hit.transform;
				var selectionRenderer = selection.GetComponent<Renderer>();
				var selectionComponent = selection.GetComponent<Box>();

				if (selectionComponent.tower != null)
				{
					buildManager.SelectBox(selectionComponent);
					return;
				}

				if (!buildManager.CanBuild)
					return;

				selectionComponent.BuildTower(buildManager.GetTowerToBuild());
			}
		}



	}

}
