using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a curve and not a spline because a spline is a collection
// of curves
public class CatmullRomSpline : MonoBehaviour
{
    // Has to be at least 4 points
    [SerializeField]
    private Transform[] controlPointsList;
    [SerializeField]
    private bool isLooping = true;
    // Spline's resolution,
    // make sure they add up to 1, e.g. 0.3 gives a gap, but 0.2 will work
    [SerializeField]
    float resolution = 0.2f;

    [SerializeField]
    private GameObject cart;
    [SerializeField]
    private int pointIndex = 0;

    [SerializeField]
    private List<Vector3> points = new List<Vector3>();

    [SerializeField]
    private float speedScale = 2f;

    // To move cart around spline... I will need to calculate all
    // the points generated by the spline and update its transform position on each tick

    private void Start()
    {
        CalculateSplinePoints();
    }

    private void Update()
    {
        if (points.Count > 0)
        {
            Vector3 nextPoint = points[pointIndex];

            Vector3 normalPoint = (nextPoint - cart.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(normalPoint);
            cart.transform.rotation = Quaternion.Slerp(cart.transform.rotation, lookRotation, 5f * Time.deltaTime);

            cart.transform.position = Vector3.MoveTowards(cart.transform.position, nextPoint, Time.deltaTime * speedScale);
            
            if((cart.transform.position - nextPoint).sqrMagnitude <= 0.001f)
            {
                pointIndex = (pointIndex + 1) % points.Count;
            }
        }
    }

    // Is potentially a slow algorithm to pre-calculate all points for it to generate a path
    private void CalculateSplinePoints()
    {
        for (int i = 0; i < controlPointsList.Length; ++i)
        {
            //Cant draw between the endpoints
            //Neither do we need to draw from the second to the last endpoint
            //...if we are not making a looping line
            if ((i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1) && !isLooping)
            {
                continue;
            }

            Vector3 p0 = controlPointsList[ClampListPos(i - 1)].position;
            Vector3 p1 = controlPointsList[i].position;
            Vector3 p2 = controlPointsList[ClampListPos(i + 1)].position;
            Vector3 p3 = controlPointsList[ClampListPos(i + 2)].position;

            // Controls number of loops to generate i * j positions
            int loops = Mathf.FloorToInt(1f / resolution);
            for (int j = 1; j <= loops; ++j)
            {
                // t should be between 0 and 1 for points p1 and p2
                float t = j * resolution;
                Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
                points.Add(newPos);
                //yield return new WaitForSeconds(1f);
            }
        }

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // Draw the Catmull-Rom spline between the points
        for (int i = 0; i < controlPointsList.Length; ++i)
        {

            //Cant draw between the endpoints
            //Neither do we need to draw from the second to the last endpoint
            //...if we are not making a looping line
            if ((i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1) && !isLooping)
            {
                continue;
            }

            DisplayCatmullRomSpline(i);
        }
    }

    // 2 mid points p1 and p2 range betweeen 0 and 1 in t space
    // Display spline between 2 points with Catmull-Rom spline algorithm
    void DisplayCatmullRomSpline(int pos)
    {
        Vector3 p0 = controlPointsList[ClampListPos(pos - 1)].position;
        Vector3 p1 = controlPointsList[pos].position;
        Vector3 p2 = controlPointsList[ClampListPos(pos + 1)].position;
        Vector3 p3 = controlPointsList[ClampListPos(pos + 2)].position;

        // Start position of line
        Vector3 lastPos = p1;

        int loops = Mathf.FloorToInt(1f / resolution);

        for (int i = 1; i <= loops; ++i)
        {
            // t should be between 0 and 1 for points p1 and p2
            float t = i * resolution;
            Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }

    }

    // clamp positions for looping
    int ClampListPos(int pos)
    {
        // wrap to the last point in the list
        if (pos < 0)
        {
            pos = controlPointsList.Length - 1;
        }

        // This can probably be condensed to pos % controlPointsList.Length;
        if (pos > controlPointsList.Length)
        {
            pos = 1;
        }
        else if (pos > controlPointsList.Length - 1)
        {
            pos = 0;
        }

        return pos;
    }

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // I have no idea how these coefficients were derived ~~ may need multivariable calculus help for this lol
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}
