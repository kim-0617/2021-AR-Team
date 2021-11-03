using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*Game을 모두 클리어 했을 때 나오는 GameClear UI*/
public class GameClearUI : MonoBehaviour
{
	public string menuSceneName = "MainMenu";
	public SceneFader sceneFader;
	public Text rounds_text;
	private int rounds;
	public RoundSpawner roundSpawner;
	public void GameQuit()
	{
		Debug.Log("exit");
		Application.Quit();
	}
	public void Menu()
	{
		sceneFader.FadeTo(menuSceneName);
	}
	public void SyncRounds()
	{
		rounds = roundSpawner.GetCurrentRound();
		rounds_text.text = "Clear Rounds : " + string.Format("{00}", (rounds));
	}
}
