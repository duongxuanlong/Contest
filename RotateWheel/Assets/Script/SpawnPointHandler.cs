using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHandler : MonoBehaviour {

	#region reference
	public GameObject m_SpawnPoint;
	#endregion

	#region param
	public float m_TimeForSpawn;
	public int m_TotalSpawnPoints;
	public float m_Radius;
	public float m_Probability;
	#endregion

	#region probability
	public AnimationCurve m_Curve;
	#endregion

	List<GameObject> m_SpawnPoints;
	float m_RunningTime;

	void Awake()
	{
		if (m_SpawnPoints == null)
			m_SpawnPoints = new List<GameObject> ();

		if (m_TimeForSpawn == 0)
			m_TimeForSpawn = 0.5f;

		if (m_TotalSpawnPoints == 0)
			m_TotalSpawnPoints = 7;

		if (m_Radius == 0)
			m_Radius = 2.5f;

		if (m_Probability == 0)
			m_Probability = 0.5f;

		m_RunningTime = 0f;

		for (int i = 0; i < m_TotalSpawnPoints; i++) {
			Vector2 pos = Vector2.zero;
			while (pos == Vector2.zero) {
				pos = Random.insideUnitCircle * m_Radius;
			}

			GameObject obj = (GameObject)Instantiate (m_SpawnPoint,new Vector3 (pos.x, pos.y, 0), Quaternion.identity) as GameObject;
			m_SpawnPoints.Add (obj);
		}
	}

	public void Reset()
	{
		m_SpawnPoints.Clear ();
	}

	void SpawnObject()
	{
		foreach (GameObject obj in m_SpawnPoints) {
			float needspawn = m_Curve.Evaluate (Random.value);
			if (needspawn <= m_Probability) {
				SpawnPointController controller = (SpawnPointController)obj.GetComponent<SpawnPointController> ();
				if (controller != null)
					controller.GeneratePoint ();
				return;
			}
		}

		SpawnPointController ctrl = (SpawnPointController)m_SpawnPoints[m_SpawnPoints.Count - 1].GetComponent<SpawnPointController> ();
		if (ctrl != null)
			ctrl.GeneratePoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_RunningTime >= m_TimeForSpawn) {
			SpawnObject ();
			m_RunningTime = 0f;
		} else
			m_RunningTime += Time.deltaTime;
	}
}
