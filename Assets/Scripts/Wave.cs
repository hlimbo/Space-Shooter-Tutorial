using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Template", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public new string name;
    public GameObject[] enemyPrefabs;
    public GameObject pathContainer;
}
