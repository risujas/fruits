using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private SpriteRenderer background;

	[SerializeField] private Color borderDefaultColor = Color.white;
	[SerializeField] private Color backgroundDefaultColor = Color.black;

	[SerializeField, ReadOnly] private Vector2Int gridPosition;
	private bool positionWasSet = false;

	public Vector2Int GridPosition => gridPosition;

	public SlotItem insertedItem => GetComponentInChildren<SlotItem>();

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

	public void InsertItem(SlotItem item, bool destroyExistingItem, bool useSlowTransition)
	{
		if (item != null)
		{
			if (destroyExistingItem)
			{
				Empty();
			}

			item.transform.parent = transform;
			item.transform.rotation = Quaternion.identity;

			if (useSlowTransition)
			{
				item.MoveToPosition(transform.position, 0.2f);
			}
			else
			{
				item.transform.position = transform.position;
			}
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
