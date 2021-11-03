using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeByAcc : MonoBehaviour
{
    //public GameObject cube;
    public float speed = 5f;
    public Vector2 moveLimit = Vector2.zero;
    Vector3 dir = Vector3.zero;

	private void Start()
	{
        dir.y = 0.5f;
	}

	void Update()
    {
        
        dir.x = Mathf.Round(Input.acceleration.x * 1000) / 1000;    //소수점 3자리까지의 값만 저장
        dir.z = Mathf.Round(Input.acceleration.y * 1000) / 1000;
        /*
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        transform.Translate(dir * speed);
        */
        dir.x *= speed;
        dir.z *= speed;
        transform.position = dir;

        KeepInScreen();
    }
    private void KeepInScreen() {
        Vector3 pos = transform.position;
        if (transform.position.x > moveLimit.x)
            pos.x = moveLimit.x;
        if (transform.position.x < -moveLimit.x)
            pos.x = -moveLimit.x;
        if (transform.position.z > moveLimit.y)
            pos.z = moveLimit.y;
        if (transform.position.z < -moveLimit.y)
            pos.z = -moveLimit.y;
        transform.position = pos;
    }
}
