using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentChildren : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0;i < transform.childCount; ++i)
        {
            transform.GetChild(i).SetParent(null);
            --i;
        }

        Destroy(gameObject);
    }
}
