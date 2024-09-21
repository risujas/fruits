using UnityEditor.SceneManagement;

using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
	private Slot FindNearestSlot()
	{
		Slot nearest = null;

		float nearestDistance = Mathf.Infinity;

		var allSlots = FindObjectsByType<Slot>(FindObjectsSortMode.None);
		foreach (var s in allSlots)
		{
			float distance = Vector3.Distance(transform.position, s.transform.position);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearest = s;
			}
		}

		return nearest;
	}

	private void Update()
	{
		if (!Application.isPlaying && !PrefabStageUtility.GetCurrentPrefabStage())
		{
			Slot nearest = FindNearestSlot();
			if (nearest)
			{
				gameObject.transform.position = nearest.transform.position;
				gameObject.transform.parent = nearest.transform;
			}
		}
	}
}
