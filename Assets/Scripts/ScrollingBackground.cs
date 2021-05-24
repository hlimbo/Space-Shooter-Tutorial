using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(RawImage))]
public class ScrollingBackground : MonoBehaviour
{
    private new Renderer renderer;
    private Vector2 moveOffset;
    private Vector2 originalTexOffset;

    [SerializeField]
    private float scrollSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        moveOffset = Vector2.zero;
        originalTexOffset = renderer.material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float offsetDelta = Mathf.Repeat((scrollSpeed * Time.deltaTime) + renderer.material.mainTextureOffset.y, 1f);
        moveOffset.Set(0f, offsetDelta);
        renderer.material.mainTextureOffset = moveOffset;
    }

    private void OnDestroy()
    {
        renderer.material.mainTextureOffset = originalTexOffset;
    }
}
