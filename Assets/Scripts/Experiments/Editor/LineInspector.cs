using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
    // Can be used to draw graphics in the scene view of our component
    private void OnSceneGUI()
    {
        // target is an Object type inherited from Editor class
        // that is the object we inspect in the scene and is being casted
        // as a Line during runtime
        Line line = target as Line;

        // Handles operates in world space while points
        // are in local space of line
        // need to convert from local to world space
        Transform handleTransform = line.transform;
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        // Rotation handle
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);

        // Add rotation handles
        // Update handle positions
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            // Enables Undo Operation in Unity Editor
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            // Enables Undo Operation in Unity Editor
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }

    }
}
