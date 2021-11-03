using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*이미지 인식을 통한 트래킹 박스를 관리하는 스크립트*/
public class BoxListManager : MonoBehaviour
{
    public static BoxListManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("BuildManager can exist only one in a scene!");
            return;
        }
        instance = this;
    }
    public AGrid grid;
	private List<GameObject> boxList;
    public int countTracked = 0;
    public int countSaved = 0;

    // private List<GameObject> towerList;
    // public int countBuiltTower;

    public bool isTracking = true;
    private void Update()
    {
        if (!isTracking)
        {


        }
    }
    private void Start()
	{
        boxList = new List<GameObject>();
       // towerList = new List<GameObject>();
	}

	public void AddBoxInList(GameObject box)
    {
        if (grid != null)
        {
            GameObject newbox = (GameObject)Instantiate(box, 
                box.transform.position,   //그 위치 그대로 저장
                //grid.GetNodeFromWorldPoint(box.transform.position).objPos,  //Grid에 맞게 저장
                box.transform.rotation);
            boxList.Add(newbox);
            Box tempbox = newbox.GetComponent<Box>();
            tempbox.BuildCastle(grid.imghere.rotation);
            countSaved++;
        }
    }

    public void ResetAll() // 박스 리셋
    {
        foreach (GameObject box in boxList)
        {
            Box tempbox = box.GetComponent<Box>();
            tempbox.DestroyCastle();
            Destroy(box);
        }
        boxList.Clear();
        countSaved = 0;
    }

    public void RemoveTowerInList(Box box, GameObject tower)
    {


    }

    public void CountTrackingBox(bool plus)
    {
        if (plus)
        {
            countTracked++;
        }
        else
        {
            countTracked--;
            if (countTracked < 0)
                countTracked = 0;
        }

    }

    public int CountSavedBox()
    {
        countSaved = boxList.Count;
        return countSaved;
    }

    public void ChangeMaterial(Material mat)
    {
        foreach (GameObject box in boxList)
        {
            box.GetComponent<MeshRenderer>().material = mat;
        }
    }
    public void ChangeScale(Vector3 scale)
    {
        foreach (GameObject box in boxList)
        {
            box.transform.localScale = scale;
        }
    }
}
