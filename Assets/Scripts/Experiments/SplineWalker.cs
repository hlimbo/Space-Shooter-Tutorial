using UnityEngine;
using Experiments;

public class SplineWalker : MonoBehaviour
{
    public enum SplineWalkerMode
    {
        Once,
        Loop,
        PingPong,
    };

    [SerializeField]
    private SplineWalkerMode mode;
    [SerializeField]
    private bool goingForward = true;

    [SerializeField]
    private BezierSpline spline;

    // the time it takes to complete the entire spline path
    [SerializeField]
    private float duration;

    // measured between 0 and 1 (parametric t variable)
    private float progress;

    [SerializeField]
    private bool lookForward;

    private void Update()
    {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if(mode == SplineWalkerMode.Once)
                {
                    progress = 1f;
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        }
        else
        {
            // Going backwards
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }

        // teleport to the next point defined by the Bezier Spline
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        
        // always look ahead of the path when moving
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}
