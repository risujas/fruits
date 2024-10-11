using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
    private Slot slot;

    private void FindSlot()
    {
        slot = Slot.FindNearestSlot(transform.position);
        if (slot.GetItem() == null)
        {
            slot.InsertItem(GetComponent<SlotItem>(), false);
        }
    }

    private void MatchSlotPosition()
    {
        slot = GetComponentInParent<Slot>();
        if (slot != null)
        {
            transform.position = slot.transform.position;
            transform.parent = slot.transform;
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
