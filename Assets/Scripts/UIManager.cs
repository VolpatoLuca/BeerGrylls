using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image sparedBeerImage;
    [SerializeField] private RectTransform sparedBeerTransform;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text startTimeText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text holeCreatedText;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject menuCanvas;

    [SerializeField] private Image playImage;
    [SerializeField] private Image creditsImage;
    [SerializeField] private Image resumeImage;
    [SerializeField] private Image newGameImage;
    [SerializeField] private Image startQuitImage;
    [SerializeField] private Image pauseQuitImage;
    [SerializeField] private Image gameOverQuitImage;

    private float minSparedBeerHeight = 0;
    private float maxSparedBeerHeight = 300;

    private void Start()
    {
        if (sparedBeerTransform != null)
        {
            sparedBeerTransform.sizeDelta = new Vector2(sparedBeerTransform.sizeDelta.x, minSparedBeerHeight);
        }
    }

    private void Update()
    {
        UpdateSparedBeer();
        UpdateScore();

        if (playImage != null)
        {
            playImage.fillAmount = GameManager.instance.playFillAmount;
        }
        if (startQuitImage != null)
        {
            startQuitImage.fillAmount = GameManager.instance.startQuitFillAmount;
        }
        if (pauseQuitImage != null)
        {
            pauseQuitImage.fillAmount = GameManager.instance.pauseQuitFillAmount;
        }
        if (creditsImage != null)
        {
            creditsImage.fillAmount = GameManager.instance.creditsFillAmount;
        }
        if (resumeImage != null)
        {
            resumeImage.fillAmount = GameManager.instance.resumeFillAmount;
        }
        if (startTimeText != null)
        {
            if (GameManager.instance.hasGameStarted)
            {
                startTimeText.gameObject.SetActive(true);
            }
            else
            {
                startTimeText.gameObject.SetActive(false);
            }
            if (GameManager.instance.remainedTimeToStart <= 0)
            {
                startTimeText.gameObject.SetActive(false);
            }
            int countdownTime = (int)GameManager.instance.remainedTimeToStart;
            countdownTime++;
            startTimeText.text = countdownTime.ToString();
        }
        if (creditsCanvas != null)
        {
            if (GameManager.instance.hasGameStarted)
            {
                creditsCanvas.SetActive(false);
            }
            else
            {
                creditsCanvas.SetActive(GameManager.instance.showCredits);

            }
        }
        if(holeCreatedText != null)
        {
            if (GameManager.instance.showHoleCreated)
            {
                holeCreatedText.gameObject.SetActive(true);
                if (GameManager.instance.holesCreated == 1)
                {
                    holeCreatedText.text = "There's a hole in the barrel! Fill the glasses and share the beer with everyone!";
                }
                else if(GameManager.instance.holesCreated == 2)
                {
                    holeCreatedText.text = "Watch out! there's another hole in the other barrel!";
                }
            }
            else
            {
                holeCreatedText.gameObject.SetActive(false);
            }
        }
        if (menuCanvas != null)
        {
            if (GameManager.instance.hasGameStarted)
            {
                if (!GameManager.instance.isPlaying)
                {
                    menuCanvas.SetActive(true);
                }
                else
                {
                    menuCanvas.SetActive(false);
                }
            }
            else
            {
                menuCanvas.SetActive(false);
            }
        }
        if (GameManager.instance.hasGameEnded)
        {
            if (endGameUI != null)
            {
                endGameUI.SetActive(true);
                gameOverText.text = "Game ended! Your score is: " + GameManager.instance.score;
                newGameImage.fillAmount = GameManager.instance.newGameFillAmount;
                gameOverQuitImage.fillAmount = GameManager.instance.gameOverQuitFillAmount;
            }
        }
        else
        {
            if (endGameUI != null)
            {
                endGameUI.SetActive(false);
            }
        }

        if (!GameManager.instance.hasGameStarted)
        {
            if (startGameUI != null)
            {
                startGameUI.SetActive(true);
            }
        }
        else
        {
            if (startGameUI != null)
            {
                startGameUI.SetActive(false);
            }
        }
    }

    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + GameManager.instance.score;
        }
    }

    private void UpdateSparedBeer()
    {
        if (sparedBeerImage == null || sparedBeerTransform == null)
        {
            return;
        }
        float newHeight = Mathf.Lerp(minSparedBeerHeight, maxSparedBeerHeight, GameManager.instance.sparedBeer / GameManager.instance.maxSparedBeer);
        sparedBeerTransform.sizeDelta = new Vector2(sparedBeerTransform.sizeDelta.x, newHeight);
    }
}