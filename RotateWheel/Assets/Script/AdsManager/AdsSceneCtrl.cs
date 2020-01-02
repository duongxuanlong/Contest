using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    bool mdone = false;
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
            mReviveObject.SetActive(false);
            SceneManager.LoadScene(Constant.SCENE_END);
        }
            
    }

    private void Update() {
        if (mIsCountDown)
        {
            SetSprite();

            CheckTime();

            mRunningTime+= Time.deltaTime;
        }
    }

    void EnableIncentAds (bool isActive)
    {
        mIsCountDown = isActive;

        if (mIsCountDown)
            ResetCountDown();
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    // public void EnableAdsButton ()

}
