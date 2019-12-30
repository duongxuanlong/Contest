using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour {
	public GameObject m_Player;

	private List<GameObject> m_Objects;

	private bool m_IsGrowth = true;

	public int m_Total;

	public float m_EffectTime;

	#region reference for Destruction Effect
	public AnimatorCtrl mDestructionEffect;
	const string DESTRUCTION_ANIM = "destruction";
	#endregion

	void OnEnable()
	{
		EventManager.GetAvailableCallback += GetAvailablePlayer;
		EventManager.SendBallTypeCallback += ReceiveBallType;
		EventManager.GenerateSpecialBallCallback += GenerateSpecialBalls;
	}

	void OnDisable()
	{
		EventManager.GetAvailableCallback -= GetAvailablePlayer;
		EventManager.SendBallTypeCallback -= ReceiveBallType;
		EventManager.GenerateSpecialBallCallback -= GenerateSpecialBalls;
	}

	void ReceiveBallType (PlayerController.BallType type)
	{
		if (type == PlayerController.BallType.Destroy) {
			EventManager.CanRun (false);
			EventManager.DontDestroy (false);
			// StartCoroutine (DeActiveDamBalls());
			StartCoroutine(StartDestruction());
		}
	}

	void GenerateSpecialBalls ()
	{
		GameObject obj = GetAvailablePlayer ();
		if (obj != null) {
			obj.transform.position = new Vector2 (0, 0);
			PlayerController ctrl = obj.GetComponent<PlayerController> ();
			if (ctrl != null) {
				ctrl.GenerateSpecialBall ();
			}
			obj.SetActive (true);
		}
	}

	IEnumerator StartDestruction()
	{
		if (mDestructionEffect != null)
		{
			mDestructionEffect.SetActive(true);
			mDestructionEffect.PlayAnim(DESTRUCTION_ANIM, Vector3.zero);
		}
		
		yield return new WaitForSeconds(0.7f);

		yield return StartCoroutine(DeActiveDamBalls());
	}

	IEnumerator DeActiveDamBalls()
	{
		for (int i = 0; i < m_Total; ++i) {
			if (m_Objects [i].activeInHierarchy) {
				PlayerController ctrl = m_Objects [i].GetComponent<PlayerController> ();
				if (ctrl.GetBallType () == PlayerController.BallType.Damage) {
					m_Objects [i].SetActive (false);
					ParticleMgr.SInstance.PlayParticle(PlayerController.BallType.Damage, m_Objects[i].transform.position);
					yield return new WaitForSeconds (m_EffectTime);
				}
			}
		}
		EventManager.CanRun (true);
		EventManager.DontDestroy (true);
	}

	void Awake () {
		if (m_Total == 0)
			m_Total = 50;

		if (m_EffectTime == 0)
			m_EffectTime = 1f;

		if (m_Objects == null)
			m_Objects = new List<GameObject> ();

		for (int i = 0; i < m_Total; i++) {
			GameObject obj = Instantiate (m_Player) as GameObject;
			obj.transform.SetParent(transform);
			obj.SetActive (false);
			m_Objects.Add (obj);
		}

		if (mDestructionEffect != null)
		{
			mDestructionEffect.InitAnimCtrl();
			mDestructionEffect.SetActive(false);
		}
	}
	
	private GameObject GetAvailablePlayer ()
	{
		for (int i = 0; i < m_Total; i++)
			if (!m_Objects [i].activeSelf)
				return m_Objects [i];

		if (m_IsGrowth) {
			GameObject obj = Instantiate (m_Player) as GameObject;
			m_Objects.Add (obj);
			m_Total++;
			return obj;
		}

		return null;
	}
}
