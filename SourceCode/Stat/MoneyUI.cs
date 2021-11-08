using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*현재 Money를 보여준다.*/
public class MoneyUI : MonoBehaviour
{

	public Text moneyText;

	// Update is called once per frame
	void Update()
	{
		moneyText.text = PlayerStats.Money.ToString() + " G";
	}
}
