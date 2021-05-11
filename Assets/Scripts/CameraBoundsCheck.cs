using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsCheck : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private bool isVisibleInCamera;
    [SerializeField]
    private float yOffset = 0f;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    public bool IsOutOfBounds
    {
        get
        {
            bool isBottom = transform.position.y < mainCamera.transform.position.y - mainCamera.orthographicSize - yOffset;
            //Debug.Log("isBottom " + isBottom);
            
            bool isAbove = transform.position.y > mainCamera.transform.position.y + mainCamera.orthographicSize + yOffset;
            //Debug.Log("isAbove " + isAbove);

            // Orthographic width: https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
            float height = 2f * mainCamera.orthographicSize;
            float width = height * mainCamera.aspect;
            bool isLeft = transform.position.x < mainCamera.transform.position.x - (width / 2f);
            //Debug.Log("isLeft " + isLeft);

            bool isRight = transform.position.x > mainCamera.transform.position.x + (width / 2f);
            //Debug.Log("isRight " + isRight);

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
