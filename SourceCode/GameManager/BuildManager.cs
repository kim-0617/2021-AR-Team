using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*박스를 선택하거나 박스에 타워를 설치할 때 관리하는 스크립트*/
public class BuildManager : MonoBehaviour
{

	public static BuildManager instance;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("BuildManager can exist only one in a scene!");
			return;
		}
		instance = this;
	}

	public GameObject buildEffect;
	public GameObject sellEffect;

	private TowerBlueprint towerToBuild;
	private Box selectedBox;

	public BoxUI boxUI;

	public bool CanBuild { get { return towerToBuild != null; } }
	
	public bool HasMoney { get { return true; } }
	//public bool HasMoney { get { return PlayerStats.Money >= towerToBuild.buildCost; } }

	public void SelectBox(Box box) // 박스 선택 시
	{
		if (selectedBox == box)
		{
			DeselectBox();
			return;
		}

		selectedBox = box;
		towerToBuild = null;

		boxUI.SetTarget(box);
	}

	public void DeselectBox()
	{
		selectedBox = null;
		boxUI.Hide();
	}

	public void SelectTowerToBuild(TowerBlueprint tower) // 선택한 타워 건설
	{
		towerToBuild = tower;
		DeselectBox();
	}

	public TowerBlueprint GetTowerToBuild()
	{
		return towerToBuild;
	}
}
