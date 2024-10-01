using UnityEngine;

[ExecuteInEditMode]
public class SlotColorSetter : MonoBehaviour
{
	[SerializeField] private SpriteRenderer target;
	[SerializeField] private Color defaultColor;

	public void SetBackgroundColor()
	{
		SlotItem item = GetComponentInChildren<SlotItem>();
		if (item)
		{
			target.color = item.ItemColor;
		}
		else
		{
			target.color = defaultColor;
		}
	}

	private void Update()
	{
		SetBackgroundColor();
	}
}
