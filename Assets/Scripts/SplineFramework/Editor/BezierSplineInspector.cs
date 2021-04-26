using UnityEngine;
using UnityEditor;

namespace SplineFramework
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : Editor
    {
        private static Color[] modeColors =
{
            Color.white, // Free
            Color.yellow, // Aligned
            Color.cyan, // Mirrored
        };

        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;

        private BezierSpline spline;
        private Transform handleTransform;
        private Quaternion handleRotation;
        private int selectedIndex = -1;

        private void OnSceneGUI()
        {
            // Debug.Log("OnSceneGUI - BezierSplineInspector");

            spline = target as BezierSpline;
            handleTransform = spline.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for(int i = 1; i < spline.ControlPointCount; i += 3)
            {
                // Render control points
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector3 p3 = ShowPoint(i + 2);

                Handles.color = Color.green;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);
                // End Render Control Points

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }
        }

        public override void OnInspectorGUI()
        {
            // This base function should auto-serialize all fields of this component
            // and expose in the Unity Inspector Tab that are public fields or 
            // fields annotated with the [SerializeField] attribute
            // Excluding this base function call here will hide the fields specified above
            // base.OnInspectorGUI();

           //  Debug.Log("OnInspectorGUI - BezierSplineInspector called");

            spline = target as BezierSpline;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Toggle Loop");
                EditorUtility.SetDirty(spline);
                spline.Loop = loop;
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(spline, "Add Curve");
                spline.AddCurve();
                EditorUtility.SetDirty(spline);
            }

            if (GUILayout.Button("Remove Curve"))
            {
                Undo.RecordObject(spline, "Remove Curve");
                spline.RemoveCurve();
                EditorUtility.SetDirty(spline);
            }

            if (GUILayout.Button("Reset Spline Positions"))
            {
                Undo.RecordObject(spline, "Reset Spline Positions");
                spline.ResetPositions();
                EditorUtility.SetDirty(spline);
            }

            if (GUILayout.Button("Reset Spline to 1 Curve"))
            {
                Undo.RecordObject(spline, "Reset Spline to 1 Curve");
                spline.Reset();
                EditorUtility.SetDirty(spline);
            }

            if(selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

        }

        // Can possibly create editor tools using UI elements to further explore
        private Vector3 ShowPoint(int index)
        {
            if(handleTransform == null || handleRotation == null)
            {
                Debug.LogWarning("handleTransform or handleRotation are NULL");
                return Vector3.zero;
            }

            Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));

            // keep size of handle fixed
            float size = HandleUtility.GetHandleSize(point);
            // make first point size on spline twice as big than the rest
            if (index == 0)
            {
                size *= 2f;
            }

            // change handle to active if selected
            if (selectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                Handles.color = Color.red;
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Point");
                    EditorUtility.SetDirty(spline);
                    spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
                }
            }
            else
            {
                Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
            }

            if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
            {
                selectedIndex = index;
                // Refresh Unity Inspector when a point is selected from the editor
                // https://docs.unity3d.com/ScriptReference/Editor.Repaint.html
                Repaint();
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


