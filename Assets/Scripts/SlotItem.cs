using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;
	[SerializeField] private string typeID;

	public bool IsPartOfSet { get; set; } = false;

	public Color ItemColor => color;

	public string TypeID => typeID;
}
