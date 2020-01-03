﻿using System.Collections;
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
    AdsState mAdsState;
    #endregion

    #region Incentivize ads params
    public const string INCENTIVIZE_PLACEMENT_ID = "rewardedVideo";
    #endregion

    #region enum
    public enum AdsState
    {
        None,
        Watch,
        Start,
        Error,
        Finish_Complete,
        Finish_Error
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (!mIsInit)
        {
            // #if UNITY_IOS || UNITY_ANDROID
            Advertisement.Initialize(GameID, IsTestMode);
            // #endif

            Advertisement.AddListener(this);
            DontDestroyOnLoad(gameObject);
            mIsInit = true;
        }
        this.mAdsState = AdsState.None;
    }

    #region public methods
    public void SetWatchAds ()
    {
        this.mAdsState = AdsState.Watch;
    }

    public AdsState GetAdsState ()
    {
        return this.mAdsState;
    }
    #endregion

    #region override unity ads listener interface
    void  IUnityAdsListener.OnUnityAdsReady(string placementId)
    {
        // Debug.Log("Unity ads ready: " + placementId);
        // if (placementId == INCENTIVIZE_PLACEMENT_ID)
        //     Debug.Log(placementId + " is ready");
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        // Debug.Log("Unity ads start: " + placementId);
        
        if (placementId == INCENTIVIZE_PLACEMENT_ID)
            this.mAdsState = AdsState.Start;
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        // Debug.Log("Uniy ads error: " + message);
        this.mAdsState = AdsState.Error;
    }

    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //  Debug.Log("Ads finished with placementID: " + placementId + " and result: " + showResult);
        if (placementId == INCENTIVIZE_PLACEMENT_ID)
        {
            // 0: fail 1: finish 2: skip
            // Debug.Log("Ads finished: " + showResult);
            switch (showResult)
            {
                case ShowResult.Failed:
                {
                    this.mAdsState = AdsState.Finish_Error;
                    break;
                }

                case ShowResult.Skipped:
                {
                    this.mAdsState = AdsState.Finish_Error;
                    break;
                }

                case ShowResult.Finished:
                {
                    this.mAdsState = AdsState.Finish_Complete;
                    break;
                }
            }
        }
    }
    #endregion
    
}
