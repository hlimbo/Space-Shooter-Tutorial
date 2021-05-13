using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPicker : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private float xOffset;
    [SerializeField]
    private float yOffset;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        transform.position = SpawnAtRandomPosition();
    }

    private Vector3 SpawnAtRandomPosition ()
    {
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        var randomPosition = new Vector3();

        randomPosition.x = Random.Range(mainCamera.transform.position.x - (width / 2) + xOffset, mainCamera.transform.position.x + (width / 2) - xOffset);
        randomPosition.y = mainCamera.transform.position.y + (height / 2) + yOffset;
        randomPosition.z = 0f;
        return randomPosition;
    }
}
