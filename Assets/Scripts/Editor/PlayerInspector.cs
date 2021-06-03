using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
    private Player player;

    private void OnSceneGUI()
    {
        player = target as Player;
        Handles.color = Color.red;
        Handles.DrawWireDisc(player.transform.position, player.transform.forward, player.AttractRadius);
    }
}
