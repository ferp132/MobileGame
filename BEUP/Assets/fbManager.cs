using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Linq;

public class fbManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void Share()
    {
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(null, callback: onLogin);
        }
        else
        {
            FB.ShareLink(
                contentTitle: "Wecho Games", 
                contentURL:new System.Uri("https://www.facebook.com/louis.cresswell"), 
                contentDescription: "Like and Share my page",
                callback: onShare);
        }
    }

    private void onLogin(ILoginResult result)
    {
        if (result.Cancelled)
        {
            Debug.Log(" user cancelled login");
        }
        else
        {
            Share(); // call share() again
        }
    }

    private void onShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("sharelinkerror: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log("link shared");
      }
    }
}
