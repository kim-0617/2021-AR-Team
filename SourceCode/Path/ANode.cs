using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*노드에 대한 정보를 정의하는 스크립트*/
public class ANode
{
    public int gridX;   //노드 배열에서의 X 좌표
    public int gridY;   //노드 배열에서의 Y 좌표

    public bool isWalkAble; //현재 노드가 벽인지 표시
    public Vector3 worldPos;    //현재 노드의 위치정보 (월드기준) 

    public ANode parentNode; // 이전 노드 정보

    public int gCost;   // The cost of moving to the next square
    public int hCost;   // 현재 노드부터 목표지점까지의 거리(칸수)
    public int fCost { get { return gCost + hCost; } } // fCost = gCost + hCost
    private GameObject thisNodeObject;

    ANodeObjectController objController;
    public Vector3 objPos;    //(이동용, box설치용) 
    Vector3 tempPos = new Vector3(0,0.025f,0);
    public ANode(bool nWalkable, Vector3 nWorldPos, int nGridX, int nGridY)    //생성자
    {
        isWalkAble = nWalkable;
        worldPos = nWorldPos;
        objPos = worldPos + tempPos;
        gridX = nGridX;
        gridY = nGridY;

        // 테스트용 추가
        CreateNodeObj();

    }
    public void DestoryNodeObj()
    {
        if (thisNodeObject != null)
        {
            objController.DestoryNodeObj();
        }
    }
    public void CreateNodeObj()
    {
        if (thisNodeObject != null && objController != null)
        {
            objController.DestoryNodeObj(thisNodeObject);
            objController.DestoryNodeObj();
        }
        objController = new ANodeObjectController();
        thisNodeObject = objController.CreateNodeObj(worldPos);
        objPos = worldPos + tempPos;
    }
    public bool CheckObj()
    {
        return objController.CheckObj();
    }
    public void ChangeColor()                       //AGrid.ChangeGridPositionByRotation()에서 사용
    {
        objController.ChangeColor(isWalkAble);
    }
    public void MoveNodeObj(float euler_angle)
    {
        objController.MoveNodeObj(euler_angle, worldPos);
        objPos = worldPos + tempPos;
    }
    public void HideObj()
    {
        objController.HideObj();
    }
    public void ShowObj()
    {
        objController.ShowObj();
    }
}
