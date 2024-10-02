using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

[ExecuteInEditMode]
public class GameBoardCreation : MonoBehaviour
{
	[SerializeField] private List<SlotItem> availableSlotItems;
	[SerializeField] private Vector2Int boardSize = new Vector2Int(6, 6);
	[SerializeField] private GameObject slotPrefab;
	[SerializeField] private float cameraMargin = 1.0f;

	public Vector2Int BoardSize => boardSize;

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
				var newSlot = PrefabUtility.InstantiatePrefab(slotPrefab, transform) as GameObject;
				newSlot.transform.position = new Vector3(x, y);
				newSlot.name = "Slot_X" + x + "_Y" + y;
				newSlot.GetComponent<Slot>().SetPosition(x, y);
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
		transform.position -= transform.right * ((boardSize.x / 2.0f) - slotWidth);
		transform.position -= transform.up * ((boardSize.y / 2.0f) - slotWidth);
	}

	public void SetCameraSize()
	{
		Camera.main.orthographicSize = (boardSize.y / 2.0f) + cameraMargin;
	}

	public void FillWithRandomItems()
	{
		var slots = GetComponentsInChildren<Slot>();
		foreach (var slot in slots)
		{
			var randomItem = availableSlotItems[Random.Range(0, availableSlotItems.Count)];
			slot.InsertItem(Instantiate(randomItem, slot.transform.position, Quaternion.identity, slot.transform));
		}
	}

	private void DestroySlots()
	{
		var slots = GetComponentsInChildren<Slot>();
		foreach (var s in slots)
		{
			DestroyImmediate(s.gameObject);
		}
	}
}
