using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*보드에 맞게 격자의 그리드를 생성하고 보여주는 스크립트*/
public class AGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;      //벽을 찾을 때 비교할 수 있는 공용 레이어 마스크 (사전에 Wall을 만들어두고 이후 Wall 로 설정
    public Vector2 gridWorldSize;   //전체 월드의 실제 너비와 높이
    public float nodeRadius;        // 노드의 반경 (반지름)
    public GameObject board;    // grid의 기준이 될 보드

    ANode[,] grid;                   // 노드 2차원배열 
    public List<ANode> path;    // 최종 경로 (노드리스트)

    float nodeDiameter;         // 노드의 직경을 저장 (반경*2) (지름)
    int gridSizeX;              // 배열에서의 노드들의 위치를 저장할 그리드의 X 크기 (세로 열 = 8)
    int gridSizeY;              // y좌표 (가로 행 = 5)

    //0717 추가부분
    public LayerMask nodeMask;
    public Transform imghere;       //이미지 회전값을 받아올 오브젝트의 Transform
    private Quaternion imgRotation;    //imghere transform의 rotation값 (쿼터니온)
    public SpawnPosition spawnPosition;
    private PathVisualizerManager pathVisualizerManager;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

	private void Start()
    {
        pathVisualizerManager = PathVisualizerManager.instance;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();   
    }

    public void CreateGrid()
    {                                   
        if (grid != null)
        {
            foreach (ANode n in grid)
            {
                n.DestoryNodeObj();
            }
            grid = null;
        }
        grid = new ANode[gridSizeX, gridSizeY];  //노드배열 선언 (전체 grid를 나타내는 배열)
        Vector3 worldBottomLeft = board.transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
        Vector3 worldPoint;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius/2, unwalkableMask));   //감지반경을 중앙으로 설정
                bool walkable = true;   // 임시값 저장
                grid[x, y] = new ANode(walkable, worldPoint, x, y);
            }
        }
        //0717추가부분
        ChangeGridPositionByRotation();             //보드 회전값에 따라 grid 전체 회전이동
        spawnPosition.SetSpawnPosition();           //스폰지점 노드에 맞게 이동
        pathVisualizerManager.Show();
    }
    public void ShowGrid()
    {
        foreach (ANode n in grid)
        {
            n.ShowObj();
        }
        return;

    }
    public void HideGrid()
    {
        foreach (ANode n in grid)
        {
            n.HideObj();
        }
        return;
    }

    //0717추가함수 ChangeGridPositionByRotation
    public void ChangeGridPositionByRotation()     //보드의 회전값에 따라 각 노드 위치를 수정하여 저장
    {
        float x, y = 0;

        Vector3 newPos;         //변경될 좌표 (보드기준 상대좌표)


        float angle = GetEulerRotation();
        if (grid != null)
        {
            foreach (ANode n in grid)
            {
                x = imghere.transform.position.x - n.worldPos.x;
                y = imghere.transform.position.z - n.worldPos.z;

                newPos.x = (x * Mathf.Cos(angle)) - (y * Mathf.Sin(angle));  // 각도만큼 회전이동 (xz평면 y축기준 반시계방향 회전각도)
                newPos.z = (y * Mathf.Cos(angle)) + (x * Mathf.Sin(angle));  // 오일러각도사용, 라디안 단위로 입력
                newPos.y = 0;
                n.worldPos = imghere.transform.position - newPos;  //변환된 위치로 이동
                n.MoveNodeObj(angle);
                bool walkable = !(Physics.CheckSphere(n.worldPos, nodeRadius / 2, unwalkableMask));   //변경된 위치 기준 감지
                n.isWalkAble = walkable;
                n.ChangeColor();
            }
        }
        return;
    }
	void Update()
	{
        CheckWalkable();
    }
    public void CheckWalkable()
    {
        if (grid != null)
        {
            foreach (ANode n in grid)
            {
                bool walkable = !(Physics.CheckSphere(n.worldPos, nodeRadius / 2, unwalkableMask));   //변경된 위치 기준 감지
                if (n.isWalkAble == walkable)
                    continue;
                else
                {
                    n.isWalkAble = walkable;
                    n.ChangeColor();
                }
            }
        }
    }
	public float GetEulerRotation()        //수학적 계산을 위한 양의 각도 반환   
    {
        imgRotation = imghere.rotation;
        float angle_m = -imgRotation.eulerAngles.y;   //수학계산시 양의각도 회전방향 == 반시계방향 , 유니티 양의각도 회전방향 == 시계방향
        
        float rotationNum = angle_m / 360;            //회전수 
        if (angle_m == 0 || angle_m == 360)
        {
            angle_m = 0;
        }
        else if (angle_m > 0)
        {
            if (rotationNum < 1)
            {
                ;                                       //양의 각도 0~360사이 반환
            }
            else
            {
                angle_m = angle_m - (int)rotationNum * 360;     //양의 각도 0~360사이 반환
            }
        }
        else if (angle_m < 0)
        {
            if (rotationNum > -1)
            {
                angle_m = 360 + angle_m;                        //양의 각도 0~360사이 반환
            }
            else
            {
                angle_m = (-(int)rotationNum + 1) * 360 + angle_m;  //양의 각도 0~360사이 반환
            }
        }

        angle_m = 2 * Mathf.PI * angle_m / 360;         //호도법
        return angle_m;
    }

    public Quaternion GetQuaternionRotation()
    {
        return imgRotation;
    }

    public ANode GetNodeFromWorldPoint(Vector3 worldPostition)
    {
        string tagName = "NodeTag";
        List<GameObject> foundNodes;
        foundNodes = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName));              //NodeTag 라는 태그 이름을 가진 오브젝트만 검색(노드들만)
        float shortestDis = Vector3.Distance(worldPostition, foundNodes[0].transform.position);     //기본값
        GameObject node = foundNodes[0];            //기본값
        Vector3 temp;
        float distance;
        foreach (GameObject found in foundNodes)                                    //거리가 가장 짧은 노드 오브젝트 추출
        {
            temp.x = Mathf.Abs(worldPostition.x - found.transform.position.x);
            temp.z = Mathf.Abs(worldPostition.z - found.transform.position.z);
            temp.y = 0;

            distance = Mathf.Sqrt(temp.x * temp.x + temp.z * temp.z);  
            if (distance < shortestDis)
            {
                node = found;
                shortestDis = distance;
            }
        }
        foreach (ANode n in grid) // 거리가 가장 짧은 노드의 ANode 검색
        {
            if (node.transform.position == n.worldPos)
            {
                return n;
            }
        }
        return grid[0, 0];  //반환없으면 기본값 반환.. (몬스터 스폰지점)

    }

    public ANode GetNodeFromArrayNum(int column, int row)
    {
        if (grid != null)
        {
            if(column <= gridSizeX && row <= gridSizeY)
                return grid[column, row];
            else
                return grid[0, 0];
        }

        return null;
    }
        public List<ANode> GetNeighbours(ANode CurNode)
    {
        List<ANode> NeighboringNodes = new List<ANode>();
        int xCheck;
        int yCheck;
        //우측 확인
        xCheck = CurNode.gridX + 1;
        yCheck = CurNode.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //좌측 확인
        xCheck = CurNode.gridX - 1;
        yCheck = CurNode.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //상단 확인
        xCheck = CurNode.gridX;
        yCheck = CurNode.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //하단 확인
        xCheck = CurNode.gridX;
        yCheck = CurNode.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        return NeighboringNodes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(board.transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            foreach (ANode n in grid)
            {
                if (!n.isWalkAble)
                {
                    Gizmos.color = Color.white;     //벽은 하얀색 표시
                }
                else
                {
                    Gizmos.color = Color.yellow;    //이동가능한 공간은 노란색표시
                }
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.red;       //현재 노드가 finalPath에 포함된 노드면 빨간표시
                    }
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

}
