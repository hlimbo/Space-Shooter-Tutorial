using UnityEngine;
using SplineFramework;
using UnityEngine.Assertions;
using System.Linq;

public class PathFollower : MonoBehaviour, IMovable
{
    [SerializeField]
    private BezierSpline spline;

    // the time it takes to complete the entire path
    [SerializeField]
    private float duration;

    [SerializeField]
    private float speedScale;

    // Measured between 0 and 1 (parametric t variable)
    private float progress = 0f;

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        progress = 0f;
    }

    void IMovable.Move(float deltaTime)
    {
        if (enabled)
        {
            if (spline != null)
            {
                progress = Mathf.Clamp01(progress + (deltaTime * speedScale) / duration);
                Vector3 position = spline.GetPoint(progress);

                // Set this game-object's local up axis (green axis) to point towards the direction its traveling on the spline
                transform.up = (transform.position - position).normalized;
                transform.position = position;
            }
            else
            {
                // Find random path to follow in the event PathPicker does not pick
                // a valid random path for the enemy
                BezierSpline[] paths = GameObject.FindGameObjectsWithTag("PossiblePaths")
                    .Select(g => g.GetComponent<BezierSpline>()).ToArray();

                Assert.IsTrue(paths.Length > 0, "Possible Bezier Spline Paths not in the scene. Make sure to spawn them first");
                int randomPathIndex = Random.Range(0, paths.Length);
                AssignPath(paths[randomPathIndex]);
            }
        }
    }

    public void AssignPath(BezierSpline spline)
    {
        this.spline = spline;
    }
}
