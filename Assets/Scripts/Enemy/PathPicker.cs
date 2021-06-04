using SplineFramework;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class PathPicker : MonoBehaviour
{
    private BezierSpline[] possiblePaths;
    private PathFollower follower;

    private void Awake()
    {
        possiblePaths = GameObject.FindGameObjectsWithTag("PossiblePaths")
         .Select(g => g.GetComponent<BezierSpline>()).ToArray(); ;
        follower = GetComponent<PathFollower>();
    }

    private void Start()
    {
        Assert.IsTrue(possiblePaths.Length > 0, "Possible Bezier Spline Paths not found in the scene. Make sure to spawn them first");
        int randomPathIndex = Random.Range(0, possiblePaths.Length);
        follower.AssignPath(possiblePaths[randomPathIndex]);
    }

    
}
