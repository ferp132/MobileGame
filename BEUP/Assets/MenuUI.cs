using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TimesPlayedLabel;
    int timesPlayedValue = 0;

    AudioSource audio = null;

    public void PlayUISound()
    {
        if (!audio.isPlaying) audio.Play();
    }




    public void openTwitter()
    {
        string twitterAddress = "http://twitter.com/intent/tweet";
        string message = "BEAT EM UP IN THIS GAAAME";
        string descriptionParameter = "BEUP";
        string appStoreLink = "https://play.google.com/store/apps/details?id=com.growlgamesstudio.pizZapMania";
        Application.OpenURL(twitterAddress + "?text=" + WWW.EscapeURL(message + "n" + descriptionParameter + "n" +
        appStoreLink));
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        timesPlayedValue = PlayerPrefs.GetInt("Times Played");


        TimesPlayedLabel.text = "Times Played: " + timesPlayedValue;
    }
}
