using UnityEngine;

[ExecuteInEditMode]
public class GameBoardCreation : MonoBehaviour
{
	[SerializeField] private uint boardSizeX = 8;
	[SerializeField] private uint boardSizeY = 8;
	[SerializeField] private GameObject slotPrefab;

	public void CreateSlots()
	{
		transform.position = Vector3.zero;

		if (Application.isPlaying || slotPrefab == null)
		{
			return;
		}

		DestroySlots();

		for (int x = 0; x < boardSizeX; x++)
		{
			for (int y = 0; y < boardSizeY; y++)
			{
				var newSlot = Instantiate(slotPrefab, new Vector3(x, y), Quaternion.identity, transform);
				newSlot.name = "Slot_X" + x + "_Y" + y;
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
		transform.position -= transform.right * ((boardSizeX / 2.0f) - slotWidth);
		transform.position -= transform.up * ((boardSizeY / 2.0f) - slotWidth);
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
