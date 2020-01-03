using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

public class GameController : MonoBehaviour {

	private int m_TutorialPhase;
	private float m_BestScore;
	private float m_Score;

	private bool m_IsReady = false;

	public static GameController m_Instance;

	#region particle prefabs
	// public GameObject PreHitExplosion;
	// public GameObject PreHitExplosionDam;
	public GameObject[] PreExplosion;

	#endregion


	public bool IsReady ()
	{
		return m_IsReady;
	}

	public int GetTutorialPhase()
	{
		return m_TutorialPhase;
	}

	public void SetTutorialPhase()
	{
		if (m_TutorialPhase == Constant.TUTORIAL_PHASE_0)
			m_TutorialPhase++;
		//m_TutorialPhase = phase;
	}

	public float GetBestScore()
	{
		return m_BestScore;
	}

	public void SetBestScore(float score)
	{
		m_BestScore = score;
	}

	public float GetScore()
	{
		return m_Score;
	}

	public void SetScore(float score)
	{
		m_Score = score;
	}

	void Awake()
	{
		if (m_Instance == null) {
			m_Instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (m_Instance != this){
			Destroy (gameObject);
		}
	}

	private void Start() {
		
		StartCoroutine(InitHitExplosion());
	}

	IEnumerator InitHitExplosion()
	{
		for (ParticleMgr.ParticleType temp = ParticleMgr.ParticleType.HitExplosion;
			temp <= ParticleMgr.ParticleType.HitExplosionDam; ++temp)
			{
				InitHitExplosion(PreExplosion[(int)temp], temp);
				yield return null;
			}
	}

	void InitHitExplosion (GameObject pre, ParticleMgr.ParticleType part)
	{
		for (int i = 0; i < 20; ++i)
		{
			GameObject obj = Instantiate(pre);
			obj.transform.SetParent(transform);
			ParticleMgr.SInstance.InitParticle(obj, part);
		}
	}

	void OnEnable()
	{
		EventManager.ModifyPhaseCallback += SetTutorialPhase;
		EventManager.ScoreBestCallback += SetBestScore;

		BinaryFormatter bif = new BinaryFormatter ();
		string path = Application.persistentDataPath + Constant.SAVE_GAME;
		Debug.Log ("Path: " + path);
		if (File.Exists(path))
		{
			FileStream f = File.Open (Application.persistentDataPath + Constant.SAVE_GAME, FileMode.Open);
			GameInfo info = new GameInfo ();
			info = (GameInfo)bif.Deserialize (f);
			f.Close ();

			m_TutorialPhase = info.TutorialPhase;
			//m_TutorialPhase = Constant.TUTORIAL_PHASE_0;
			m_BestScore = info.BestScore;
			m_Score = info.Score;
		} else {
			m_TutorialPhase = Constant.TUTORIAL_PHASE_0;
			m_BestScore = 0f;
		}

//		if (m_TutorialPhase == Constant.TUTORIAL_PHASE_0)
//			EventManager.GenerateGreenBalls ();
		
		m_IsReady = true;
	}

	void OnDisable()
	{
		EventManager.ModifyPhaseCallback -= SetTutorialPhase;
		EventManager.ScoreBestCallback -= SetBestScore;

		BinaryFormatter bif = new BinaryFormatter ();
		string path = Application.persistentDataPath + Constant.SAVE_GAME;
		FileStream f = File.Open (path, FileMode.OpenOrCreate);

		GameInfo info = new GameInfo ();
		info.TutorialPhase = m_TutorialPhase;
		info.BestScore = m_BestScore;
		if (Constant.WATCH_ADS_COUNT > 0)
			info.Score = m_Score;
		else 
			info.Score = 0;
		bif.Serialize (f, info);
		f.Close ();
	}

	private void OnApplicationPause(bool pauseStatus) {
		EventManager.CanRun(!pauseStatus);
	}

}

[Serializable]
class GameInfo
{
	public int TutorialPhase;
	public float BestScore;
	public float Score;
}
