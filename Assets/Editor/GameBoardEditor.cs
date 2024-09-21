using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameBoardCreation))]
public class GameBoardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameBoardCreation gameBoardCreation = (GameBoardCreation)target;
		if (GUILayout.Button("Create Slots"))
		{
			gameBoardCreation.CreateSlots();
			gameBoardCreation.SetOffset();
		}
	}
}
