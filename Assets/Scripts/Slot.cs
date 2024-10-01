using UnityEngine;

public class Slot : MonoBehaviour
{
	public int x;
	public int y;

	public SlotItem InsertedItem
	{
		get
		{
			return GetComponentInChildren<SlotItem>();
		}
	}
}
