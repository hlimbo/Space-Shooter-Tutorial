using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver = false;

    private void Awake()
    {
        // Target FPS = targetFrameRate / vSyncCount
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
