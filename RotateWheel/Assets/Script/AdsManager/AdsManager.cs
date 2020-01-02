using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    #region Unity ads params
    #if UNITY_IOS
        string GameID = "3417710";
    #elif UNITY_ANDROID
        string GameID = "3417711";
    #else 
        string GameID = "1234567";
    #endif
    bool IsTestMode = true;
    bool mIsInit = false;
    #endregion

    #region Incentivize ads params
    // [SerializeField]
    // IncentAds mIncentAds;
    const string INCENTIVIZE_PLACEMENT_ID = "rewardedVideo";
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (!mIsInit)
        {
            Advertisement.Initialize(GameID, IsTestMode);
            Advertisement.AddListener(this);
            DontDestroyOnLoad(gameObject);
            mIsInit = true;

            // if (mIncentAds != null)
            // {
            //     mIncentAds.InitIncentAds();
            // }
        }
    }

    #region public methods
    public void ShowRewardVideo ()
    {
        Advertisement.Show(INCENTIVIZE_PLACEMENT_ID);
    }
    #endregion

    #region override unity ads listener interface
    void  IUnityAdsListener.OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity ads ready: " + placementId);
        if (placementId == INCENTIVIZE_PLACEMENT_ID)
            Debug.Log(placementId + " is ready");
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Unity ads start: " + placementId);
        if (placementId == INCENTIVIZE_PLACEMENT_ID)
            Debug.Log(placementId + " start");
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        Debug.Log("Uniy ads error: " + message);
    }

    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == INCENTIVIZE_PLACEMENT_ID)
        {
            // 0: fail 1: finish 2: skip
            Debug.Log("Ads finished: " + showResult);
        }
    }
    #endregion
    
}
