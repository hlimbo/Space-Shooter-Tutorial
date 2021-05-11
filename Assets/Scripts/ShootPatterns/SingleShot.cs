using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private float speed;

    [ContextMenu("Fire Single Shot Test")]
    public void FireShot()
    {
        Quaternion up = Quaternion.LookRotation(transform.forward, transform.up);
        Instantiate(laser, transform.position, up, null);
    }
}
