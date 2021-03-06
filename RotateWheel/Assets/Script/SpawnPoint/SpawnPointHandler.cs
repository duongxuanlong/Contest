﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHandler : MonoBehaviour {

	#region reference
	public GameObject[] m_SpawnLevels;
	private List<GameObject> m_InstanceLevels;
	#endregion

	#region param
	public float m_TimeForSpawn;
	//public int m_TotalSpawnPoints;
	//public float m_Radius;
	//public float m_Probability;
	int m_TotalLevels;
	private int m_CurrentLevel;
	#endregion

	#region Easy Pools
	public int m_EasyPools;
	private int m_RunningEasyPools;
	private int m_TotalEasyPools;
	#endregion

	#region Medium Pools
	public int m_MediumPools;
	private int m_RunningMediumPools;
	private int m_TotalMediumPools;
	#endregion

	#region Hyper dam
	private int m_CurrentHyperDam;
	public int m_MaxHyperDam;
	#endregion

	#region probability
	public AnimationCurve m_Curve;
	#endregion

	#region Special Balls
	public int m_LelBreak;
	private int m_LelRunning;
	private bool m_FirstTime;
	#endregion

	//List<GameObject> m_SpawnPoints;
	float m_RunningTime;
	bool m_CanRun;
	bool m_CanSpawn;

	void Awake()
	{
		m_CanSpawn = true;
		if (m_InstanceLevels == null)
			m_InstanceLevels = new List<GameObject> ();
//		if (m_SpawnPoints == null)
//			m_SpawnPoints = new List<GameObject> ();
		m_TotalLevels = m_SpawnLevels.Length;
		for (int i = 0; i < m_TotalLevels; i++) {
			GameObject level = Instantiate (m_SpawnLevels[i], m_SpawnLevels[i].transform.position , Quaternion.identity) as GameObject;
			m_InstanceLevels.Add (level);
		}
//		GameObject currentlevel = m_InstanceLevels[m_CurrentLevel];
//		foreach (Transform child in currentlevel.transform)
//			m_SpawnPoints.Add (child.gameObject);
		//m_TotalSpawnPoints = m_SpawnPoints.Count;

		if (m_TimeForSpawn == 0)
			m_TimeForSpawn = 0.2f;

		m_CurrentLevel = -1;

//		if (m_Radius == 0)
//			m_Radius = 2.5f;

//		if (m_Probability == 0)
//			m_Probability = 1f / m_SpawnPoints.Count;

		m_RunningTime = m_TimeForSpawn;
		m_CanRun = true;

		if (m_EasyPools == 0)
			m_EasyPools = 3;
		m_RunningEasyPools = 0;
		m_TotalEasyPools = 3;

		if (m_MediumPools == 0)
			m_MediumPools = 5;
		m_RunningMediumPools = 0;
		m_TotalMediumPools = 8;

		if (m_MaxHyperDam == 0)
			m_MaxHyperDam = 2;
		m_CurrentHyperDam = 0;

		if (m_LelBreak == 0)
			m_LelBreak = 4;
		m_LelRunning = 0;

//		for (int i = 0; i < m_TotalSpawnPoints; i++) {
//			Vector2 pos = Vector2.zero;
//			while (pos == Vector2.zero) {
//				pos = Random.insideUnitCircle * m_Radius;
//			}
//
//			GameObject obj = (GameObject)Instantiate (m_SpawnPoint,new Vector3 (pos.x, pos.y, 0), Quaternion.identity) as GameObject;
//			m_SpawnPoints.Add (obj);
//		}
	}

	void OnEnable()
	{
		EventManager.CanRunCallback += CanRun;
		EventManager.IncreaaseDiffCallback += OnDifferentLevel;
		EventManager.CanGenerateHyperDamCallback += CanGenerateHyperDam;
		EventManager.UpdateHyperDamCallback += UpdateHyperDam;

		m_FirstTime = true;
	}

	void OnDisable()
	{
		EventManager.CanRunCallback -= CanRun;
		EventManager.IncreaaseDiffCallback -= OnDifferentLevel;
		EventManager.CanGenerateHyperDamCallback -= CanGenerateHyperDam;
		EventManager.UpdateHyperDamCallback -= UpdateHyperDam;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;	
	}

	public void Reset()
	{
		//m_SpawnPoints.Clear ();
	}

	bool CanGenerateHyperDam()
	{
		return m_CurrentHyperDam < m_MaxHyperDam;
	}

	void UpdateHyperDam()
	{
		m_CurrentHyperDam++;
	}

	void OnDifferentLevel()
	{
		//m_Probability = 0.7f;
		//m_TimeForSpawn = 0.1f;
	}

	IEnumerator SpawnObject()
	{
		m_CanSpawn = false;
		int newlevel = 0;
		m_LelRunning++;
		if (m_RunningEasyPools < m_EasyPools) {
			EventManager.StartGenerateAllGreen ();
			newlevel = Random.Range (0, m_TotalEasyPools);
			m_RunningEasyPools++;
			//Debug.Log (newlevel);
		} else if (m_RunningMediumPools < m_MediumPools) {
			EventManager.StopGenerateAllGcreen ();
			EventManager.ResetRedBallCount ();
			EventManager.StartMediumDifficulty ();
			newlevel = Random.Range (0, m_TotalEasyPools + m_TotalMediumPools);
			m_RunningMediumPools++;
			//Debug.Log (newlevel);
		} else if (m_RunningMediumPools < (m_MediumPools +10)) {
			EventManager.StartHardDifficulty ();
			newlevel = Random.Range (3, m_TotalLevels);
			m_RunningMediumPools++;
			//Debug.Log (newlevel);
		} else {
			EventManager.StartHardDifficulty ();
			EventManager.StartVeryHardDifficulty ();
			newlevel = Random.Range (5, m_TotalLevels);
		}

		m_CurrentLevel = newlevel;
//		if (m_CurrentLevel == -1)
//			m_CurrentLevel = newlevel;
//		else if (m_TotalLevels > 1){
//			while (newlevel == m_CurrentLevel)
//				newlevel = Random.Range (0, m_TotalLevels);
//		}
			
		m_CurrentHyperDam = 0;
		m_CurrentLevel = newlevel;
		GameObject level = m_InstanceLevels [m_CurrentLevel];
		foreach (Transform child in level.transform) {
			//float needspawn = m_Curve.Evaluate (Random.value);
			//if (needspawn <= m_Probability * i) {
			GameObject obj = child.gameObject;
			SpawnPointController controller = (SpawnPointController)obj.GetComponent<SpawnPointController> ();
			if (controller != null)
				controller.GeneratePoint ();
			yield return new WaitForSeconds (0f);
			//i++;
			//return;
		//}
		}

		//Check for should generate special balls
		if (m_FirstTime) {
			if (m_LelRunning >= m_LelBreak + 2) {
				EventManager.GenerateSpecialBall ();
				m_LelRunning = 0;
				m_FirstTime = false;
			}
		} else {
			if (m_LelRunning >= m_LelBreak) {
				EventManager.GenerateSpecialBall ();
				m_LelRunning = 0;
			}
		}

		m_CanSpawn = true;

		//if (m_LevRunning >= m_LelBreak)
			

//		if (EventManager.ShouldGenerateAllGreen ())
//			EventManager.StopGenerateAllGcreen ();

//		SpawnPointController ctrl = (SpawnPointController)m_SpawnPoints[m_SpawnPoints.Count - 1].GetComponent<SpawnPointController> ();
//		if (ctrl != null)
//			ctrl.GeneratePoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.m_Instance == null ||
			!GameController.m_Instance.IsReady() ||
			GameController.m_Instance.GetTutorialPhase() == Constant.TUTORIAL_PHASE_0)
			return;
		
		if (m_CanRun) {
			if (m_RunningTime >= m_TimeForSpawn) {
				if (m_CanSpawn) {
					StartCoroutine(SpawnObject ());
					m_RunningTime = 0f;
				}
			} else {
				m_RunningTime += Time.deltaTime;
			}
		}
		//Debug.Log ("Running Time: " + m_RunningTime);
	}
}
