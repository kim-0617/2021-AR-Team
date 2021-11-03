using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*게임에서 타워선택이 됐을 때 선택된 타워를 박스 위 소환가능하게 하는 스크립트
 선택 되었을 때 빌드매니저 스크립트에서 타워 소환*/
public class ArmyShop : MonoBehaviour
{
	public TowerBlueprint standardTower; 
	public TowerBlueprint second_Tower;
	public TowerBlueprint third_Tower;
	public TowerBlueprint fourth_Tower;
	BuildManager buildManager;

	void Start()
	{
		buildManager = BuildManager.instance;
	}

	public void SelectStandardTower() 
	{
		buildManager.SelectTowerToBuild(standardTower);
	}
	public void SelectSecondTower()
	{
		buildManager.SelectTowerToBuild(second_Tower);
	}
	public void SelectThirdTower()
	{
		buildManager.SelectTowerToBuild(third_Tower);
	}
	public void SelectFourthTower()
	{
		buildManager.SelectTowerToBuild(fourth_Tower);
	}
}
