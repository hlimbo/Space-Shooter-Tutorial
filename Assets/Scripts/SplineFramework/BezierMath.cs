using UnityEngine;

namespace SplineFramework
{
    // See CatlikeCoding Bezier Splines tutorial
    public static class BezierMath
    {
        public static Vector3 GetQuadraticPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            /*
             * Formula
             *  B(t) = ((1 -  t) * (1 - t) * P0) + t * P1) + t((1 - t) * P1 + t * P2)
             */

            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2f * oneMinusT * t * p1 +
                t * t * p2;
        }

        public static Vector3 GetCubicPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            /*
             * Formula
             *   B(t) = (1 - t)^3 * p0 + 3f * (1 - t)^2 * t * p1 + 3f * (1 - t) * t^2 * p2 + t^3 * p3;
             */ 

            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        // Need to understand power rule and some chain rule from Calculus 1 course
        // Obtains tangent to the curve at some variable t which can be interpreted as the speed in which an object moves along the curve
        public static Vector3 GetQuadraticFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            return
                2f * (1f - t) * (p1 - p0) +
                2f * t * (p2 - p1);
        }

        public static Vector3 GetCubicFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }

    }
}

