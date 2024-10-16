using System.Collections;
using UnityEngine;

public class LerpMovable : MonoBehaviour
{
	private Coroutine movementCoroutine;
	
	public bool IsMoving
	{
		get; private set;
	}

	public void MoveToPosition(Vector3 targetPosition, float duration)
	{
		if (movementCoroutine != null)
		{
			StopCoroutine(movementCoroutine);
			movementCoroutine = null;
		}
		movementCoroutine = StartCoroutine(MovementCoroutine(targetPosition, duration));
	}

	private IEnumerator MovementCoroutine(Vector3 targetPosition, float duration)
	{
		Vector3 startPosition = transform.position;
		float elapsedTime = 0f;
		IsMoving = true;

		while (elapsedTime < duration)
		{
			transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
			elapsedTime += Time.deltaTime;

			yield return null;
		}

		transform.position = targetPosition;
		IsMoving = false;
	}
}
