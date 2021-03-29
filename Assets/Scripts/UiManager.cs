using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Sprite[] liveSprites;
    [SerializeField]
    private Image livesImage;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartText;
    private GameManager gameManager;

    private Coroutine flickerRef;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }

    public void UpdateLives(int currentLives)
    {
        livesImage.sprite = liveSprites[currentLives];
    }

    public void ToggleGameOver(bool toggle)
    {
        gameOverText.gameObject.SetActive(toggle);
        restartText.gameObject.SetActive(toggle);
        gameManager?.ToggleGameOver(toggle);
        if(toggle)
        {
            if(flickerRef == null)
            {
                flickerRef = StartCoroutine(FlickerGameOverText());
            }
        }
        else
        {
            if(flickerRef != null)
            {
                StopCoroutine(flickerRef);
            }
        }
    }

    private IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
