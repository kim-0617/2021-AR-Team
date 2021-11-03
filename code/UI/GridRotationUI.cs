using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*게임시작 시 보드 크기에 맞게 그리드를 보여주는 스크립트*/
public class GridRotationUI : MonoBehaviour
{
    public Text gridRotationText;
	public AGrid grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	private void Update()
	{
		ChangeText();
	}

	public void ChangeText()
	{
		if (grid != null)
		{
			gridRotationText.text = "Grid Rotation (" + grid.imghere.rotation.eulerAngles.x + ", "
				+ grid.imghere.rotation.eulerAngles.y + ", " + grid.imghere.rotation.eulerAngles.z + ")";
		}
	}
}
