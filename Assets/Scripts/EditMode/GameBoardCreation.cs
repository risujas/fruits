using UnityEngine;

[ExecuteInEditMode]
public class GameBoardCreation : MonoBehaviour
{
	[SerializeField] private Vector2Int boardSize = new Vector2Int(6, 6);
	[SerializeField] private GameObject slotPrefab;
	[SerializeField] private float cameraMargin = 1.0f;

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
		transform.position -= transform.right * ((boardSize.x / 2.0f) - slotWidth);
		transform.position -= transform.up * ((boardSize.y / 2.0f) - slotWidth);
	}

	public void SetCameraSize()
	{
		Camera.main.orthographicSize = (boardSize.y / 2.0f) + cameraMargin;
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
