using UnityEngine;
using UnityEngine.UI;
/*몬스터가 나오기 전까지 카운트 다운 타이머*/
public class Timer : MonoBehaviour
{
    private static float countdown = 2f;
    public float startcount;
    public Text CountdownText;
    private bool mode = false;          //버튼 클릭상황 // 타이머 완료전까지 true;
    public bool countFinish;     //Count 시작~ Count 종료까지 = false; 그외 true;
    public RoundSpawner roundSpawner;

	private void Start()
	{
        countFinish = true;
        startcount = 2f;
    }
	public void InitializeTime()
    {
        countdown = startcount;
        CountdownText.gameObject.SetActive(true);
        CountdownText.text = string.Format("{0:0}", countdown + 1);
        countFinish = false;
        mode = true;
    }

    void Update()
    {
        if (mode)
        {
            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            //CountdownText.text = string.Format("{0:00.00}", countdown);
            CountdownText.text = string.Format("{0:0}", countdown + 1);
            if (countdown <= 0f)
            {
                CountdownText.text = "";
                mode = false;
				countFinish = true;
                CountdownText.gameObject.SetActive(false);
                StartSpawner();
            }
        }

    }
	private void StartSpawner()     //타이머 종료 후 Spawn 시작
	{
        roundSpawner.StartRound();
	}
}