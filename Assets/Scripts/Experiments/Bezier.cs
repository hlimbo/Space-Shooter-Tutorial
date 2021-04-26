using UnityEngine;
namespace Experiments 
{
    public static class Bezier
    {
        // could generalize to higher order polynomials by using recursion
        // but the cost would be using more call stack space
        // benefit: you can specify what degree the polynomial can be
        // I think the higher the polynomial exponent, the smoother the curves become
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            //// Lerp between first and 2nd point
            //var lerpPoint1 = Vector3.Lerp(p0, p1, t);
            //// Lerp between 2nd and last point
            //var lerpPoint2 = Vector3.Lerp(p1, p2, t);
            //// and input those intermediate results and in the lerp function
            //return Vector3.Lerp(lerpPoint1, lerpPoint2, t);

            // Alternatively, it can be written like this using the quadratic formula
            // B(t) = (1 - t)((1 - t)P0 + tP1) + t((1 - t)P1 + tP2)

            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2f * oneMinusT * t * p1 +
                t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            // Need some Algebra 2 skills to simplify it down to this form
            // where it looks like Linear Interpolation (e.g. (a * (1 - t) + b * t)

            // The first derivative here obtains the  tangent to the curve or
            // the speed in which we move along the curve
            return
                2f * (1f - t) * (p1 - p0) +
                2f * t * (p2 - p1);
        }

        // Cubic Bezier Formula
        public static Vector3 GetPoint2(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var lerpPoint1 = Vector3.Lerp(p0, p1, t);
            // Lerp between 2nd and last point
            var lerpPoint2 = Vector3.Lerp(p1, p2, t);
            // and input those intermediate results and in the lerp function
            var quadLerp1 = Vector3.Lerp(lerpPoint1, lerpPoint2, t);
            var lerpPoint3 = Vector3.Lerp(p2, p3, t);
            var quadLerp2 = Vector3.Lerp(lerpPoint2, lerpPoint3, t);
            return Vector3.Lerp(quadLerp1, quadLerp2, t);

            //t = Mathf.Clamp01(t);
            //float oneMinusT = 1f - t;
            //return
            //    oneMinusT * oneMinusT * oneMinusT * p0 +
            //    3f * oneMinusT * oneMinusT * t * p1 +
            //    3f * oneMinusT * t * t * p2 +
            //    t * t * t * p3;
        }

        public static Vector3 GetFirstDerivative2(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
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

