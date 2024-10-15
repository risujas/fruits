using System.Collections;
using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;
	[SerializeField] private string typeID;
	[SerializeField] private ParticleSystem destructionParticles;

	private Animator animator;
	private Coroutine movementCoroutine;

	public bool IsPartOfSet { get; set; } = false;

	public Color ItemColor => color;

	public string TypeID => typeID;

	public bool IsMoving 
	{
		get; private set;
	}

	public void TriggerDestructionEffect()
	{
		destructionParticles.Play();
	}

	public void PlaySelectionAnimation(bool play)
	{
		if (animator == null)
		{
			return;
		}

		animator.SetBool("PlaySelectionAnimation", play);
	}

	public void PlayHoverAnimation(bool play)
	{
		if (animator == null)
		{
			return;
		}

		animator.SetBool("PlayHoverAnimation", play);
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

	public void FadingDestroy(float duration)
	{
		StartCoroutine(FadingDestroyCoroutine(duration));
	}

	private IEnumerator FadingDestroyCoroutine(float duration)
	{
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		Color startColor = spriteRenderer.color;
		Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
			elapsedTime += Time.deltaTime;

			yield return null;
		}

		spriteRenderer.color = endColor;
		Destroy(gameObject);
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
