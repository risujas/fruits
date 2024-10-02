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

	public Vector2Int GridPosition => gridPosition;

	private SlotItem insertedItem;
	public SlotItem InsertedItem => insertedItem;

	public void InsertItem(SlotItem item)
	{
		Empty();

		insertedItem = item;
		insertedItem.transform.position = transform.position;
		insertedItem.transform.rotation = Quaternion.identity;
		insertedItem.transform.parent = transform;
	}

	public void Empty()
	{
		if (insertedItem != null)
		{
			DestroyImmediate(insertedItem.gameObject);
			insertedItem = null;
		}
	}

	public void SetBackgroundColor()
	{
		SlotItem item = GetComponentInChildren<SlotItem>();
		if (item)
		{
			background.color = item.ItemColor;
		}
		else
		{
			background.color = backgroundDefaultColor;
		}
	}

	public void ApplyBorderHighlight()
	{
		if (InsertedItem)
		{
			border.color = InsertedItem.ItemColor * 1.1f;
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
