using System.Collections;
using UnityEngine;

public class TimescaleLerper : MonoBehaviour
{
	private Coroutine slowmoCoroutine;

	public void TriggerSlowmo()
	{
		if (slowmoCoroutine != null)
		{
			StopCoroutine(slowmoCoroutine);
		}
		slowmoCoroutine = StartCoroutine(LerpSlowmo(0.0f, 1.0f, 1.0f));
	}

	private IEnumerator LerpSlowmo(float startValue, float targetValue, float duration)
	{
		Time.timeScale = startValue;
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
