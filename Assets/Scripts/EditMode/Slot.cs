using UnityEngine;

[ExecuteInEditMode]
public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private SpriteRenderer background;

	[SerializeField] private Color borderDefaultColor = Color.white;
	[SerializeField] private Color backgroundDefaultColor = Color.black;

	private Vector2Int gridPosition;
	private bool positionWasSet = false;

	private SlotItem insertedItem;

	public Vector2Int GridPosition => gridPosition;

	public void InsertItem(SlotItem item)
	{
		if (item != null)
		{
			Empty();

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
			DestroyImmediate(insertedItem.gameObject);
			insertedItem = null;
		}
	}

	public void UpdateInsertedItem()
	{
		insertedItem = GetComponentInChildren<SlotItem>();
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
		UpdateInsertedItem();
		SetBackgroundColor();
	}
}
