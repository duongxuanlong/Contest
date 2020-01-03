using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdsSceneCtrl : MonoBehaviour
{
    #region reference
    [SerializeField]
    GameObject mReviveObject;

    [SerializeField]
    SpriteRenderer mCountDown;
    [SerializeField]
    Button mBtnRevive;
    Image mBtnImage;

    [SerializeField]
    Sprite[] mNumbers;
    
    #endregion

    #region params
    AdsManager mAdsManager;
    const float COUNT_DOWN_TIME = 6f;
    const float COUNT_DOWN_TIME_LIMIT = 5f;
    const int START_INDEX = 5;
    float mRunningTime;
    int mIndex = 5;
    bool mIsCountDown = false;
    // bool mIsWatchAds = false;
    Color mPlayColor;
    Color mUnPlayColor;
    #endregion

    private void Start() {
        
        if (mAdsManager == null)
            mAdsManager = GetComponent<AdsManager>();

        if (mReviveObject != null)
            mReviveObject.SetActive(false);

        if (mBtnRevive != null)
        {
            if (mBtnImage == null)
                mBtnImage = mBtnRevive.GetComponent<Image> ();
        }

        if (mPlayColor == Color.clear)
            mPlayColor = new Color(1, 1, 1, 1);
        if (mUnPlayColor == Color.clear)
            mUnPlayColor = new Color(0.177f, 0.177f, 0.177f, 1);
        
        // mIsWatchAds = false;

        ResetCountDown();
    }
    
    void ResetCountDown ()
    {
        mRunningTime = 0;
        mIndex = START_INDEX;
        mCountDown.sprite = mNumbers[mIndex];
    }

    void SetSprite ()
    {
        int temp = (int)mRunningTime;
        temp = START_INDEX - temp;
        if (temp >= 0 && temp != mIndex)
        {
            mIndex = temp;  
            mCountDown.sprite = mNumbers[mIndex];
        }
    }

    void CheckTime ()
    {
        if (mRunningTime >= COUNT_DOWN_TIME_LIMIT)
        {
            mBtnImage.color = mUnPlayColor;
        }
        
        if (mRunningTime >= COUNT_DOWN_TIME)
        {
            mIsCountDown = false;

            if (mAdsManager.GetAdsState() == AdsManager.AdsState.None)
            {
                mReviveObject.SetActive(false);
                SceneManager.LoadScene(Constant.SCENE_END);
            }
        }
            
    }

    void CheckAdsState ()
    {
        if (mAdsManager.GetAdsState() > AdsManager.AdsState.None)
        {
            if (mAdsManager.GetAdsState() == AdsManager.AdsState.Finish_Complete)
            {
                mReviveObject.SetActive(false);
                SceneManager.LoadScene(Constant.SCENE_LOADING);
            }
            else if ((mAdsManager.GetAdsState() == AdsManager.AdsState.Finish_Error ||
                      mAdsManager.GetAdsState() == AdsManager.AdsState.Error))
                      {
                          mReviveObject.SetActive(false);
                          SceneManager.LoadScene(Constant.SCENE_END);
                      }
        }
    }

    private void Update() {
        if (mIsCountDown)
        {
            SetSprite();

            CheckTime();

            mRunningTime+= Time.deltaTime;
        }

        CheckAdsState();
    }

    void EnableIncentAds (bool isActive)
    {
        // if (!mIsWatchAds)
        // if (mAdsManager.GetAdsState() == AdsManager.AdsState.None)
        if (!mIsCountDown)
        {
            mIsCountDown = isActive;

            if (mIsCountDown)
            {
                // mIsWatchAds = true;
                // mAdsManager.SetWatchAds();
                mReviveObject.SetActive(true);
                mBtnImage.color = mPlayColor;
                ResetCountDown();
            }
        }
    }

    private void OnEnable() {
        EventManager.EnableIncentAds += EnableIncentAds;
    }

    private void OnDisable() {
        EventManager.EnableIncentAds -= EnableIncentAds;
    }

    public void ShowRewardVideo ()
    {
        mAdsManager.SetWatchAds();
        Advertisement.Show(AdsManager.INCENTIVIZE_PLACEMENT_ID);
    }

}
