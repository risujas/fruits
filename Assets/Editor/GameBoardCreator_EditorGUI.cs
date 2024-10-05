using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameBoardCreator))]
public class GameBoardCreator_EditorGUI : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUI.enabled = !Application.isPlaying;

		GameBoardCreator gameBoardCreator = (GameBoardCreator)target;

		if (GUILayout.Button("Create Board Instance"))
		{
			gameBoardCreator.CreateBoard();
			gameBoardCreator.CreateSlots();
			gameBoardCreator.SetOffset();
			gameBoardCreator.SetCameraSize();
		}

		if (GUILayout.Button("Fill With Random Items"))
		{
			gameBoardCreator.FillWithRandomItems();
		}

		GUI.enabled = true;
	}
}
