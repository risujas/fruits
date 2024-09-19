using System.Collections.Generic;

using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField] private int boardSizeX = 8;
	[SerializeField] private int boardSizeY = 8;
	[SerializeField] private GameObject slotPrefab;

	private List<GameObject> slots;

	public void CreateSlots()
	{
		DestroySlots();
		transform.position = Vector3.zero;

		slots = new List<GameObject>();
		for (int x = 0; x < boardSizeX; x++)
		{
			for (int y = 0; y < boardSizeY; y++)
			{
				var newSlot = Instantiate(slotPrefab, new Vector3(x, y), Quaternion.identity, transform);
				newSlot.name = "Slot_X" + x + "_Y" + y;
				slots.Add(newSlot);
			}
		}
	}

	public void DestroySlots()
	{
		if (slots != null)
		{
			foreach (var s in slots)
			{
				DestroyImmediate(s.gameObject);
			}
		}
	}

	public void SetOffset()
	{
		float slotWidth = 0.5f;
		transform.position -= transform.right * ((boardSizeX / 2.0f) - slotWidth);
		transform.position -= transform.up * ((boardSizeY / 2.0f) - slotWidth);
	}
}
