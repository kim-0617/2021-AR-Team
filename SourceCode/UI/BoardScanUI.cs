using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*보드중앙의 이미지를 인식하여 게임을 시작할 수 있도록 하는 스크립트*/
public class BoardScanUI : MonoBehaviour
{
    public GameObject board;
    public GameObject descriptionUI;
    public GameObject gameUI;
    public GameObject boardScanUI;

    public void ScanSuccess() // 스캔이 성공적으로 되면, 관련 UI들을 활성화 시킨다.
    {
        if (board.activeSelf)
        {
            gameUI.SetActive(true);
            descriptionUI.SetActive(true);
            boardScanUI.SetActive(false);
        }
    }
}
