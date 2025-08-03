#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DissolveView))]
public class DissolveViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DissolveView view = (DissolveView)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Dissolve Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Dissolve"))
        {
            view.StartDissolve();
        }

        if (GUILayout.Button("Undissolve"))
        {
            view.StartUndissolve();
        }

        if (GUILayout.Button("Toggle Distortion"))
        {
            view.ToggleDistortion();
        }
    }
}
#endif

