using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer border;
	[SerializeField] private Color normalColor = Color.white;
	[SerializeField] private Color highlightColor = Color.grey;

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

	public void ApplyBorderHighlight()
	{
		border.color = highlightColor;
	}

	public void RemoveBorderHighlight()
	{
		border.color = normalColor;
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
}
