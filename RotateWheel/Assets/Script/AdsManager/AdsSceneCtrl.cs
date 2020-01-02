using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsSceneCtrl : MonoBehaviour
{
    #region reference
    [SerializeField]
    GameObject mReviveObject;
    #endregion

    #region params
    AdsManager mAdsManager;
    #endregion

    private void Start() {
        if (mAdsManager == null)
            mAdsManager = GetComponent<AdsManager>();
    }

    

}
