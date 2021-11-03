using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*타워가 소환되어 있는 박스UI를 클릭하여 타워를 업그레이드 하거나 팔 수 있게 하는 스크립트*/

public class BoxUI : MonoBehaviour
{
	public GameObject ui;

	public Text upgradeCost;
	public Button upgradeButton;
	public Text sellAmount;
	private Box target;
	public Vector3 uiPosition = new Vector3(0,0.065f,0);

	public void SetTarget(Box _target)
	{
		target = _target;
		transform.position = target.GetBuildPosition() + uiPosition;

		if (!target.isFullUpgraded)
		{
			upgradeCost.text = target.towerBlueprint.GetUpgradeCost(target.towerLevel + 1) + " G";	//다음 레벨의 타워 업그레이드 비용 반환
			upgradeButton.interactable = true;
		}
		else // 업그레이드
		{
			upgradeCost.text = "DONE";
			upgradeButton.interactable = false;
		}

		sellAmount.text = target.towerBlueprint.GetSellAmount(target.towerLevel) + " G";

		ui.SetActive(true);
	}

	public void Hide()
	{
		ui.SetActive(false);
	}

	public void Upgrade() // 업그레이드
	{
		target.UpgradeTower();
		BuildManager.instance.DeselectBox();
	}

	public void Sell() // 판매
	{
		target.SellTower();
		BuildManager.instance.DeselectBox();
	}

}
