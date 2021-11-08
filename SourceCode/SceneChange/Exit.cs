using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*게임 종료하는 스크립트*/
public class Exit : MonoBehaviour
{
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameQuit();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
