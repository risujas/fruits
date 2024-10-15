using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private SpriteRenderer background;

	[SerializeField] private Color borderDefaultColor = Color.white;
	[SerializeField] private Color backgroundDefaultColor = Color.white;

	[SerializeField, ReadOnly] private Vector2Int gridPosition;
	private bool positionWasSet = false;

	private Color targetBackgroundColor;

	private const float colorLerpSpeed = 5.0f;

	private const float itemFallDurationPerUnit = 0.075f;

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
				DestroyInsertedItem();
			}

			item.transform.parent = transform;
			item.transform.rotation = Quaternion.identity;

			if (useSlowTransition)
			{
				float distance = Vector3.Distance(transform.position, item.transform.position);
				item.MoveToPosition(transform.position, itemFallDurationPerUnit * distance);
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

	public void DestroyInsertedItem()
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

	public void DetachInsertedItem()
	{
		if (insertedItem != null)
		{
			insertedItem.transform.parent = null;
		}
	}

	public void SetBackgroundColor()
	{
		targetBackgroundColor = insertedItem == null ? backgroundDefaultColor : insertedItem.PrimaryColor;
		background.color = Color.Lerp(background.color, targetBackgroundColor, Time.deltaTime * colorLerpSpeed);
	}

	public void ApplyBorderHighlight()
	{
		if (insertedItem != null)
		{
			border.color = insertedItem.PrimaryColor * 1.1f;
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
