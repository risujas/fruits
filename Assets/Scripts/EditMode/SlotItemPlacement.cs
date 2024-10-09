using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class SlotItemPlacement : MonoBehaviour
{
    private bool isMouseButtonPressed = false;
    private bool hasProcessedMouseRelease = false;

    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        if (!PrefabStageUtility.GetCurrentPrefabStage())
        {
            Slot nearest = Slot.FindNearestSlot(transform.position);
            if (nearest)
            {
                transform.position = nearest.transform.position;
                transform.parent = nearest.transform;

                DetectMouseInput();

                if (!isMouseButtonPressed && !hasProcessedMouseRelease)
                {
                	nearest.InsertItem(GetComponent<SlotItem>(), true);
                    hasProcessedMouseRelease = true; 
                }
            }
        }
    }

    private void DetectMouseInput() // this shit don't work
    {
        if (!Application.isPlaying)
        {
            Event e = Event.current;

            if (e != null && e.type == EventType.MouseDown && e.button == 0)
            {
                isMouseButtonPressed = true;
                hasProcessedMouseRelease = false; 
				Debug.Log("Mouse Down");
            }

            if (e != null && e.type == EventType.MouseUp && e.button == 0)
            {
                isMouseButtonPressed = false;
				Debug.Log("Mouse Up");
            }
        }
    }
}
