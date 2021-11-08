using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*노드에 대한 생성/소멸 정의 스크립트*/
public class ANodeObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject thisNodeObject;
    Renderer rend;
    SphereCollider col;
    LayerMask nodeMask; //노드 마스크
    Renderer rend_before;
    public ANodeObjectController()
    {
        nodeMask = LayerMask.NameToLayer("Node");
    }
    public void DestoryNodeObj()
    {
        if (thisNodeObject != null)
        {
            Destroy(thisNodeObject);
        }
    }
    public void DestoryNodeObj(GameObject node)
    {
        if (node != null)
        {
            Destroy(node);
        }
    }
    public GameObject CreateNodeObj(Vector3 worldPos)
    {
        if (thisNodeObject != null)
        {
            Destroy(thisNodeObject);
        }
        thisNodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        thisNodeObject.transform.position = worldPos;
        thisNodeObject.transform.localScale = new Vector3(0.025f, 0.01f, 0.025f);
        thisNodeObject.layer = nodeMask;
        thisNodeObject.tag = "NodeTag";
        rend = thisNodeObject.GetComponent<Renderer>();                 //색상변경용
        col = thisNodeObject.GetComponent<SphereCollider>();
        rend.material.color = Color.blue;
        col.isTrigger = true;

        return thisNodeObject;
    }
    public bool CheckObj()
    {
        if (thisNodeObject == null)
            return false;
        else
            return true;
    }
    public void ChangeColor(bool isWalkAble)                       //AGrid.ChangeGridPositionByRotation()에서 사용
    {
        if (isWalkAble)
            rend.material.color = Color.green;       //이동가능 노드 green
        else
            rend.material.color = Color.red;        //이동불가 노드 빨간표시
    }
    public void MoveNodeObj(float euler_angle, Vector3 worldPos)
    {
        if (thisNodeObject != null)
        {
            if (thisNodeObject.transform.position != worldPos)
            {
                thisNodeObject.transform.position = worldPos;
                thisNodeObject.transform.rotation = Quaternion.Euler(0f, euler_angle / (2 * Mathf.PI) * 360, 0f);
            }
        }

    }
    public void HideObj()
    {
        rend.enabled = false;
    }
    public void ShowObj()
    {
        rend.enabled = true;
    }
}
