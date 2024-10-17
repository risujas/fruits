using System.Collections;
using UnityEngine;

public class TimescaleLerper : MonoBehaviour
{
	public void TriggerSlowmo()
	{
		StartCoroutine(LerpSlowmo(0.0f, 1.0f, 1.0f));
		Debug.Log("Slowmo triggered!");
	}

	private IEnumerator LerpSlowmo(float startValue, float targetValue, float duration)
	{
		float t = 0.0f;
		while (t < duration)
		{
			Time.timeScale = Mathf.Lerp(startValue, targetValue, t / duration);
			t += Time.unscaledDeltaTime;

			yield return null;
		}

		Time.timeScale = targetValue;
	}
}
