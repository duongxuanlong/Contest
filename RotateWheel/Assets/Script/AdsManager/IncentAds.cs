using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class IncentAds : MonoBehaviour, IUnityAdsListener
{
    #region Incentivize ads param
    const string PLACEMENT_ID = "rewardedVideo";
    bool mIsInit = false;
    #endregion
    // Start is called before the first frame update

    // Update is called once per frame

    #region public methods
    public void InitIncentAds ()
    {
        if (!mIsInit)
        {
            if (!Advertisement.isInitialized)
                return;

            mIsInit = true;
            Advertisement.AddListener(this);
        }
    }

    public void ShowIncentAds ()
    {
        Advertisement.Show(PLACEMENT_ID);
    }
    void  IUnityAdsListener.OnUnityAdsReady(string placementId)
    {
        if (placementId == PLACEMENT_ID)
            Debug.Log(placementId + " is ready");
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        if (placementId == PLACEMENT_ID)
            Debug.Log(placementId + " start");
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        Debug.Log("Uniy ads error: " + message);
    }

    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == PLACEMENT_ID)
        {
            // 0: fail 1: finish 2: skip
            Debug.Log("Ads finished: " + showResult);
        }
    }
    #endregion
}
