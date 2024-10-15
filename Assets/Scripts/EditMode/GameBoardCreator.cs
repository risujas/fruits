using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[ExecuteInEditMode]
public class GameBoardCreator : MonoBehaviour
{
	[SerializeField] private List<SlotItem> availableSlotItems;
	[SerializeField, HideInInspector] private List<SlotItem> selectedItems = null;
	[SerializeField] private GameBoard boardPrefab;
	[SerializeField] private Slot slotPrefab;
	[SerializeField] private Vector2Int boardSize = new Vector2Int(6, 6);
	[SerializeField] private float cameraMargin = 1.0f;

	private GameBoard gameBoardInstance;

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

#if UNITY_EDITOR

	public void CreateBoard()
	{
		if (gameBoardInstance != null)
		{
			DestroyImmediate(gameBoardInstance.gameObject);
			gameBoardInstance = null;
		}

		gameBoardInstance = PrefabUtility.InstantiatePrefab(boardPrefab) as GameBoard;
		gameBoardInstance.transform.parent = transform;
		gameBoardInstance.name = "Game Board Instance";
		gameBoardInstance.Size = boardSize;

		SetColliderDimensions();
		CreateSlots();
		SetOffset();
		SetCameraSize();
	}

	public void FillWithRandomItems()
	{
		if (CulledSelectedItems.Count == 0)
		{
			return;
		}

		var slots = gameBoardInstance.GetComponentsInChildren<Slot>();
		foreach (var slot in slots)
		{
			var randomItem = CulledSelectedItems[Random.Range(0, CulledSelectedItems.Count)];
			var newItem = PrefabUtility.InstantiatePrefab(randomItem) as SlotItem;
			newItem.GetComponent<SlotItemPlacement>().enabled = false;
			slot.InsertItem(newItem, true, false);
		}
	}

	private void CreateSlots()
	{
		transform.position = Vector3.zero;

		if (slotPrefab == null)
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

	private void SetOffset()
	{
		float slotWidth = 0.5f;
		gameBoardInstance.transform.position -= transform.right * ((boardSize.x / 2.0f) - slotWidth);
		gameBoardInstance.transform.position -= transform.up * ((boardSize.y / 2.0f) - slotWidth);
	}

	private void DestroySlots()
	{
		var slots = gameBoardInstance.GetComponentsInChildren<Slot>();
		if (slots != null)
		{
			foreach (var s in slots)
			{
				DestroyImmediate(s.gameObject);
			}
		}
	}

	private void SetCameraSize()
	{
		Camera.main.orthographicSize = (boardSize.y / 2.0f) + cameraMargin;
	}

	private void SetColliderDimensions()
	{
		var boxCollider = gameBoardInstance.GetComponent<BoxCollider2D>();
		boxCollider.size = new Vector2(boardSize.x, boardSize.y);
		boxCollider.offset = new Vector2((boardSize.x - 1) / 2.0f, (boardSize.y - 1) / 2.0f) + new Vector2(2.0f, 2.0f);
	}

	private void Update()
	{
		if (gameBoardInstance == null)
		{
			gameBoardInstance = GetComponentInChildren<GameBoard>();
		}
	}

#endif

}
