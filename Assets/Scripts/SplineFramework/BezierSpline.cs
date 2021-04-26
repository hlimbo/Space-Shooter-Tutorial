using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplineFramework
{
    public class BezierSpline : MonoBehaviour
    {
        // control points
        [SerializeField]
        private Vector3[] points;

        // Grouped by number of curves
        [SerializeField]
        private BezierControlPointMode[] modes;

        // Does first point connect back to last point?
        private bool loop;
        public bool Loop
        {
            get
            {
                return loop;
            }

            set
            {
                loop = value;
                if(value)
                {
                    // update mode here
                    SetControlPoint(0, points[0]);
                }
            }
        }

        public int ControlPointCount => points.Length;
        public Vector3 GetControlPoint(int index) => points[index];
        public void SetControlPoint(int index, Vector3 point)
        {
            // Enforce middle point's left and right points
            if (index % 3 == 0)
            {
                Vector3 delta = point - points[index];
                // Ensures first point is connected to last point by having the
                // last point share the same position as the first point.
                if (loop)
                {
                    // Wrap around in the points array
                    if (index == 0)
                    {
                        // first point
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[points.Length - 1] = point;
                    }
                    else if (index == points.Length - 1)
                    {
                        // last point
                        points[0] = point;
                        points[1] += delta;
                        points[index - 1] += delta;
                    }
                    else
                    {
                        // in between
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }

                }
                else
                {
                    // Enforce previous point
                    if (index > 0)
                    {
                        points[index - 1] += delta;
                    }
                    // Enforce point ahead of itself
                    if (index + 1 < points.Length)
                    {
                        points[index + 1] += delta;
                    }
                }
            }

            points[index] = point;
            EnforceMode(index);
        }

        public int CurveCount => (points.Length - 1) / 3;

        public Vector3 GetPoint(float t)
        {
            var (clampedT, i) = GetStartingIndexPointFromT(t);
            Vector3 cubicPoint = BezierMath.GetCubicPoint(points[i], points[i + 1], points[i + 2], points[i + 3], clampedT);
            return transform.TransformPoint(cubicPoint);               
        }

        public Vector3 GetVelocity(float t)
        {
            var (clampedT, i) = GetStartingIndexPointFromT(t);
            Vector3 tangent = BezierMath.GetCubicFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], clampedT);
            // tangent point relative to the origin's point (e.g. transform position)
            // convert the local space to 
            return transform.TransformPoint(tangent - transform.position);
        }

        public Vector3 GetDirection(float t) => GetVelocity(t).normalized;

        // Adds new curve to end of spline
        public void AddCurve()
        {
            Vector3 point = points[points.Length - 1];

            // Add 3 new points where each successive point
            // is 1 greather than the previous in the x direction
            Array.Resize(ref points, points.Length + 3);
            for(int i = 3; i > 0; --i)
            {
                point.x += 1f;
                points[points.Length - i] = point;
            }

            // Copy previous control point mode and set it as the control point mode for the new curve
            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Length - 4);

            if (loop)
            {
                points[points.Length - 1] = points[0];
                EnforceMode(0);
            }

        }

        // Removes curve from end of spline
        public void RemoveCurve()
        {
            // Don't remove if 1 curve left
            if(points.Length == 3)
            {
                return;
            }

            Array.Resize(ref points, points.Length - 3);
        }

        public void ResetPositions()
        {
            // reset points back to create a horizontal line
            for(int i = 0;i < points.Length; ++i)
            {
                points[i].Set(i + 1, 0f, 0f);
            }

            for(int i = 0;i < modes.Length; ++i)
            {
                modes[i] = BezierControlPointMode.Free;
            }
        }

        public void Reset()
        {
            points = new Vector3[]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(2f, 0f, 0f),
                new Vector3(3f, 0f, 0f),
                new Vector3(4f, 0f, 0f),
            };

            modes = new BezierControlPointMode[]
            {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free,
            };
        }

        public BezierControlPointMode GetControlPointMode(int index) => modes[(index + 1) / 3];

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;

            // Ensure 1st and last mode remain equal when loop is true
            if (loop)
            {
                if (modeIndex == 0)
                {
                    modes[modes.Length - 1] = mode;
                }
                else if (modeIndex == modes.Length - 1)
                {
                    modes[0] = mode;
                }
            }

            EnforceMode(index);
        }

        // ensures that the points that connect one curve and the next curve are continuous
        // e.g. don't have cusps or cause an abrupt change in velocities
        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            // Check if we don't need to enforce any mode
            // in the case the mode is set to free or we're at end points of a curve
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
            {
                return;
            }

            // Locate left point and right control points
            // Determine which point is fixed and which point will be enforced
            // The enforced point will be affected by the Mirror or Aligned Constraint Modes
            // whereas the fixed point is not affected by either modes
            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                enforcedIndex = middleIndex + 1;

                // loop checks
                if (fixedIndex < 0)
                {
                    fixedIndex = points.Length - 2;
                }

                // loop checks
                if (enforcedIndex >= points.Length)
                {
                    enforcedIndex = 1;
                }
            }
            else
            {
                fixedIndex = middleIndex + 1;
                enforcedIndex = middleIndex - 1;

                // loop checks
                if (fixedIndex >= points.Length)
                {
                    fixedIndex = 1;
                }

                // loop checks
                if (enforcedIndex < 0)
                {
                    enforcedIndex = points.Length - 2;
                }
            }

            Vector3 middle = points[middleIndex];

            // Mirror the enforced point ~ have the enforced point be in the opposite direction of the fixed point
            if (mode == BezierControlPointMode.Mirrored)
            {
                Vector3 enforcedTangent = middle - points[fixedIndex];
                points[enforcedIndex] = middle + enforcedTangent;
            }
            else if(mode == BezierControlPointMode.Aligned)
            {
                // Align the enforced point by keeping the enforced point's distance the same as it's previous distance
                // but changing its direction to be the opposite of the fixed point's direction
                Vector3 enforcedTangent = middle - points[fixedIndex];
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
                points[enforcedIndex] = middle + enforcedTangent;
            }
        }

        // used to obtain the nearest control point in points from variable t
        // where t ranges from 0 to 1 as input
        private Tuple<float, int> GetStartingIndexPointFromT(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                // control points are grouped in multiples of 3
                i *= 3;
            }

            return new Tuple<float, int>(t, i);
        }
    }
}
