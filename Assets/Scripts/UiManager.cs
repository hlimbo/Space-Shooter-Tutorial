using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text livesText;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartText;
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private Image thrustFillImg;
    [SerializeField]
    private Text waveText;
    [SerializeField]
    private Text winText;

    private GameManager gameManager;
    private Coroutine flickerRef;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives < 0)
        {
            return;
        }

        livesText.text = $"x{currentLives}";
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
                flickerRef = StartCoroutine(FlickerText(gameOverText));
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

    private IEnumerator FlickerText(Text text)
    {
        while (true)
        {
            text.enabled = true;
            yield return new WaitForSeconds(0.5f);
            text.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateAmmoText(int ammoCount)
    {
        ammoText.text = $"x{ammoCount}";
    }

    public void SetThrustFill(float fillAmount)
    {
        thrustFillImg.fillAmount = fillAmount;
    }

    public void SetWaveText(string newText)
    {
        if(waveText != null)
        {
            waveText.text = $"Starting {newText}";
        }
    }

    public void ToggleWaveTextVisibility(bool toggle)
    {
        if(waveText != null)
        {
            waveText.enabled = toggle;
        }
    }

    public void DisplayWinnerText(bool toggle)
    {
        restartText.gameObject.SetActive(toggle);
        winText.gameObject.SetActive(toggle);
        gameManager?.ToggleGameOver(toggle);

        if (toggle)
        {
            if (flickerRef != null)
            {
                StopCoroutine(flickerRef);
            }
            flickerRef = StartCoroutine(FlickerText(winText));
        }
        else
        {
            if (flickerRef != null)
            {
                StopCoroutine(flickerRef);
                flickerRef = null;
            }
        }
    }
}
