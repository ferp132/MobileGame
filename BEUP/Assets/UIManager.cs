using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager manager = null;
    TextMeshProUGUI livesLabel = null;
    TextMeshProUGUI scoreLabel = null;
    AudioSource audio = null;

    [SerializeField] Canvas pauseMenuCanvas;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayUISound()
    {
        if (!audio.isPlaying) audio.Play();
    }


    public void Init(GameManager manager)
    {
        this.manager = manager;

        livesLabel = transform.Find("Lives Label").GetComponent<TextMeshProUGUI>();
        scoreLabel = transform.Find("Score Label").GetComponent<TextMeshProUGUI>();

        UpdateUI();
    }

    public void UpdateUI()
    {
        livesLabel.text = "Lives: " + manager.Lives;
        scoreLabel.text = "Score: " + manager.Score;
    }

    public void PauseGame()
    {
        pauseMenuCanvas.gameObject.SetActive(true);
        FindObjectOfType<GameManager>().Paused = true;
    }
}
