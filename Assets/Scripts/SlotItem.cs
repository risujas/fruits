using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;

	public Color ItemColor => color;
}
