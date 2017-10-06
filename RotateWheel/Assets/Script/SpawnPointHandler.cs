using System.Collections;
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
	public float m_Radius;
	public float m_Probability;
	public int m_CurrentLevel;
	#endregion

	#region probability
	public AnimationCurve m_Curve;
	#endregion

	List<GameObject> m_SpawnPoints;
	float m_RunningTime;
	bool m_CanRun;

	void Awake()
	{
		if (m_InstanceLevels == null)
			m_InstanceLevels = new List<GameObject> ();
		if (m_SpawnPoints == null)
			m_SpawnPoints = new List<GameObject> ();
		int totallevels = m_SpawnLevels.Length;
		for (int i = 0; i < totallevels; i++) {
			GameObject level = Instantiate (m_SpawnLevels[i], m_SpawnLevels[i].transform.position , Quaternion.identity) as GameObject;
			m_InstanceLevels.Add (level);
		}
		GameObject currentlevel = m_InstanceLevels[m_CurrentLevel];
		foreach (Transform child in currentlevel.transform)
			m_SpawnPoints.Add (child.gameObject);
		//m_TotalSpawnPoints = m_SpawnPoints.Count;

		if (m_TimeForSpawn == 0)
			m_TimeForSpawn = 0.2f;

		if (m_Radius == 0)
			m_Radius = 2.5f;

		if (m_Probability == 0)
			m_Probability = 1f / m_SpawnPoints.Count;

		m_RunningTime = m_TimeForSpawn;
		m_CanRun = true;

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
	}

	void OnDisable()
	{
		EventManager.CanRunCallback -= CanRun;
		EventManager.IncreaaseDiffCallback -= OnDifferentLevel;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;	
	}

	public void Reset()
	{
		//m_SpawnPoints.Clear ();
	}

	void OnDifferentLevel()
	{
		//m_Probability = 0.7f;
		//m_TimeForSpawn = 0.1f;
	}

	void SpawnObject()
	{
		int i = 1;
		foreach (GameObject obj in m_SpawnPoints) {
			float needspawn = m_Curve.Evaluate (Random.value);
			//if (needspawn <= m_Probability * i) {
				SpawnPointController controller = (SpawnPointController)obj.GetComponent<SpawnPointController> ();
				if (controller != null)
					controller.GeneratePoint ();
				i++;
				//return;
			//}
		}

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
				SpawnObject ();
				m_RunningTime = 0f;
			} else {
				m_RunningTime += Time.deltaTime;
			}
		}
		//Debug.Log ("Running Time: " + m_RunningTime);
	}
}
