using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField, ReadOnly] private SlotItem insertedItem;

	public SlotItem InsertedItem => insertedItem;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
