using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*Life 모두 소진 시 나오게 되는 GameOver UI*/
public class GameOver : MonoBehaviour
{
	public GameObject ui;
	public string menuSceneName = "MainMenu";

	public SceneFader sceneFader;

	public Text rounds_text;
	private int rounds;
	public RoundSpawner roundSpawner;

	public void StopTime() // GameOver UI 등장 시 모든 동작을 멈춤
	{
		if (ui.activeSelf)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Retry() // 재시작
	{
		Time.timeScale = 1f;
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
	}

	public void Menu() // 메인메뉴로 이동
	{
		Time.timeScale = 1f;
		sceneFader.FadeTo(menuSceneName);
	}
	public void SyncRounds() // 최근라운드로 이동
	{
		rounds = roundSpawner.GetCurrentRound();
		rounds_text.text = string.Format("{00}", (rounds-1));
	}
}
