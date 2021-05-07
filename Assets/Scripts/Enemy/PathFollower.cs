using UnityEngine;
using SplineFramework;

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

    void Update()
    {
        // May need to cut out rotation here as control points of spline influence where the ship should rotate to greatly
        // just use it to follow a path for now
        //var targetRotation = Quaternion.FromToRotation(transform.position, spline.GetDirection(progress));
        //transform.rotation = targetRotation;
    }

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        progress = 0f;
    }

    void IMovable.Move(float deltaTime)
    {
        progress = Mathf.Clamp01(progress + (deltaTime * speedScale) / duration);
        Vector3 position = spline.GetPoint(progress);
        transform.position = position;
    }
}
