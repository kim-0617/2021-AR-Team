using UnityEngine;
using System.Collections;
/* 게임의 시작과 끝을 관리하는 스크립트 */
public class GameManager : MonoBehaviour
{

	public static bool GameIsOver; // GameOver 플래그
	public static bool isPause = false;
	public GameObject gameOverUI;
	public GameObject gameClearUI;
	public GameObject roundClearUI;

	void Start()
	{
		GameIsOver = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (GameIsOver)
			return;

		if (PlayerStats.Lives <= 0) // Life가 모두 소진되었을 때 GameOver UI 생성 
		{
			EndGame(); 
		}
	}

	void EndGame() 
	{
		GameIsOver = true;
		gameOverUI.GetComponent<GameOver>().SyncRounds();	
		gameOverUI.SetActive(true); // GameOver UI 생성
		gameOverUI.GetComponent<GameOver>().StopTime();

	}

	public void WinGame() // 모든라운드 클리어 시 게임클리어 UI 생성
	{
		GameIsOver = true;
		gameClearUI.GetComponent<GameClearUI>().SyncRounds();
		gameClearUI.SetActive(true);
	}
	public void WinRound()
	{
		roundClearUI.SetActive(true); // 1Round 끝날 때 마다 WinRound UI 생성
		Invoke("HideRoundClearUI", 3f); // 잠시후 소멸
	}
	private void HideRoundClearUI() { // RoundClearUI 소멸 함수
		roundClearUI.SetActive(false);
	}
}
