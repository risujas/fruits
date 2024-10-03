using UnityEngine;

public class GameBoard : MonoBehaviour
{
	private Slot firstSelectedSlot;
	private Slot secondSelectedSlot;
	private GameBoardCreator gameBoardCreator;

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

		if ((firstSelectedSlot.GridPosition.x == 0 && secondSelectedSlot.GridPosition.x < 0) || (firstSelectedSlot.GridPosition.x == gameBoardCreator.BoardSize.x - 1 && secondSelectedSlot.GridPosition.x > firstSelectedSlot.GridPosition.x))
		{
			return false;
		}

		if ((firstSelectedSlot.GridPosition.y == 0 && secondSelectedSlot.GridPosition.y < 0) || (firstSelectedSlot.GridPosition.y == gameBoardCreator.BoardSize.y - 1 && secondSelectedSlot.GridPosition.y > firstSelectedSlot.GridPosition.y))
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

	private void Awake()
	{
		gameBoardCreator = GameObject.FindGameObjectWithTag("Game Board Creator").GetComponent<GameBoardCreator>();
	}

	private void Update()
	{
		HandleInput();
	}
}
