using UnityEngine;

[ExecuteInEditMode]
public class Slot : MonoBehaviour
{
	[SerializeField] private SpriteRenderer background;

	public void SetBackgroundColor()
	{
		SlotItem item = GetComponentInChildren<SlotItem>();
		if (item)
		{
			background.color = item.ItemColor;
		}
		else
		{
			background.color = Color.white;
		}
	}

	private void Update()
	{
		SetBackgroundColor();
	}
}
