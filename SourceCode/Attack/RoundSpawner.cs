using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/*라운드 시작이나 종료에 관한 동작정의가 담긴 스크립트*/
public class RoundSpawner : MonoBehaviour
{
    public static int MonstersAlive = 0;
    public Round[] rounds;
    public Transform spawnPoint;
    private Vector3 spawnPosition;

    public Text MonsterAliveText;
    public Text RoundIndexText;
    public GameManager gameManager;
    private int RoundIndex = 0;

    public GameObject board;            //board 인식상태 확인용
    public Timer timer;                 //Round Start버튼 누르면 일정시간 후 round가 시작되도록 하는 timer

    private bool roundOver = true;      //현재 라운드 진행중이면 false, 대기상태면 true; (첫 몬스터 생성~몬스터 전원사망까지)

    PathVisualizerManager pathVisualizerManager;
	private void Start()
	{
        pathVisualizerManager = PathVisualizerManager.instance;

    }

    private void Update()
    {
        if (!roundOver)     //round가 진행중일때만 동작
        {
            MonsterAliveText.text = "Alive Monsters : " + MonstersAlive; // 남아있는 몬스터 수 표시
            if (MonstersAlive > 0)
            {
                return;
            }
            else if (MonstersAlive <= 0)
            {
                MonstersAlive = 0;
                if (RoundIndex == rounds.Length)    //최종라운드 종료시
                {
                    gameManager.WinGame();
                    roundOver = true;
                    this.enabled = false;
                    
                }
                else
                {
                    RoundIndexText.text = "Rounds : " + string.Format("{00}", (1 + RoundIndex));    //다음 라운드 표시
                    gameManager.WinRound();// RoundClearUI 3초간 표시
                    pathVisualizerManager.Show();
                    roundOver = true;
                }
                
            }
        }
        else
        {
            MonsterAliveText.text = "Press RoundStart";
        }
    }
    
    public void touched()
    {
        if (!board.activeSelf)              //보드 미인식시 round 시작 불가
        {
            Debug.Log("Board is not tracked");
            return;
        }
        if (!roundOver)                     // round가 진행중이면 다음round 시작 불가
        {
            Debug.Log("round is not over");
            return;
        }
        if (!timer.countFinish)             //타이머가 진행중이면 다음round 시작불가
        {
            Debug.Log("timer is not end");
            return;

        }
        timer.InitializeTime();             // timer 시작
        pathVisualizerManager.Hide();
    }
    public void StartRound()                //timer 종료 후 실행하는 메소드
    {
        if (timer.countFinish)              // timer count가 0까지 완료되고 실행되는지 확인
        {
            StartCoroutine(SpawnWave());
        }
    }
    public IEnumerator SpawnWave()
    {
        Round round = rounds[RoundIndex];
        MonstersAlive = round.count;

        for(int i=0; i<round.count; i++)
        {
            SpawnMonster(round.monster);
            roundOver = false;                  // round 진행중으로 변경
            yield return new WaitForSeconds(1f / round.rate);

        }
        RoundIndex++;
    }

    private void SpawnMonster(GameObject monster)
    {
        Instantiate(monster, GetSpawnPosition(), spawnPoint.rotation);

    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnPoint != null)
        {
            spawnPosition.x = spawnPoint.transform.position.x;
            //spawnPosition.y = spawnPoint.transform.position.y;
            spawnPosition.y = spawnPoint.transform.position.y - 0.025f;
            spawnPosition.z = spawnPoint.transform.position.z;

        }


        return spawnPosition;
    }
    public int GetCurrentRound()
    {
        return RoundIndex;
    }
}
