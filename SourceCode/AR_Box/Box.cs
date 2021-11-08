using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*박스에 설치된 타워에 대한 동작 설명 스크립트*/
public class Box : MonoBehaviour
{
	public Color selectedColor;
	public Color canNotBuildColor;
	public Vector3 positionOffset;

	[HideInInspector]
	public GameObject tower;
	//[HideInInspector]
	public TowerBlueprint towerBlueprint;
	[HideInInspector]
	public bool isFullUpgraded = false;
	[HideInInspector]
	public int towerLevel = 0;

	private Renderer rend;
	private Color defaultColor;

	BuildManager buildManager;

	public GameObject boxCastlePrefab;
	GameObject boxCastle;
	void Start()
	{
		rend = GetComponent<Renderer>();
		defaultColor = rend.material.color;

		buildManager = BuildManager.instance;
	}

	public void BuildCastle(Quaternion rot)
	{
		if (boxCastle != null)
		{
			Destroy(boxCastle);
		}
		Vector3 buildPosTemp = new Vector3(0, 0.025f, 0);
		boxCastle = (GameObject)Instantiate(boxCastlePrefab, transform.position - buildPosTemp, rot);
	}
	public void DestroyCastle()
	{
		if (boxCastle != null)
		{
			Destroy(boxCastle);
			boxCastle = null;
		}
	}
	public Vector3 GetBuildPosition()
	{
		return transform.position + positionOffset;
	}

	public void BuildTower(TowerBlueprint blueprint)
	{
		
		if (PlayerStats.Money < blueprint.buildCost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

		PlayerStats.Money -= blueprint.buildCost;
		
		GameObject _tower = (GameObject)Instantiate(blueprint.towerLevel1Prefab, GetBuildPosition(), Quaternion.Euler(new Vector3(0,180,0)));
		tower = _tower;
		towerLevel = 1;
		towerBlueprint = blueprint;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Debug.Log("Turret build!");
	}

	public void UpgradeTower()
	{

		if (PlayerStats.Money < towerBlueprint.GetUpgradeCost(towerLevel + 1))
		{
			Debug.Log("Not enough money to upgrade that!");
			return;
		}

		PlayerStats.Money -= towerBlueprint.GetUpgradeCost(towerLevel + 1);
		
		Destroy(tower);
		if (towerLevel == 1)
		{
			towerLevel = 2;
		}
		else if (towerLevel == 2)
		{
			towerLevel = 3;
			isFullUpgraded = true;
		}
		else
		{
			Debug.Log("tower doesn't exist!");
			return;
		}
		GameObject _tower = (GameObject)Instantiate(towerBlueprint.GetPrefab(towerLevel), GetBuildPosition(), Quaternion.Euler(new Vector3(0, 180, 0)));
		tower = _tower;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Debug.Log("Tower upgraded!");
	}

	public void SellTower()
	{
		PlayerStats.Money += towerBlueprint.GetSellAmount(towerLevel);

		GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(effect, 5f);

		Destroy(tower);
		isFullUpgraded = false;
		towerLevel = 0;
		towerBlueprint = null;

	}

	private void OnDestroy()
	{
		SellTower();
	}
}
