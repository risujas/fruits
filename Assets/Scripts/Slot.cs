using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private SpriteRenderer background;

	[SerializeField] private Color borderNormalColor = Color.white;
	[SerializeField] private Color borderHighlightColor = Color.grey;
	[SerializeField] private Color backgroundNormalColor;

	private Vector2Int gridPosition;
	private bool positionWasSet = false;

	public Vector2Int GridPosition => gridPosition;

	public SlotItem InsertedItem
	{
		get
		{
			return GetComponentInChildren<SlotItem>();
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
			background.color = backgroundNormalColor;
		}
	}

	public void ApplyBorderHighlight()
	{
		if (InsertedItem)
		{
			border.color = InsertedItem.ItemColor * 1.1f;
		}
		else
		{
			border.color = borderHighlightColor;
		}
	}

	public void RemoveBorderHighlight()
	{
		border.color = borderNormalColor;
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
