using UnityEditor.SceneManagement;

using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
	private void Update()
	{
		if (!Application.isPlaying && !PrefabStageUtility.GetCurrentPrefabStage())
		{
			Slot nearest = Slot.FindNearestSlot(transform.position);
			if (nearest)
			{
				gameObject.transform.position = nearest.transform.position;
				gameObject.transform.parent = nearest.transform;
			}
		}
	}
}
