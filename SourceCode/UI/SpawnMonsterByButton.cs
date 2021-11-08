using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*준비를 마치고 게임을 시작할 때 누르면 몬스터가 스폰된다.*/
public class SpawnMonsterByButton : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject monster;
	private GameObject spawned_monster;
	public Transform spawnPositionObject;

	public void SpawnMonster() // 버튼이 눌리면 몬스터를 스폰하는 메소드
	{
		if (monster != null)
		{
			if (spawned_monster != null)
			{
				Destroy(spawned_monster);
			}
			spawned_monster = (GameObject)Instantiate(monster, spawnPositionObject.position - new Vector3(0,0.025f,0), Quaternion.Euler(new Vector3(0, 90, 0)));

		}
	}
}
