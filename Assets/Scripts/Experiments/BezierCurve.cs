using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experiments
{
    public class BezierCurve : MonoBehaviour
    {
        public Vector3[] points;

        public void Reset()
        {
            points = new Vector3[]
            {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4, 0f, 0f),
            };
        }

        public Vector3 GetPoint(float t)
        {
            Vector3 point = Bezier.GetPoint2(points[0], points[1], points[2], points[3], t);
            return transform.TransformPoint(point);
        }

        public Vector3 GetVelocity(float t)
        {
            var tangent = Bezier.GetFirstDerivative2(points[0], points[1], points[2], points[3], t);
            return
                transform.TransformPoint(tangent - transform.position);
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }
    }

}
