using System.Collections;
using UnityEditor.Animations;
using UnityEngine;


public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;
	[SerializeField] private string typeID;
	[SerializeField] private int selectionAnimationIndex;

	private Animator animator;
	private Coroutine movementCoroutine;

	public bool IsPartOfSet { get; set; } = false;

	public Color ItemColor => color;

	public string TypeID => typeID;

	public bool IsMoving 
	{
		get; private set;
	}

	public void PlaySelectionAnimation(bool play)
	{
		if (animator == null)
		{
			return;
		}
		if (play)
		{
			animator.SetBool("PlaySelectionAnimation", true);
			animator.SetInteger("SelectionAnimationIndex", selectionAnimationIndex);
		}
		else
		{
			animator.SetBool("PlaySelectionAnimation", false);
		}
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

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
}
