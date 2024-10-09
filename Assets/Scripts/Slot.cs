using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private SpriteRenderer background;

	[SerializeField] private Color borderDefaultColor = Color.white;
	[SerializeField] private Color backgroundDefaultColor = Color.black;

	[SerializeField, ReadOnly] private SlotItem insertedItem;

	[SerializeField, ReadOnly] private Vector2Int gridPosition;
	private bool positionWasSet = false;

	public Vector2Int GridPosition => gridPosition;

	public static Slot FindNearestSlot(Vector3 point)
	{
		Slot nearest = null;

		float nearestDistance = Mathf.Infinity;

		var allSlots = FindObjectsByType<Slot>(FindObjectsSortMode.None);
		foreach (var s in allSlots)
		{
			float distance = Vector3.Distance(point, s.transform.position);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearest = s;
			}
		}

		return nearest;
	}

	public void InsertItem(SlotItem item, bool destroyExistingItem)
	{
		if (item != null)
		{
			if (destroyExistingItem)
			{
				Empty();
			}

			insertedItem = item;
			insertedItem.transform.position = transform.position;
			insertedItem.transform.rotation = Quaternion.identity;
			insertedItem.transform.parent = transform;
		}
	}

	public SlotItem GetItem()
	{
		return insertedItem;
	}

	public void Empty()
	{
		if (insertedItem != null)
		{
			if (Application.isPlaying)
			{
				Destroy(insertedItem.gameObject);
			}
			else
			{
				DestroyImmediate(insertedItem.gameObject);
			}
		}
		insertedItem = null;
	}

	public void SetBackgroundColor()
	{
		if (insertedItem != null)
		{
			background.color = insertedItem.ItemColor;
		}
		else
		{
			background.color = backgroundDefaultColor;
		}
	}

	public void ApplyBorderHighlight()
	{
		if (insertedItem != null)
		{
			border.color = insertedItem.ItemColor * 1.1f;
		}
	}

	public void RemoveBorderHighlight()
	{
		border.color = borderDefaultColor;
	}

	public void SetPosition(int x, int y)
	{
		if (positionWasSet)
		{
			throw new System.Exception("Tried to set the position of a grid slot more than once");
		}

		gridPosition = new Vector2Int(x, y);
		positionWasSet = true;
	}

	private void Update()
	{
		SetBackgroundColor();
	}
}
