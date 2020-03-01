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
            // FB.ShareLink(new System.Uri("https://www.google.com/"),
            // "MineGrab",
            // "Your Score: " + GameController.m_Instance.GetScore());
            // https://script.google.com/macros/s/AKfycbyIsxIcGu1Lvp0KgCag0-QQGcbA4own68W3P7zyCvBrtq7SLY4/exec?BestScore=100&Score=10
            // FB.FeedShare(link: new System.Uri("https://www.google.com/"),
            //             linkName: "MineGrab Score: " + GameController.m_Instance.GetScore());
            // FB.FeedShare(link: new System.Uri(@"https://script.google.com/macros/s/AKfycbyIsxIcGu1Lvp0KgCag0-QQGcbA4own68W3P7zyCvBrtq7SLY4/exec
            //                                 BestScore=" + GameController.m_Instance.GetBestScore() + "&Score=" + GameController.m_Instance.GetScore()));
            FB.FeedShare(link: new System.Uri(@"https://www.facebook.com/Magic-Signs-Saga-106093860972945/"),
                        linkName: "Your Score: " + GameController.m_Instance.GetScore());
        }
    }

    public void ShareScoreWithScreenShot ()
    {
        var texture = ScreenCapture.CaptureScreenshotAsTexture();

        byte[] encodedScreenshot = texture.EncodeToPNG();
        var form = new WWWForm();
        form.AddField("message", "Write your Description");
        form.AddBinaryData("image", encodedScreenshot);
        FB.API("me/photo", HttpMethod.POST, ShareScoreWithScreenShotCallback, form);
    }

    void ShareScoreWithScreenShotCallback (IResult result)
    {

    }
}
