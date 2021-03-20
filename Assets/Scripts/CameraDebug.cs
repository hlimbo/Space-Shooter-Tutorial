using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Debug.Log("drawing");
        Camera camera = GetComponent<Camera>();
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(p, 0.1F);
    }
}
