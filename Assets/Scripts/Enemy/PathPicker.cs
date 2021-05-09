using SplineFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PathPicker : MonoBehaviour
{
    private BezierSpline[] possiblePaths;
    private PathFollower follower;

    private void Awake()
    {
        possiblePaths = FindObjectsOfType<BezierSpline>();
        follower = GetComponent<PathFollower>();

        if(possiblePaths.Length > 0)
        {
            Assert.IsTrue(possiblePaths.Length > 0, "Possible Bezier Spline Paths not found in the scene. Make sure to spawn them first");

            int randomPathIndex = Random.Range(0, possiblePaths.Length);
            follower.AssignPath(possiblePaths[randomPathIndex]);
        }
    }

    
}
