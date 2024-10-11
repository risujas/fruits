using UnityEditor;
using UnityEditor.SceneManagement;
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
        if (Application.isPlaying)
        {
            return;
        }

        if (PrefabStageUtility.GetCurrentPrefabStage())
        {
            return;
        }

        FindSlot();
        MatchSlotPosition();
    }
}
