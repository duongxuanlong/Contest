using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LoadingScene : MonoBehaviour
{
    #region  variables

    bool mIsLoadMain;
    // AsyncOperation mLoadMain;

    float mLoadingTime;
    const float LIMIT_LOADING_TIME = 1f;

    #endregion

    #region private methods
    IEnumerator LoadMainScene ()
    {
        var temp = SceneManager.LoadSceneAsync(Constant.SCENE_MAIN);

        temp.allowSceneActivation = false;

        while (!temp.isDone)
        {
            if (temp.progress >= 0.9f && mLoadingTime >= LIMIT_LOADING_TIME)
            {
                temp.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mIsLoadMain = false;
        // mLoadMain = null;
        mLoadingTime = 0;
        // Debug.Log("Loading Scene Start");
        if (Constant.TRACKING_IS_FIRST_LAUNCH)
        {
            Constant.TRACKING_IS_FIRST_LAUNCH = false;
            Analytics.CustomEvent(Constant.TRACKING_START_GAME, new Dictionary<string, object> 
            {
                { Constant.PARAM_SESSION_ID, AnalyticsSessionInfo.sessionId.ToString()}
            });
        }

        FBMgr.Instance.InitFB();
        // FireBaseMg
    }

    // Update is called once per frame
    void Update()
    {
        if (!mIsLoadMain)
        {
            mIsLoadMain = true;
            mLoadingTime = 0;
            StartCoroutine(LoadMainScene());
        }
        else
        {
            mLoadingTime += Time.deltaTime;
        }
    
    }
}
