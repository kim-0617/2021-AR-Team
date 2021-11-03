using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*가속도계를 이용해서 기기의 수평을 검사함*/
public class AccelerationUI : MonoBehaviour
{
	public Text accelX;
	public Text accelY;
	public Text accelZ;

	public Color disableColor = Color.red; // 수평이 아닐시 red
	public Color enableColor = Color.white; // 수평이 맞춰질 때 white

	//public GameObject PlayButton;
	Vector3 accelValue;
	public Button playButton;
	public Text buttonText;

	private void Start()
	{
		//myButton = GameObject.Find("PlayButton");
		//btn = PlayButton.GetComponent<Button>();
		ChangeTextColor(disableColor);
		playButton.interactable = false;
		
	}
	
	public void ShowValue()
	{
		accelValue = Input.acceleration;
		accelX.text = "Acceleration X : " + accelValue.x.ToString("N5");	//유효숫자5개
		accelY.text = "Acceleration Y : " + accelValue.y.ToString("N5");
		accelZ.text = "Acceleration Z : " + accelValue.z.ToString("N5");
	}

	private void Update()
	{
		ShowValue();
		CheckValue();
	}

	private void CheckValue()
	{
		if (accelValue.x < 0.02 && accelValue.x > -0.02 
			&& accelValue.y < 0.02 && accelValue.y > -0.02
			&& accelValue.z < -0.98) // 수평
		{
			playButton.interactable = true;
			ChangeTextColor(enableColor);
		}
		else
		{
			ChangeTextColor(disableColor);
			playButton.interactable = false;
		}
	}

	private void ChangeTextColor(Color changeColor)
	{
		/*
		ColorBlock cb = btn.colors;
		cb.normalColor = changeColor;
		btn.colors = cb;
		*/

		buttonText.color = changeColor;
	}
}
