using UnityEngine;
using System.Collections;
/*기본적으로 지급되는 돈과 생명을 설정할 수 있다.*/
public class PlayerStats : MonoBehaviour
{

	public static int Money;
	public int startMoney = 400;

	public static int Lives;
	public int startLives = 20;

	public static int Rounds;

	void Start()
	{
		Money = startMoney;
		Lives = startLives;

		Rounds = 0;
	}

}
