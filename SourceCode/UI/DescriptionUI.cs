using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*보드이미지 인식 후 게임조작 방법에 대한 UI*/
public class DescriptionUI : MonoBehaviour
{
    public GameObject descriptionUI;

    public void Hide()
    {
        descriptionUI.SetActive(false);
    }
}
