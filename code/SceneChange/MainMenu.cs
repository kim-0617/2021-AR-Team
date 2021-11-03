using UnityEngine;
using UnityEngine.SceneManagement;
/*메인메뉴 스크립트*/
public class MainMenu : MonoBehaviour {

	public string levelToLoad = "GameScene";

	public SceneFader sceneFader;

	public void Play () // GameScene으로 이동하는 메소드
	{
		sceneFader.FadeTo(levelToLoad);
	}

	public void Quit () // 게임 종료 메소드
	{
		Debug.Log("Exciting...");
		Application.Quit();
	}

}
