using UnityEngine;
using UnityEngine.UI;
/*현재 Life를 보여준다.*/
public class LivesUI : MonoBehaviour
{

	public Text livesText;

	// Update is called once per frame
	void Update()
	{
		livesText.text = "Lives : " + PlayerStats.Lives.ToString();
	}
}
