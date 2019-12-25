using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    #region params
    Image mFlash;
    // Color mFlashColor;
    bool mActive; 
    float mInitialAlpha;
    float mTargetAlpha;
    float mPeriod;
    float mRunningTime;
    float mVelocity;
    int mTotalTimes;
    public int mTimes;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (mFlash == null)
        {
            mFlash = GetComponent<Image>();
            // mFlashColor = mFlash.color;
            gameObject.SetActive(false);
        }
        
        SetupInitialParams();
    }

    // Update is called once per frame
    void Update()
    {
        if (mActive)
        {
            Color col = CalculateAlpha();
            mFlash.color = col;

            bool expire = CalculateTimes();
            if (!expire)
                mRunningTime += Time.deltaTime;
        }
    }

    private void OnEnable() {
        ParticleMgr.SInstance.SetDamgeFlash(this);
    }

    #region private methods
    void SetupInitialParams ()
    {
        mActive = false;
        mInitialAlpha = 0.8f;
        mTargetAlpha = 0.2f;
        mPeriod = 0.2f;
        mTotalTimes = 3;

        SetupDamageFlash();
    }

    void SetupDamageFlash ()
    {
        mTimes = 0;
        SetupRunningTimeAndVelocity();
    }

    void SetupRunningTimeAndVelocity ()
    {
        mRunningTime = 0f;
        mVelocity = 0f;
    }

    void ActivateDamageFlash (bool isactive = true)
    {
        mActive = isactive;
        gameObject.SetActive(isactive);

        if (isactive)
            SetupDamageFlash();
    }

    Color CalculateAlpha ()
    {
        Color col = mFlash.color;
        float remaining = mPeriod - mRunningTime;
        float a = col.a;
        if (remaining > 0)
            a = Mathf.SmoothDamp(a, mTargetAlpha, ref mVelocity, remaining);
        else
            a = mTargetAlpha;
        col.a = a;
        return col;
    }

    bool CalculateTimes ()
    {
        bool expire = false;
        if (mRunningTime >= mPeriod)
        {
            ++mTimes;
            expire = true;

            if (mTimes >= mTotalTimes)
            {
                ActivateDamageFlash(false);
                // Debug.Log("Time: " + mTimes);
            }
            else
            {
                this.SetupRunningTimeAndVelocity();
            }
        }

        if (expire)
        {
            Color col = mFlash.color;
            col.a = 1;
            mFlash.color = col;
            expire = true;
        }
        return expire;
    }
    #endregion

    #region public methods
    public void PlayDamageFlash ()
    {
        if (!mActive)
        {
            ActivateDamageFlash();
        }
    }
    #endregion
}
