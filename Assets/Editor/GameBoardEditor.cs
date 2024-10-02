using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameBoardCreation))]
public class GameBoardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameBoardCreation gameBoardCreation = (GameBoardCreation)target;

		GUILayout.Label("\nInitialization");

		if (GUILayout.Button("Create Slots"))
		{
			gameBoardCreation.CreateSlots();
			gameBoardCreation.SetOffset();
		}

		if (GUILayout.Button("Set Camera Size"))
		{
			gameBoardCreation.SetCameraSize();
		}

		if (GUILayout.Button("Fill With Random Items"))
		{
			gameBoardCreation.FillWithRandomItems();
		}
	}
}
