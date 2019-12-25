using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMgr
{
    #region enum
    public enum ParticleType
    {
        HitExplosion,
        HitExplosionDam,
        Destruction,
        All
    }
    #endregion

    #region delegate - events
    public delegate void BroadCastCacllback ();
    public static event BroadCastCacllback EvtDamageFlash;
    #endregion

    #region  private variables
    static ParticleMgr mInstance;
    List<ParticleObject> mHitExplosion;
    List<ParticleObject> mHitExplosionDam;
    DamageFlash mDamageFlash;
    #endregion

    #region private methods
    ParticleMgr()
    {
        mHitExplosion = new List<ParticleObject>();
        mHitExplosionDam = new List<ParticleObject>();
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

    void PlayHitExplosionDam (Vector3 pos) 
    {
        foreach(var item in mHitExplosionDam)
        {
            if (item.IsFinished())
            {
                item.PlayParticle(pos);
                break;
            }
        }
    }

    void PlayParticle (ParticleType part, Vector3 pos)
    {
        switch (part)
        {
            case ParticleType.HitExplosion:
            {
                PlayHitExplosion(pos);
                break;
            }

            case ParticleType.HitExplosionDam:
            {
                PlayHitExplosionDam(pos);
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

    public void SetDamgeFlash (DamageFlash flash)
    {
        if (this.mDamageFlash == null)
            this.mDamageFlash = flash;
    }

    public void InitParticle (GameObject obj, ParticleType part)
    {
        switch (part)
        {
            case ParticleType.HitExplosion:
            {
                ParticleObject pa = obj.GetComponent<ParticleObject>();
                pa.InitParticleObject();
                mHitExplosion.Add(pa);
                break;
            }

            case ParticleType.HitExplosionDam:
            {
                ParticleObject pa = obj.GetComponent<ParticleObject>();
                pa.InitParticleObject();
                break;
            }
        }
    }

    public void PlayParticle(PlayerController.BallType ball, Vector3 pos)
    {
        switch (ball)
        {
            case PlayerController.BallType.Heal:
            {
                PlayParticle(ParticleType.HitExplosion, pos);
                break;
            }

            case PlayerController.BallType.Damage:
            {
                PlayParticle(ParticleType.HitExplosionDam, pos);

                this.mDamageFlash.PlayDamageFlash();
                break;
            }
        }
    }

    #endregion
}
