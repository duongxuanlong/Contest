using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
using System.Collections;

public class FBMgr
{
    // private void Awake() {
    //     InitFacebook();
    // }

    // void InitFacebook ()
    // {
    //     if (!FB.IsInitialized)
    //     {
    //         FB.Init( () => {
    //             if (FB.IsInitialized)
    //                 FB.ActivateApp();
    //             else
    //                 Debug.Log("Fail to Init Facebook app");
    //         }, 
    //         (isGameShow) => {
    //             if (!isGameShow)
    //                 Time.timeScale = 0;
    //             else
    //                 Time.timeScale = 1;
    //         });
    //     }
    // }

    // public void ShareScore()
    // {
    //     if (FB.IsInitialized)
    //     {
    //         // FB.ShareLink(new System.Uri("https://www.google.com/"),
    //         // "MineGrab",
    //         // "Your Score: " + GameController.m_Instance.GetScore());
    //         // https://script.google.com/macros/s/AKfycbyIsxIcGu1Lvp0KgCag0-QQGcbA4own68W3P7zyCvBrtq7SLY4/exec?BestScore=100&Score=10
    //         // FB.FeedShare(link: new System.Uri("https://www.google.com/"),
    //         //             linkName: "MineGrab Score: " + GameController.m_Instance.GetScore());
    //         // FB.FeedShare(link: new System.Uri(@"https://script.google.com/macros/s/AKfycbyIsxIcGu1Lvp0KgCag0-QQGcbA4own68W3P7zyCvBrtq7SLY4/exec
    //         //                                 BestScore=" + GameController.m_Instance.GetBestScore() + "&Score=" + GameController.m_Instance.GetScore()));
    //         FB.FeedShare(link: new System.Uri(@"https://www.facebook.com/Magic-Signs-Saga-106093860972945/"),
    //                     linkName: "Your Score: " + GameController.m_Instance.GetScore());
    //     }
    // }

    // public void ShareScoreWithScreenShot ()
    // {
    //     var texture = ScreenCapture.CaptureScreenshotAsTexture();

    //     byte[] encodedScreenshot = texture.EncodeToPNG();
    //     var form = new WWWForm();
    //     form.AddField("message", "Write your Description");
    //     form.AddBinaryData("image", encodedScreenshot);
    //     FB.API("me/photo", HttpMethod.POST, ShareScoreWithScreenShotCallback, form);
    // }

    // void ShareScoreWithScreenShotCallback (IResult result)
    // {

    // }

    #region fields
    static FBMgr m_Instance;
    AccessToken m_Token;

    static string[] FB_PERMISSION = {
        "public_profile",
        "email"
    };
    #endregion

    #region private methods
    FBMgr()
    {

    }

    void LoginResult (ILoginResult result)
    {
        if (FB.IsLoggedIn) {
        // AccessToken class will have session details
        // var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        this.m_Token = Facebook.Unity.AccessToken.CurrentAccessToken;

        // Print current access token's User ID
        // Debug.Log(aToken.UserId);

        // Print current access token's granted permissions
        // foreach (string perm in aToken.Permissions) {
        //     Debug.Log(perm);
        // }
        } else {
            Debug.Log("User cancelled login");
        }
    }
    #endregion

    #region  public methods
    public static FBMgr Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FBMgr();
            return m_Instance;
        }
    }

    public AccessToken GetToken ()
    {
        return this.m_Token;
    }
    
    public void ShareFB ()
    {
        FB.FeedShare(link: new System.Uri(@"https://www.facebook.com/Magic-Signs-Saga-106093860972945/"),
                        linkName: "Your Score: " + GameController.m_Instance.GetScore());
    }

    public void LoginFB ()
    {
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(FB_PERMISSION, this.LoginResult);
        }
    }

    public bool IsLogin()
    {
        return FB.IsLoggedIn;
    }

    public bool IsFBInit ()
    {
        return FB.IsInitialized;
    }

    public void InitFB ()
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
    #endregion
}
