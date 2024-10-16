using UnityEngine;

public class DestructionEffect : MonoBehaviour
{
	[SerializeField] private Color firstDestructionColor = Color.grey;
	[SerializeField] private Color secondDestructionColor = Color.grey;

	[SerializeField] private ParticleSystem destructionParticles;

	private const float particleColorMultiplier = 1.3f;

	public void Trigger()
	{
		destructionParticles.Play();
	}

	private void SetDestructionEffectColour()
	{
		var colorOverLifetime = destructionParticles.colorOverLifetime;
		colorOverLifetime.enabled = true;

		Gradient gradient1 = new Gradient();
		gradient1.SetKeys(
			new GradientColorKey[] {
			new GradientColorKey(firstDestructionColor * particleColorMultiplier, 0.0f)
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
			new GradientColorKey(secondDestructionColor * particleColorMultiplier, 0.0f)
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

	private void Start()
	{
		SetDestructionEffectColour();
	}
}
