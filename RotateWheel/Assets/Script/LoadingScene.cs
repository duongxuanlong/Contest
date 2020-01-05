using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
