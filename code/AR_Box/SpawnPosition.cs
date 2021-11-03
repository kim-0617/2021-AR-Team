using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*몬스터 스폰지점에 대한 정의가 담긴 스크립트*/
public class SpawnPosition : MonoBehaviour
{
    public GameObject spawnPositionObject;
    
    public int column = 0;  //0~7   //세로 열
    public int row = 0;     //0~5   //가로 행
    public AGrid grid;

    public void SetSpawnPosition()
    {
        if (spawnPositionObject != null)
        {
            Vector3 temp = new Vector3(0, 0.025f, 0);
            spawnPositionObject.transform.position = grid.GetNodeFromArrayNum(column, row).worldPos + temp;
            
        }
    }
}
