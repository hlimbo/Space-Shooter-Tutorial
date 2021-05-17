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

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        progress = 0f;
    }

    void IMovable.Move(float deltaTime)
    {
        if(spline != null)
        {
            progress = Mathf.Clamp01(progress + (deltaTime * speedScale) / duration);
            Vector3 position = spline.GetPoint(progress);

            // Set this game-object's local up axis (green axis) to point towards the direction its traveling on the spline
            transform.up = transform.position - position;
            transform.position = position;
        }
    }

    public void AssignPath(BezierSpline spline)
    {
        this.spline = spline;
    }
}
