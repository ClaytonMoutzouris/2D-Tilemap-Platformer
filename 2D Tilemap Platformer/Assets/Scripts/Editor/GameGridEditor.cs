using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameGrid GameGrid = (GameGrid)target;
        if (GUILayout.Button("Resize Maps"))
        {
            GameGrid.ResizeMaps();
        }

    }

}