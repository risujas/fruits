using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameBoardCreator))]
public class GameBoardCreator_EditorGUI : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameBoardCreator gameBoardCreator = (GameBoardCreator)target;

		GUILayout.Label("\nInitialization");

		if (GUILayout.Button("Create Board Instance"))
		{
			gameBoardCreator.CreateBoard();
		}

		if (GUILayout.Button("Create Slots"))
		{
			gameBoardCreator.CreateSlots();
			gameBoardCreator.SetOffset();
		}

		if (GUILayout.Button("Destroy Slots"))
		{
			gameBoardCreator.DestroySlots();
		}

		if (GUILayout.Button("Set Camera Size"))
		{
			gameBoardCreator.SetCameraSize();
		}

		if (GUILayout.Button("Fill With Random Items"))
		{
			gameBoardCreator.FillWithRandomItems();
		}
	}
}
