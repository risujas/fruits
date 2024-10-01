using UnityEditor.SceneManagement;

using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
	public static Slot FindNearestSlot(Vector3 point)
	{
		Slot nearest = null;

		float nearestDistance = Mathf.Infinity;

		var allSlots = FindObjectsByType<Slot>(FindObjectsSortMode.None);
		foreach (var s in allSlots)
		{
			float distance = Vector3.Distance(point, s.transform.position);
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
			Slot nearest = FindNearestSlot(transform.position);
			if (nearest)
			{
				gameObject.transform.position = nearest.transform.position;
				gameObject.transform.parent = nearest.transform;
			}
		}
	}
}
