using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMgr
{
    #region enum
    public enum ParticleType
    {
        HitExplosion,
        Destruction,
        All
    }
    #endregion

    #region  private variables
    static ParticleMgr mInstance;
    List<ParticleObject> mHitExplosion;
    #endregion

    #region private methods
    ParticleMgr()
    {
        mHitExplosion = new List<ParticleObject>();
    }

    void PlayHitExplosion (Vector3 pos)
    {
        foreach (var item in mHitExplosion)
        {
            if (item.IsFinished())
            {
                item.PlayParticle(pos);
                break;
            }
        }
    }
    #endregion

    #region public methods
    public static ParticleMgr SInstance
    {
        get 
        {
            if (mInstance == null)
                mInstance = new ParticleMgr();
            return mInstance;
        }
    }

    public void InitParticle (GameObject obj, ParticleType part)
    {
        ParticleObject pa = obj.GetComponent<ParticleObject>();
        pa.InitParticleObject();
        mHitExplosion.Add(pa);
    }

    public void PlayParticle (ParticleType part, Vector3 pos)
    {
        switch (part)
        {
            case ParticleType.HitExplosion:
            {
                PlayHitExplosion(pos);
                break;
            }
        }
    }
    #endregion
}
