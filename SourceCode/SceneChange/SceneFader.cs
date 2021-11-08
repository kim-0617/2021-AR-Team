using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
/*씬을 이동할 수 있게 하는 스크립트*/
public class SceneFader : MonoBehaviour {

	public Image img;
	public AnimationCurve curve;

	void Start ()
	{
		StartCoroutine(FadeIn()); // 코루틴 시작
	}

	public void FadeTo (string scene)
	{
		StartCoroutine(FadeOut(scene));
	}

	IEnumerator FadeIn ()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color (0f, 0f, 0f, a);
			yield return 0;
		}
	}

	IEnumerator FadeOut(string scene)
	{
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}

		SceneManager.LoadScene(scene);
	}

}
