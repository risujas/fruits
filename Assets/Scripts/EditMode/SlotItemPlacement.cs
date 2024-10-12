#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
	private Slot slot;

	private void FindSlot()
	{
		var nearest = Slot.FindNearestSlot(transform.position);
		if (nearest.GetItem() == null)
		{
			slot = nearest;
			transform.parent = slot.transform;
		}
	}

	private void MatchSlotPosition()
	{
		slot = GetComponentInParent<Slot>();
		if (slot != null)
		{
			transform.position = slot.transform.position;
		}
	}

	private void Update()
	{
		if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
		{
			return;
		}

		FindSlot();
		MatchSlotPosition();

	}
}
#endif