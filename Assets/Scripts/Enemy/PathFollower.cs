using UnityEngine;
using SplineFramework;

public class PathFollower : MonoBehaviour
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
        progress = Mathf.Clamp01(progress + (Time.deltaTime * speedScale) / duration);
        Vector3 position = spline.GetPoint(progress);
        transform.position = position;
        var targetRotation = Quaternion.FromToRotation(transform.position, spline.GetDirection(progress));
        transform.rotation = targetRotation;
    }

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        progress = 0f;
    }
}
