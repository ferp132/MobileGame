using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().Paused = false;
    }
}
