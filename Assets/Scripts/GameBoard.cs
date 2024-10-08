using UnityEngine;

public class GameBoard : MonoBehaviour
{
	private Slot firstSelectedSlot;
	private Slot secondSelectedSlot;
	private Slot[,] slots = null;

	public Vector2Int size;

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

		if (firstSelectedSlot.GetItem().TypeID == secondSelectedSlot.GetItem().TypeID)
		{
			return false;
		}

		return true;
	}

	private void SwapItems()
	{
		SlotItem item1 = firstSelectedSlot.GetItem();
		SlotItem item2 = secondSelectedSlot.GetItem();

		firstSelectedSlot.InsertItem(item2, false);
		secondSelectedSlot.InsertItem(item1, false);
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
				var item = horizontal ? slots[j, i].GetItem() : slots[i, j].GetItem();

				if (item == null)
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
									slots[j - 2, i].GetItem().IsPartOfSet = true;
									slots[j - 1, i].GetItem().IsPartOfSet = true;
									slots[j, i].GetItem().IsPartOfSet = true;
								}
								else
								{
									slots[i, j - 2].GetItem().IsPartOfSet = true;
									slots[i, j - 1].GetItem().IsPartOfSet = true;
									slots[i, j].GetItem().IsPartOfSet = true;
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


	private void HandleInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (firstSelectedSlot == null)
			{
				firstSelectedSlot = Slot.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				firstSelectedSlot.ApplyBorderHighlight();
			}
			else
			{
				secondSelectedSlot = Slot.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				firstSelectedSlot.RemoveBorderHighlight();

				if (IsValidMove())
				{
					SwapItems();

					bool foundSets = CheckForSets(true, false);
					if (!foundSets)
					{
						SwapItems();
					}
				}

				firstSelectedSlot = null;
				secondSelectedSlot = null;
			}
		}
	}

	private void DestroyItems()
	{
		foreach (var slot in slots)
		{
			if (slot.GetItem() != null && slot.GetItem().IsPartOfSet)
			{
				slot.Empty();
			}
		}
	}

	private void MakeItemsFall()
	{
		// TODO
	}

	private void SpawnAdditionalItems()
	{
		// TODO
	}

	private bool BoardHasEmptySlots()
	{
		return false;
	}

	private void Start()
	{
		slots = new Slot[size.x, size.y];

		var slotObjects = GetComponentsInChildren<Slot>();
		foreach (var so in slotObjects)
		{
			slots[so.GridPosition.x, so.GridPosition.y] = so;
		}
	}

	private void Update()
	{
		bool hasCompletedSets = CheckForSets(false, true);
		if (hasCompletedSets)
		{
			DestroyItems();
		}

		bool hasEmptySlots = BoardHasEmptySlots();
		if (hasEmptySlots)
		{
			MakeItemsFall();
			SpawnAdditionalItems();
		}
		
		if (!hasCompletedSets && !hasEmptySlots)
		{
			HandleInput();
		}
	}
}
