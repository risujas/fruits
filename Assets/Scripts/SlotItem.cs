using System.Collections;
using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color primaryColor = Color.white;
	[SerializeField] private Color secondaryColor = Color.grey;

	[SerializeField] private string typeID;
	[SerializeField] private ParticleSystem destructionParticles;

	private const float particleColorMultiplier = 1.3f;

	private Animator animator;
	private Coroutine movementCoroutine;

	public bool IsPartOfSet { get; set; } = false;

	public Color PrimaryColor => primaryColor;
	public Color SecondaryColor => secondaryColor;

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

	private void SetDestructionEffectColour()
	{
		var colorOverLifetime = destructionParticles.colorOverLifetime;
		colorOverLifetime.enabled = true;

		Gradient gradient1 = new Gradient();
		gradient1.SetKeys(
			new GradientColorKey[] {
			new GradientColorKey(primaryColor * particleColorMultiplier, 0.0f)
			},
			new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 0.5f),
			new GradientAlphaKey(0.0f, 1.0f)
			}
		);

		Gradient gradient2 = new Gradient();
		gradient2.SetKeys(
			new GradientColorKey[] {
			new GradientColorKey(secondaryColor * particleColorMultiplier, 0.0f)
			},
			new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 0.5f),
			new GradientAlphaKey(0.0f, 1.0f)
			}
		);

		ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient(gradient1, gradient2);
		minMaxGradient.mode = ParticleSystemGradientMode.TwoGradients;
		colorOverLifetime.color = minMaxGradient;
	}


	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		SetDestructionEffectColour();
	}
}
