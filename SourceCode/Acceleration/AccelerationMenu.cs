using UnityEngine;
using UnityEngine.SceneManagement;
/*Acceleration Scene
 GameScene 전에 기기를 가속도 좌표를 바탕으로 수평에 맞춰야 시작 가능*/
public class AccelerationMenu : MonoBehaviour
{

	public string levelToLoad = "GameScene";
	public string levelToReturn = "MainMenu";
	public SceneFader sceneFader;

	public void Play() // 기기를 수평으로 맞추면 play 버튼이 활성화 된다.
	{
		sceneFader.FadeTo(levelToLoad);
	}

	public void Return() // 메인메뉴로 되돌아감
	{
		sceneFader.FadeTo(levelToReturn);
	}

}