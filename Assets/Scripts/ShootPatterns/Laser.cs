using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Move Laser upwards
    // Speed Variable - move 8 meters per second
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private int yDirection = 1;

    private Camera mainCamera;
    private float camHeight;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        camHeight = 2f * mainCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * yDirection * Time.deltaTime);

        float maxY = mainCamera.transform.position.y + (camHeight / 2f);
        float minY = mainCamera.transform.position.y - (camHeight / 2f);
        if(transform.position.y > maxY || transform.position.y < minY)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
