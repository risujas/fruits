using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	private GameBoardCreator gameBoardCreator;
	private Slot firstSelectedSlot;
	private Slot secondSelectedSlot;
	private Slot[,] slots = null;
	[SerializeField, HideInInspector] private Vector2Int size;

	private AudioSource audioSource;
	[SerializeField] private AudioClip selectionSound;
	[SerializeField] private AudioClip deselectionSound;
	[SerializeField] private AudioClip setCompletionSoundRegular;
	[SerializeField] private AudioClip setCompletionSoundSuper;
	[SerializeField] private List<AudioClip> spawnSounds;

	private bool userTookAction = false;

	private const float itemFadeOutTime = 0.2f;

	public Vector2Int Size
	{
		get 
		{
			return size;
		}
		set
		{
			size = value;
		}
	}

	private void TriggerSlowmo()
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

	private bool IsValidMove()
	{
		if (firstSelectedSlot == null || secondSelectedSlot == null)
		{
			return false;
		}

		if (firstSelectedSlot == secondSelectedSlot)
		{
			return false;
		}

		if ((firstSelectedSlot.GridPosition.x == 0 && secondSelectedSlot.GridPosition.x < 0) || (firstSelectedSlot.GridPosition.x == size.x - 1 && secondSelectedSlot.GridPosition.x > firstSelectedSlot.GridPosition.x))
		{
			return false;
		}

		if ((firstSelectedSlot.GridPosition.y == 0 && secondSelectedSlot.GridPosition.y < 0) || (firstSelectedSlot.GridPosition.y == size.y - 1 && secondSelectedSlot.GridPosition.y > firstSelectedSlot.GridPosition.y))
		{
			return false;
		}

		int rowDifference = Mathf.Abs(firstSelectedSlot.GridPosition.x - secondSelectedSlot.GridPosition.x);
		int columnDifference = Mathf.Abs(firstSelectedSlot.GridPosition.y - secondSelectedSlot.GridPosition.y);
		if (!((rowDifference == 1 && columnDifference == 0) || (rowDifference == 0 && columnDifference == 1)))
		{
			return false;
		}

		if (firstSelectedSlot.InsertedItem.TypeID == secondSelectedSlot.InsertedItem.TypeID)
		{
			return false;
		}

		return true;
	}

	private void SwapItems(Slot firstSlot, Slot secondSlot)
	{
		SlotItem item1 = firstSlot.InsertedItem;
		SlotItem item2 = secondSlot.InsertedItem;

		firstSlot.InsertItem(item2, false, true);
		secondSlot.InsertItem(item1, false, true);
	}

	private bool CheckForSets(bool breakAfterFirst, bool markItems)
	{
		bool foundSets = false;

		bool horizontalSetsFound = CheckForSetRow(true, breakAfterFirst, markItems);
		if (horizontalSetsFound)
		{
			foundSets = true;
		}

		bool verticalSetsFound = CheckForSetRow(false, breakAfterFirst, markItems);
		if (verticalSetsFound)
		{
			foundSets = true;
		}

		return foundSets;
	}

	private bool CheckForSetRow(bool horizontal, bool breakAfterFirst, bool markItems)
	{
		bool foundSets = false;

		for (int i = 0; i < (horizontal ? size.y : size.x); i++)
		{
			int sequentialIdenticalItems = 1;
			string previousType = string.Empty;

			for (int j = 0; j < (horizontal ? size.x : size.y); j++)
			{
				var item = horizontal ? slots[j, i].InsertedItem : slots[i, j].InsertedItem;

				if (item == null || !item.CanBeInSet)
				{
					sequentialIdenticalItems = 1;
					previousType = string.Empty;
					continue;
				}

				if (item.TypeID == previousType)
				{
					sequentialIdenticalItems++;

					if (sequentialIdenticalItems >= 3)
					{
						if (markItems)
						{
							if (sequentialIdenticalItems == 3)
							{
								if (horizontal)
								{
									slots[j - 2, i].InsertedItem.IsPartOfSet = true;
									slots[j - 1, i].InsertedItem.IsPartOfSet = true;
									slots[j, i].InsertedItem.IsPartOfSet = true;
								}
								else
								{
									slots[i, j - 2].InsertedItem.IsPartOfSet = true;
									slots[i, j - 1].InsertedItem.IsPartOfSet = true;
									slots[i, j].InsertedItem.IsPartOfSet = true;
								}
								foundSets = true;
							}
							else
							{
								item.IsPartOfSet = true;
							}
						}
						if (breakAfterFirst)
						{
							foundSets = true;
							break;
						}
					}
				}
				else
				{
					sequentialIdenticalItems = 1;
				}

				previousType = item.TypeID;
			}
		}

		return foundSets;
	}

	private void HandleFirstSelection()
	{
		firstSelectedSlot = Slot.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (firstSelectedSlot.ContainsSelectableItem())
		{
			firstSelectedSlot.ApplyBorderHighlight();
			firstSelectedSlot.InsertedItem.PlaySelectionAnimation(true);
			PlaySelectionSound();
		}
		else
		{
			firstSelectedSlot = null;
		}
	}

	private void HandleSecondSelection()
	{
		secondSelectedSlot = Slot.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (secondSelectedSlot.ContainsSelectableItem() && IsValidMove())
		{
			SwapItems(firstSelectedSlot, secondSelectedSlot);

			bool foundSets = CheckForSets(true, false);
			if (!foundSets)
			{
				SwapItems(firstSelectedSlot, secondSelectedSlot);
			}
		}
		firstSelectedSlot.RemoveBorderHighlight();
		firstSelectedSlot.InsertedItem.PlaySelectionAnimation(false);
		PlayDeselectionSound();

		firstSelectedSlot = null;
		secondSelectedSlot = null;
	}

	private bool IsHoveringOverBoard()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
		if (!hit || hit.collider.gameObject != gameObject)
		{
			return false;
		}
		return true;
	}

	private void HandleHoverAnimation()
	{
		var nearestSlot = Slot.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		nearestSlot.InsertedItem.PlayHoverAnimation(true);
	}

	private void HandleInput()
	{
		foreach (var s in slots)
		{
			s.InsertedItem.PlayHoverAnimation(false);
		}

		if (IsHoveringOverBoard())
		{
			HandleHoverAnimation();
		}
		else
		{
			return;
		}

		if (Input.GetMouseButtonUp(0))
		{
			userTookAction = true;
			if (firstSelectedSlot == null)
			{
				HandleFirstSelection();
			}
			else
			{
				HandleSecondSelection();
			}
		}
	}

	private void DestroyItems()
	{
		int numDestroyed = 0;

		foreach (var slot in slots)
		{
			if (slot.InsertedItem != null && slot.InsertedItem.IsPartOfSet && slot.InsertedItem.CanBeInSet)
			{
				slot.InsertedItem.FadingDestroy(itemFadeOutTime);
				slot.InsertedItem.GetComponentInChildren<DestructionEffect>()?.Trigger();
				slot.DetachInsertedItem();
				numDestroyed++;
			}
		}

		if (numDestroyed >= 3)
		{

			if (!userTookAction)
			{
				PlayExtraSetSound();
				TriggerSlowmo();
			}
			else
			{
				PlayRegularSetSound();
			}
		}
	}

	// fuck all this, need to do the falling in phases
	// first: drop all items straight odown
	// second: drop all items diagonally
	// third: drop all items sideways
	// NEED to cancel phases 2 or 3 if 1 becomes available

	private void HandleItemFalling()
	{
		for (int y = 0; y < size.y - 1; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				var currentSlot = slots[x, y];
				if (currentSlot.InsertedItem == null)
				{
					FillEmptySlot(currentSlot);
				}
			}
		}
	}

	private void FillEmptySlot(Slot slot)
	{
		var emptyPos = slot.GridPosition;
		SlotItem item = null;
		
		item = FindFillItem(emptyPos, new Vector2Int(0, 1));
		if (item != null)
		{
			slot.InsertItem(item, false, true);
			return;
		}

		item = FindFillItem(emptyPos, new Vector2Int(-1, 1));
		if (item != null)
		{
			slot.InsertItem(item, false, true);
			return;
		}

		item = FindFillItem(emptyPos, new Vector2Int(1, 1));
		if (item != null)
		{
			slot.InsertItem(item, false, true);
			return;
		}

		item = FindFillItem(emptyPos, new Vector2Int(-1, 0));
		if (item != null)
		{
			slot.InsertItem(item, false, true);
			return;
		}

		item = FindFillItem(emptyPos, new Vector2Int(1, 0));
		if (item != null)
		{
			slot.InsertItem(item, false, true);
			return;
		}
	}

	private SlotItem FindFillItem(Vector2Int emptyPos, Vector2Int offset)
	{
		if (emptyPos.x + offset.x < 0 || emptyPos.x + offset.x >= size.x || emptyPos.y + offset.y < 0 || emptyPos.y + offset.y >= size.y)
		{
			return null;
		}

		var item = slots[emptyPos.x + offset.x, emptyPos.y + offset.y].InsertedItem;

		if (item == null)
		{
			return null;
		}

		if (!item.CanFall)
		{
			return null;
		}

		return item;
	}

	private void SpawnAdditionalItems()
	{
		// TODO Algorithmically spawn items to ensure that valid combinations exist
		// TODO have a predetermined spawn seed for each level / board

		bool spawnedItems = false;

		for (int x = 0; x < size.x; x++)
		{
			var slot = slots[x, size.y - 1];
			if (slot.InsertedItem == null)
			{
				var randomItem = gameBoardCreator.CulledSelectedItems[Random.Range(0, gameBoardCreator.CulledSelectedItems.Count)];
				var newItem = Instantiate(randomItem, slot.transform.position + Vector3.up, Quaternion.identity);
				slot.InsertItem(newItem, false, true);
				spawnedItems = true;
			}
		}

		if (spawnedItems)
		{
			PlaySpawnSound();
		}
	}

	private bool BoardHasEmptySlots()
	{
		foreach (var s in slots)
		{
			if (s.InsertedItem == null)
			{
				return true;
			}
		}
		return false;
	}

	private bool BoardHasLerpMovingItems()
	{
		foreach (var s in slots)
		{
			var item = s.InsertedItem;

			if (item == null)
			{
				continue;
			}

			var lerpMovable = item.GetComponent<LerpMovable>();
			if (lerpMovable != null && lerpMovable.IsMoving)
			{
				return true;
			}
		}

		return false;
	}

	private void PlaySelectionSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(selectionSound, 0.25f);
	}

	private void PlayDeselectionSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(deselectionSound, 0.25f);
	}

	private void PlayRegularSetSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(setCompletionSoundRegular, 0.25f);
	}

	private void PlayExtraSetSound()
	{
		if (audioSource == null || selectionSound == null)
		{
			return;
		}

		audioSource.PlayOneShot(setCompletionSoundSuper, 0.25f);
	}

	private void PlaySpawnSound()
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

	private void Start()
	{
		gameBoardCreator = GetComponentInParent<GameBoardCreator>();

		slots = new Slot[size.x, size.y];

		var slotObjects = GetComponentsInChildren<Slot>();
		foreach (var so in slotObjects)
		{
			slots[so.GridPosition.x, so.GridPosition.y] = so;
		}
	}

	private void Update()
	{
		// If any items are being lerped to a new position, wait until they finish
		if (BoardHasLerpMovingItems())
		{
			return;
		}

		// If there are completed sets on the board, destroy the items
		bool hasCompletedSets = CheckForSets(false, true);
		if (hasCompletedSets)
		{
			Debug.Log("Board has completed sets waiting to be destroyed");
			DestroyItems();
		}

		// If there are empty slots on the board, make items fall into them
		bool hasEmptySlots = BoardHasEmptySlots();
		if (hasEmptySlots)
		{
			Debug.Log("Board has empty slots waiting to be filled");

			HandleItemFalling();
			SpawnAdditionalItems();
		}

		// If there aren't any completed sets or empty slots, the user may take an action
		userTookAction = false;
		if (!hasCompletedSets && !hasEmptySlots)
		{
			HandleInput();
		}
	}
}
