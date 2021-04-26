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
        transform.localPosition = position;
        transform.LookAt(position + spline.GetDirection(progress));
    }

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        progress = 0f;
    }
}
