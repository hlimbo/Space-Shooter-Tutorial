using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experiments;


// This script is an example of how you spawn game objects along a spline

// Instantiates a sequence of game objects along a spline when it awakens
// I can use this to spawn a swarm of enemies
public class SplineDecorator : MonoBehaviour
{
    // Path used to spawn game objects along its path
    [SerializeField]
    private BezierSpline spline;

    // controls the step size of where to place the game object along the parametric curve
    // the higher the number the smaller the step size | the lower the number the bigger the step size
    [SerializeField]
    private int frequency;

    [SerializeField]
    private bool lookForward;

    [SerializeField]
    private Transform[] items;

    private void Awake()
    {
        if (frequency <= 0 || items == null || items.Length == 0)
        {
            return;
        }

        float stepSize = 1f / (frequency * items.Length);
        for (int p = 0, f = 0; f < frequency; ++f)
        {
            for (int i = 0;i < items.Length; ++i, ++p)
            {
                Transform item = Instantiate(items[i]);
                Vector3 position = spline.GetPoint(p * stepSize);
                item.transform.localPosition = position;

                if (lookForward)
                {
                    item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                }

                item.transform.parent = transform;
            }
        }
    }
}
