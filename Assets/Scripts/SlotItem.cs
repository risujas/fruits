using System.Collections;
using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color backgroundColor = Color.white;
	[SerializeField] private string typeID;
	[SerializeField] private bool canFall;
	[SerializeField] private bool canBeInSet;

	private Animator animator;

	public bool IsPartOfSet { get; set; } = false;
	public Color BackgroundColor => backgroundColor;
	public string TypeID => typeID;
	public bool CanFall => canFall;
	public bool CanBeInSet => canBeInSet;

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

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
}
