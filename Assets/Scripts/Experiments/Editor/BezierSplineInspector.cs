using UnityEditor;
using UnityEngine;

namespace Experiments
{
    // Editor is derived from here to create custom inspector or editor for the component
    // Can be used to change the appearance in the editor
    // https://docs.unity3d.com/ScriptReference/Editor.html
    // This attribute is specified here to indicate this is an editor for the BezierSpline component
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : Editor
    {
        private static Color[] modeColors =
        {
            Color.white, // Free
            Color.yellow, // Aligned
            Color.cyan, // Mirrored
        };

        private const int lineSteps = 10;
        private const float directionScale = 0.5f;
        private const int stepsPerCurve = 10;

        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;
        private int selectedIndex = -1;

        private BezierSpline spline;
        private Transform handleTransform;
        private Quaternion handleRotation;

        // Q: Why assign spline in OnSceneGUI and OnInspectorGUI?
        // A: Both methodds act independent of one another
        // OnInspectorGUI is called once for the entire component selection
        // OnSceneGUI is called once for each appropriate component in the selection
        // and each time target changes

        // https://docs.unity3d.com/ScriptReference/Editor.OnSceneGUI.html
        // Enables the Editor to handle an event in scene view
        //Considered as the starting point to render handles in scene view in the editor
        private void OnSceneGUI()
        {
            // https://docs.unity3d.com/ScriptReference/Editor-target.html
            // target is a property from Editor class that tells us which object is being inspected in the editor
            spline = target as BezierSpline;
            handleTransform = spline.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for (int i = 1; i < spline.ControlPointCount; i += 3)
            {
                // Renders the control points (triplet points)
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector3 p3 = ShowPoint(i + 2);

                // https://docs.unity3d.com/ScriptReference/Handles.html
                Handles.color = Color.green;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);
                // End render control points //

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }
            ShowDirections();
        }

        // https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html
        // Implement this function to add your own custom IMGUI for the inspector of a specific object class
        // Changes how the component displays information to the developer in the inspector tab when the associated component is attached to a gameobject
        public override void OnInspectorGUI()
        {
            // Calling this function will allow us to view all fields in BezierSpline
            // annotated with [SerializeField] attribute to be visible in the Inspector tab in the editor
            base.OnInspectorGUI();

            spline = target as BezierSpline;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Toggle Loop");
                EditorUtility.SetDirty(spline);
                spline.Loop = loop;
            }


            if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(spline, "Add Curve");
                spline.AddCurve();
                EditorUtility.SetDirty(spline);
            }

            if (GUILayout.Button("Reset Spline"))
            {
                Undo.RecordObject(spline, "Reset Spline");
                spline.Reset();
                EditorUtility.SetDirty(spline);
            }
        }

        private void ShowDirections()
        {
            Handles.color = Color.magenta;
            Vector3 point = spline.GetPoint(0f);
            Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * spline.CurveCount;
            for (int i = 1; i <= steps; i++)
            {
                point = spline.GetPoint(i / (float)steps);
                Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
            }
        }


        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));

            // Keeps size of dots fixed size
            float size = HandleUtility.GetHandleSize(point);
            // make size of first point on spline twice as big than the rest
            if (index == 0)
            {
                size *= 2f;
            }

            Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
            if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
            {
                selectedIndex = index;
                // Refresh Unity Inspector when a point is selected from the editor
                Repaint();
            }

            // change handle to active if selected
            if (selectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Point");
                    EditorUtility.SetDirty(spline);
                    spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
                }
            }


            return point;
        }

        private void DrawSelectedPointInspector()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Selected Point", spline.GetControlPoint(selectedIndex));

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(selectedIndex, point);
            }

            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)
                EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Change Point Mode");
                spline.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(spline);
            }
        }
    }
}


