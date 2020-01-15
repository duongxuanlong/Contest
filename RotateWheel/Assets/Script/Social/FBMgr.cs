using UnityEngine;
using Facebook.Unity;

public class FBMgr : MonoBehaviour
{
    private void Awake() {
        InitFacebook();
    }

    void InitFacebook ()
    {
        if (!FB.IsInitialized)
        {
            FB.Init( () => {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Fail to Init Facebook app");
            }, 
            (isGameShow) => {
                if (!isGameShow)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
    }

    public void ShareScore()
    {
        if (FB.IsInitialized)
        {
            FB.ShareLink(new System.Uri("https://developers.facebook.com/"),
            "MineGrab",
            "Your Score: " + GameController.m_Instance.GetScore());
        }
    }
}
