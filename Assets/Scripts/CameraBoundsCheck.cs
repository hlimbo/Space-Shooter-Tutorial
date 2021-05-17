using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsCheck : MonoBehaviour
{
    private Camera mainCamera;
    private float camWidth;
    private float camHeight;

    [SerializeField]
    private bool isVisibleInCamera;
    [SerializeField]
    private float yOffset = 0f;

    void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();

        // Orthographic width: https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;
    }

    public bool IsOutOfBounds
    {
        get
        {
            bool isBottom = transform.position.y < mainCamera.transform.position.y - mainCamera.orthographicSize - yOffset;            
            bool isAbove = transform.position.y > mainCamera.transform.position.y + mainCamera.orthographicSize + yOffset;

            bool isLeft = transform.position.x < mainCamera.transform.position.x - (camWidth / 2f);
            bool isRight = transform.position.x > mainCamera.transform.position.x + (camWidth / 2f);

            return isBottom || isAbove || isLeft || isRight;
        }
    }

    public bool IsInBounds => !IsOutOfBounds;

    private void Update()
    {
        if(!isVisibleInCamera && IsInBounds)
        {
            isVisibleInCamera = true;
        }
        else if(isVisibleInCamera && IsOutOfBounds)
        {
            Destroy(gameObject);
        }
    }
}
