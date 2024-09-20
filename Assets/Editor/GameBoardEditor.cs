using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(GameBoard))]
public class GameBoardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameBoard gameBoard = (GameBoard)target;
		if (GUILayout.Button("Create Slots"))
		{
			gameBoard.CreateSlots();
			gameBoard.SetOffset();
		}
	}
}
