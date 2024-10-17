using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
	private AudioSource audioSource;
	[SerializeField] private AudioClip selectionSound;
	[SerializeField] private AudioClip deselectionSound;
	[SerializeField] private AudioClip setCompletionSoundRegular;
	[SerializeField] private AudioClip setCompletionSoundSuper;
	[SerializeField] private List<AudioClip> spawnSounds;

	public void PlaySelectionSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(selectionSound, 0.25f);
	}

	public void PlayDeselectionSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(deselectionSound, 0.25f);
	}

	public void PlayRegularSetSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(setCompletionSoundRegular, 0.25f);
	}

	public void PlayExtraSetSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(setCompletionSoundSuper, 0.25f);
	}

	public void PlaySpawnSound()
	{
		if (audioSource == null || spawnSounds == null || spawnSounds.Count == 0)
		{
			return;
		}

		var randomSound = spawnSounds[Random.Range(0, spawnSounds.Count)];
		audioSource.PlayOneShot(randomSound, 0.25f);
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
}
