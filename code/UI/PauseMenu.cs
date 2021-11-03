using UnityEngine;
using UnityEngine.SceneManagement;
/*일시정지 버튼을 클릭했을 때 나오는 퍼즈메뉴 UI 스크립트*/
public class PauseMenu : MonoBehaviour
{

	public GameObject ui;

	public string menuSceneName = "MainMenu";

	public SceneFader sceneFader;

	void Update() // 유니티상에서 esc키나 p키를 눌러도 동작
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			Toggle();
		}
	}

	public void Toggle()
	{
		ui.SetActive(!ui.activeSelf);

		if (ui.activeSelf) // UI활성화시 모두 멈춤
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Retry() // 재시작 함수
	{
		Toggle();
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
	}

	public void Menu() // 메인메뉴로 이동 함수
	{
		Toggle();
		sceneFader.FadeTo(menuSceneName);
	}
	public void GameQuit() // 게임종료 함수
	{
		Debug.Log("exit");
		Application.Quit();
	}

}
