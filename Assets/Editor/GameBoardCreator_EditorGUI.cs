using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameBoardCreator))]
public class GameBoardCreator_EditorGUI : Editor
{
    private GameBoardCreator gameBoardCreator;
    private SerializedProperty availableSlotItems;
    private SerializedProperty selectedItems;

    private void ShowCreateButton()
    {
        if (GUILayout.Button("Create Board Instance"))
        {
            gameBoardCreator.CreateBoard();
            gameBoardCreator.CreateSlots();
            gameBoardCreator.SetOffset();
            gameBoardCreator.SetCameraSize();
        }
    }

    private void ShowFillItemsButton()
    {
        if (GUILayout.Button("Fill With Random Items"))
        {
            gameBoardCreator.FillWithRandomItems();
        }
    }

	private void ShowToggleList()
	{
		serializedObject.Update();

		for (int i = 0; i < availableSlotItems.arraySize; i++)
		{
			SerializedProperty element = availableSlotItems.GetArrayElementAtIndex(i);
			
			if (i >= selectedItems.arraySize)
			{
				selectedItems.InsertArrayElementAtIndex(i);
				selectedItems.GetArrayElementAtIndex(i).objectReferenceValue = null;
			}

			SerializedProperty selectedElement = selectedItems.GetArrayElementAtIndex(i);

			bool isSelected = selectedElement.objectReferenceValue != null;

			string itemName = element.objectReferenceValue != null ? ((SlotItem)element.objectReferenceValue).name : "Null";

			bool newSelection = EditorGUILayout.Toggle(itemName, isSelected);
			if (newSelection && !isSelected)
			{
				selectedElement.objectReferenceValue = element.objectReferenceValue;
			}
			else if (!newSelection && isSelected)
			{
				selectedElement.objectReferenceValue = null;
			}
		}

		serializedObject.ApplyModifiedProperties();
	}

    private void OnEnable()
    {
        gameBoardCreator = (GameBoardCreator)target;
        availableSlotItems = serializedObject.FindProperty("availableSlotItems");
        selectedItems = serializedObject.FindProperty("selectedItems");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = !Application.isPlaying;

		EditorGUILayout.LabelField("----------", GUI.skin.horizontalSlider);
		EditorGUILayout.LabelField("Selected items", EditorStyles.boldLabel);
        ShowToggleList();
		EditorGUILayout.LabelField("----------", GUI.skin.horizontalSlider);
        ShowCreateButton();
        ShowFillItemsButton();

        GUI.enabled = true;
    }
}
