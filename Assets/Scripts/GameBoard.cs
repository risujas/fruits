using UnityEngine;

public class GameBoard : MonoBehaviour
{
	private Slot firstSelectedSlot;
	private Slot secondSelectedSlot;

	private void SelectFirstSlot()
	{
		firstSelectedSlot = SlotItemPlacement.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void SelectSecondSlot()
	{
		secondSelectedSlot = SlotItemPlacement.FindNearestSlot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void ApplyHighlight()
	{
		firstSelectedSlot.GetComponentInChildren<SpriteRenderer>().color = Color.red;
	}

	private void RemoveHighlight()
	{
		firstSelectedSlot.GetComponentInChildren<SpriteRenderer>().color = Color.white;
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

		GameBoardCreation gameBoardCreation = GetComponent<GameBoardCreation>();

		if ((firstSelectedSlot.x == 0 && secondSelectedSlot.x < 0) || (firstSelectedSlot.x == gameBoardCreation.BoardSize.x - 1 && secondSelectedSlot.x > firstSelectedSlot.x))
		{
			return false;
		}

		if ((firstSelectedSlot.y == 0 && secondSelectedSlot.y < 0) || (firstSelectedSlot.y == gameBoardCreation.BoardSize.y - 1 && secondSelectedSlot.y > firstSelectedSlot.y))
		{
			return false;
		}

		int rowDifference = Mathf.Abs(firstSelectedSlot.x - secondSelectedSlot.x);
		int columnDifference = Mathf.Abs(firstSelectedSlot.y - secondSelectedSlot.y);

		if (!((rowDifference == 1 && columnDifference == 0) || (rowDifference == 0 && columnDifference == 1)))
		{
			return false;
		}

		return true;
	}

	private void SwapItems()
	{
		SlotItem item1 = firstSelectedSlot.InsertedItem;
		SlotItem item2 = secondSelectedSlot.InsertedItem;

		item1.transform.parent = secondSelectedSlot.transform;
		item1.transform.position = secondSelectedSlot.transform.position;

		item2.transform.parent = firstSelectedSlot.transform;
		item2.transform.position = firstSelectedSlot.transform.position;
	}

	private bool CompleteSets()
	{
		// Find any completed sets, return false if none exist

		return true;
	}

	private void HandleInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (firstSelectedSlot == null)
			{
				SelectFirstSlot();
				ApplyHighlight();
			}
			else
			{
				SelectSecondSlot();
				RemoveHighlight();

				if (IsValidMove())
				{
					SwapItems();

					bool foundSets = CompleteSets();
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

	private void Update()
	{
		HandleInput();
	}
}
