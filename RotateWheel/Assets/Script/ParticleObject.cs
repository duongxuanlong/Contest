using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    #region vars
    ParticleSystem mSystem;
    bool mIsUsed;
    #endregion

    #region  public methods
    public void InitParticleObject ()
    {
        mSystem = GetComponent<ParticleSystem>();
        mIsUsed = false;
    }

    public void SetUsed (bool used)
    {
        mIsUsed = used;
    }

    public bool IsUsed ()
    {
        return mIsUsed;
    }

    public void PlayParticle (Vector3 pos)
    {
        if (!mSystem.IsAlive(true))
        {
            transform.position = pos;
            mSystem.Play();
        }
    }

    public bool IsFinished ()
    {
        return mSystem.isStopped;
    }
    #endregion
}
