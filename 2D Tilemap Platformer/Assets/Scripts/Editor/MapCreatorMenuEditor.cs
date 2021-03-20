using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapCreatorMenu))]
public class MapCreatorMenuEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapCreatorMenu MapCreator = (MapCreatorMenu)target;
        if (GUILayout.Button("New Map"))
        {
            MapCreator.NewMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            MapCreator.LoadMap();
        }

        if (GUILayout.Button("Save Map"))
        {
            MapCreator.SaveMap();
        }
    }

}