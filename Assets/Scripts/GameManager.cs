using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver = false;

    public void ToggleGameOver(bool toggle)
    {
        isGameOver = toggle;
    }

    // Update is called once per frame
    void Update()
    {
        // If player is destroyed
        if (isGameOver)
        {
            // Reload the level
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
