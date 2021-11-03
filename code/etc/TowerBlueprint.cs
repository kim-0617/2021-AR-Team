using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*타워를 팔거나 업그레이드 할 때의 돈처리 담당*/
[System.Serializable]
public class TowerBlueprint
{
	public GameObject towerLevel1Prefab;
	public int buildCost;

	public GameObject towerLevel2Prefab;
	public int upgradeCost;

	public GameObject towerLevel3Prefab;
	public int upgrade2Cost;

	public int GetUpgradeCost(int level)
	{
		switch (level)
		{
			case 1: // 타워 건설
				return buildCost;

			case 2: // 업그레이드 1회
				return upgradeCost;

			case 3: // 업그레이드 2회
				return upgrade2Cost;
			default:
				return 0;
		}
	}
	public int GetSellAmount() // 판매시 원가의 반만 되돌려 줌
	{
		return buildCost / 2;
	}
	public int GetSellAmount(int level)
	{
		int total_upgradeCost;

		switch (level)
		{
			case 1:
				total_upgradeCost = buildCost;
				break;
			case 2:
				total_upgradeCost = buildCost + upgradeCost;
				break;
			case 3:
				total_upgradeCost = buildCost + upgradeCost + upgrade2Cost;
				break;
			default:
				return 0;
		}

		return total_upgradeCost / 2; // 업그레이드 타워 철거시 환불
	}
	public GameObject GetPrefab(int level)
	{ 
		switch(level) // 업그레이드 할 때마다 타워 프리팹 교체
		{
			case 1:
				return towerLevel1Prefab;

			case 2:
				return towerLevel2Prefab;

			case 3:
				return towerLevel3Prefab;
			default:
				return null;
		}
	}


}
