using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

[ExecuteInEditMode]
public class GameBoardCreator : MonoBehaviour
{
	[SerializeField] private List<SlotItem> availableSlotItems;
	[SerializeField, HideInInspector] private List<SlotItem> selectedItems = null;
	[SerializeField] private Vector2Int boardSize = new Vector2Int(6, 6);
	[SerializeField] private GameBoard boardPrefab;
	[SerializeField] private GameBoard gameBoardInstance;
	[SerializeField] private Slot slotPrefab;
	[SerializeField] private float cameraMargin = 1.0f;

	public Vector2Int BoardSize => boardSize;

	public List<SlotItem> CulledSelectedItems
	{
		get
		{
			List<SlotItem> culledList = new();
			foreach (var i in selectedItems)
			{
				if (i != null)
				{
					culledList.Add(i);
				}
			}
			return culledList;
		}
	}

	public void CreateBoard()
	{
		if (Application.isPlaying)
		{
			return;
		}

		if (gameBoardInstance != null)
		{
			DestroyImmediate(gameBoardInstance.gameObject);
			gameBoardInstance = null;
		}

		gameBoardInstance = PrefabUtility.InstantiatePrefab(boardPrefab) as GameBoard;
		gameBoardInstance.transform.parent = transform;
		gameBoardInstance.name = "Game Board Instance";
		gameBoardInstance.size = boardSize;
	}

	public void CreateSlots()
	{
		transform.position = Vector3.zero;

		if (Application.isPlaying || slotPrefab == null)
		{
			return;
		}

		DestroySlots();

		for (int x = 0; x < boardSize.x; x++)
		{
			for (int y = 0; y < boardSize.y; y++)
			{
				var newSlot = PrefabUtility.InstantiatePrefab(slotPrefab, gameBoardInstance.transform) as Slot;
				newSlot.transform.position = new Vector3(x, y);
				newSlot.name = "Slot_X" + x + "_Y" + y;
				newSlot.SetPosition(x, y);
			}
		}
	}

	public void SetOffset()
	{
		if (Application.isPlaying)
		{
			return;
		}

		float slotWidth = 0.5f;
		gameBoardInstance.transform.position -= transform.right * ((boardSize.x / 2.0f) - slotWidth);
		gameBoardInstance.transform.position -= transform.up * ((boardSize.y / 2.0f) - slotWidth);
	}

	public void FillWithRandomItems()
	{
		if (Application.isPlaying || CulledSelectedItems.Count == 0)
		{
			return;
		}

		var slots = gameBoardInstance.GetComponentsInChildren<Slot>();
		foreach (var slot in slots)
		{
			var randomItem = CulledSelectedItems[Random.Range(0, CulledSelectedItems.Count)];
			slot.InsertItem(Instantiate(randomItem), true);
		}
	}

	public void DestroySlots()
	{
		if (Application.isPlaying)
		{
			return;
		}

		var slots = gameBoardInstance.GetComponentsInChildren<Slot>();
		if (slots != null)
		{
			foreach (var s in slots)
			{
				DestroyImmediate(s.gameObject);
			}
		}
	}

	public void SetCameraSize()
	{
		if (Application.isPlaying)
		{
			return;
		}

		Camera.main.orthographicSize = (boardSize.y / 2.0f) + cameraMargin;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}

		if (gameBoardInstance == null)
		{
			gameBoardInstance = GetComponentInChildren<GameBoard>();
		}
	}
}
