using UnityEngine;

public class SlotItem : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;
	[SerializeField] private string typeID;

	public Color ItemColor => color;

	public string TypeID => typeID;
}
