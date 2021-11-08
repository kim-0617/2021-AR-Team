using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*AR씬에서 중앙을 나타내는 스크립트*/
public class WorldCenter : MonoBehaviour
{
    public static WorldCenter instance;
	public GameObject centerObject;

	// Start is called before the first frame update
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("worldcenter can exist only one in a scene!");
			return;
		}
		instance = this;
	}
}
