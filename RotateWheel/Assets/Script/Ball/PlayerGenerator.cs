using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour {
	public GameObject m_Player;

	public GameObject Pre_Circle;
	public GameObject Pre_Monster;
	public GameObject Pre_Protection;
	public GameObject Pre_Destruction;

	// private List<GameObject> m_Objects;

	private bool m_IsGrowth = true;

	public int m_Total;

	public float m_EffectTime;

	#region reference for Destruction Effect
	// public AnimatorCtrl mDestructionEffect;
	public ParticleObject mDamExplosion;
	const string DESTRUCTION_ANIM = "destruction";
	#endregion

	#region params
	Dictionary<PlayerController.BallType, List<GameObject>> dt_Balls;
	#endregion

	#region Sounds
	[Header("Sounds for destroying Virus")]
	public AudioClip Ref_Audio_Destroy_Virus;
	AudioSource m_AudioSource;
	#endregion
	

	#region  params for generate probability random
	[Header("Probability for random ball generation")]
	public int m_MaxDamage;
	public int m_MaxHeal;

	public int m_RangeDam;
	public int m_SpecialRange;
	public float m_SpecialPercent;
	public float[] m_ProbDam;
	public float m_PercentDamage;
	public float m_PercentDestroy;
	public float m_PercentProtect;
	public float m_SpecialProb;

	float[] m_ProbBall;
	#endregion

	void OnEnable()
	{
		// EventManager.GetAvailableCallback += GetAvailablePlayer;
		EventManager.GetAvailableCallback += GenerateRandomBall;
		EventManager.SendBallTypeCallback += ReceiveBallType;
		EventManager.GenerateSpecialBallCallback += GenerateSpecialBalls;
	}

	void OnDisable()
	{
		// EventManager.GetAvailableCallback -= GetAvailablePlayer;
		EventManager.GetAvailableCallback -= GenerateRandomBall;
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

	GameObject GenerateSpecialBalls ()
	{
		float prob = Random.value;
		PlayerController.BallType type = PlayerController.BallType.Count;
		GameObject obj = null;
		if (prob <= m_SpecialProb)
			type = PlayerController.BallType.Destroy;
		else
			type = PlayerController.BallType.Protect;
		obj = GetBall(type);
		obj.transform.position = Vector3.zero;
		obj.SetActive(true);
		return obj;
	}

	IEnumerator StartDestruction()
	{
		// if (mDestructionEffect != null)
		// {
		// 	mDestructionEffect.SetActive(true);
		// 	mDestructionEffect.PlayAnim(DESTRUCTION_ANIM, Vector3.zero);
		// }

		if (mDamExplosion != null)
		{
			mDamExplosion.PlayParticle(Vector3.zero);
		}
		
		yield return new WaitForSeconds(0.7f);

		yield return StartCoroutine(DeActiveDamBalls());
	}

	IEnumerator DeActiveDamBalls()
	{
		var ls = dt_Balls[PlayerController.BallType.Damage];
		foreach (var temp in ls)
		{
			if (temp.activeInHierarchy)
			{
				ParticleMgr.SInstance.PlayParticle(PlayerController.BallType.Damage, temp.transform.position);
				if (Ref_Audio_Destroy_Virus != null)
					m_AudioSource.PlayOneShot(Ref_Audio_Destroy_Virus);
				temp.SetActive(false);
				yield return new WaitForSeconds(m_EffectTime);
			}
		}
		EventManager.CanRun (true);
		EventManager.DontDestroy (true);
	}

	void InitRandomParams()
	{
		m_ProbBall = new float[(int)PlayerController.BallType.Count];

		//Heal balls
		if (m_MaxHeal == 0)
			m_MaxHeal = 5;

		//Damage balls
		if (m_MaxDamage == 0)
			m_MaxDamage = -10;
		if (m_PercentDamage == 0) {
			if (EventManager.CheckStartVeryHard())
				m_PercentDamage = 0.4f;
			else
				m_PercentDamage = 0.1f;
		}
		if (m_RangeDam == 0)
			m_RangeDam = 16;
		m_ProbDam = new float[m_RangeDam];
		if (m_SpecialRange == 0)
			m_SpecialRange = 2;
		if (m_SpecialPercent == 0)
			m_SpecialPercent = 0.1f;

		//Destroy balls
		if (m_PercentDestroy == 0)
			m_PercentDestroy = 0f;

		//Protect balls
		if (m_PercentProtect == 0)
			m_PercentProtect = 0f;

		if (m_SpecialProb == 0)
			m_SpecialProb = 0.5f;

		GenerateBallProbability();
	}

	void GenerateBallProbability ()
	{
		//Prob for balls
		float heal = 1 - (m_PercentDamage + m_PercentDestroy + m_PercentProtect);
		m_ProbBall [(int)PlayerController.BallType.Heal] = heal;
		m_ProbBall [(int)PlayerController.BallType.Damage] = m_PercentDamage;
		m_ProbBall [(int)PlayerController.BallType.Destroy] = m_PercentDestroy;
		m_ProbBall [(int)PlayerController.BallType.Protect] = m_PercentProtect;
		
		//Prob for dams
		float special = m_SpecialPercent / m_SpecialRange;
		float rest = (1 - m_SpecialPercent) / (m_RangeDam - m_SpecialRange);
		int index = m_RangeDam - m_SpecialRange;
		for (int i = 0; i < m_RangeDam; i++) {
			if (i < index)
				m_ProbDam [i] = rest;
			else
				m_ProbDam [i] = special;
		}
	}

	void CreateAllBalls ()
	{
		List<GameObject> medicine;
		List<GameObject> virus;
		List<GameObject> shield;
		List<GameObject> bomb;

		medicine = new List<GameObject>();
		virus = new List<GameObject>();
		shield = new List<GameObject>();
		bomb = new List<GameObject>();

		// create medicine, virus
		for (int i = 0; i < 20; ++i)
		{
			// medicine
			{
				GameObject obj = Instantiate(Pre_Circle) as GameObject;
				obj.transform.SetParent (transform);
				obj.SetActive(false);
				var ctrl = obj.GetComponent<PlayerController>();
				ctrl.SetBallType(PlayerController.BallType.Heal);
				medicine.Add(obj);
			}

			// virus
			{
				GameObject obj = Instantiate(Pre_Monster) as GameObject;
				obj.transform.SetParent(transform);
				obj.SetActive(false);
				PlayerController ctrl = obj.GetComponent<PlayerController>();
				ctrl.SetBallType(PlayerController.BallType.Damage);
				virus.Add(obj);
			}

			if (i < 5)
			{
				// shield
				{
					GameObject obj = Instantiate(Pre_Protection) as GameObject;
					obj.transform.SetParent(transform);
					obj.SetActive(false);
					PlayerController ctrl = obj.GetComponent<PlayerController>();
					ctrl.SetBallType(PlayerController.BallType.Protect);
					shield.Add(obj);
				}

				// destruction
				{
					GameObject obj = Instantiate(Pre_Destruction) as GameObject;
					obj.transform.SetParent(transform);
					obj.SetActive(false);
					PlayerController ctrl = obj.GetComponent<PlayerController>();
					ctrl.SetBallType(PlayerController.BallType.Destroy);
					bomb.Add(obj);
				}
			}
		}

		if (dt_Balls == null)
			dt_Balls = new Dictionary<PlayerController.BallType, List<GameObject>>();
		else
			dt_Balls.Clear();
		
		dt_Balls[PlayerController.BallType.Heal] = medicine;
		dt_Balls[PlayerController.BallType.Damage] = virus;
		dt_Balls[PlayerController.BallType.Protect] = shield;
		dt_Balls[PlayerController.BallType.Destroy] = bomb;

		InitRandomParams ();
	}

	void Awake () {
		if (m_Total == 0)
			m_Total = 50;

		if (m_EffectTime == 0)
			m_EffectTime = 1f;

		// if (m_Objects == null)
		// 	m_Objects = new List<GameObject> ();

		if (m_AudioSource == null)
			m_AudioSource = GetComponent<AudioSource>();


		CreateAllBalls();

		if (mDamExplosion != null)
		{
			mDamExplosion.InitParticleObject();
		}
	}
	
	// private GameObject GetAvailablePlayer ()
	// {
	// 	for (int i = 0; i < m_Total; i++)
	// 		if (!m_Objects [i].activeSelf)
	// 			return m_Objects [i];

	// 	if (m_IsGrowth) {
	// 		GameObject obj = Instantiate (m_Player) as GameObject;
	// 		m_Objects.Add (obj);
	// 		m_Total++;
	// 		return obj;
	// 	}

	// 	return null;
	// }

	GameObject GetBall (PlayerController.BallType type)
	{
		var ls = dt_Balls[type];
		GameObject ball = null;
		foreach (var obj in ls)
		{
			if (!obj.activeSelf)
			{
				ball = obj;
				break;
			}
		}

		if (ball == null)
			ball = AddMoreBall(type);
		
		return ball;
	}

	GameObject AddMoreBall (PlayerController.BallType type)
	{
		var ls = dt_Balls[type];
		GameObject pre = null;

		switch (type)
		{
			case PlayerController.BallType.Heal:
			{
				pre = Pre_Circle;
				break;
			}

			case PlayerController.BallType.Damage:
			{
				pre = Pre_Monster;
				break;
			}

			case PlayerController.BallType.Protect:
			{
				pre = Pre_Protection;
				break;
			}

			case PlayerController.BallType.Destroy:
			{
				pre = Pre_Destruction;
				break;
			}
		}

		GameObject obj = Instantiate(pre);
		obj.transform.SetParent(transform);
		obj.SetActive(false);
		ls.Add(obj);
		var ctrl = obj.GetComponent<PlayerController>();
		ctrl.SetBallType(type);
		return obj;
	}

	void UpdateBallParam (GameObject obj)
	{
		PlayerController ctrl = obj.GetComponent<PlayerController>();
		float amount = 0;

		switch (ctrl.GetBallType())
		{
			case PlayerController.BallType.Heal:
			{
				int greenhp = Mathf.RoundToInt (EventManager.GetStatus ());
				amount = Random.Range (1, (greenhp/4));
				break;
			}

			case PlayerController.BallType.Damage:
			{
				int hp = Mathf.RoundToInt (EventManager.GetStatus ());
				int current = hp - m_RangeDam;
				float prob = Random.value;

				//Adjust hyper dam
				if (1 - prob <= m_SpecialPercent) {
					if (EventManager.CanGenerateHyperDam ()) {
						EventManager.UpdateHyperDam ();
					} else {
						while ((1 - prob) <= m_SpecialPercent)
							prob = Random.value;
					}
				}

				for (int i = 0; i < m_RangeDam; i++) {
					if (prob <= m_ProbDam [i]) {
						amount = current + (i + 1);
						break;
					} else {
						prob -= m_ProbDam [i];
					}
				}
				if (amount > 0) {
					if (EventManager.CheckStartVeryHard() == false) {
						amount = 0 - Mathf.Round ((amount / 1.6f));
					}
					if (EventManager.CheckStartVeryHard()) {
						amount = 0 - Mathf.Round ((amount / 1.2f));
					}
				}
				break;
			}
		}

		ctrl.SetupParam(amount);
	}

	GameObject GenerateRandomBall ()
	{
		int count = EventManager.CheckRedBallCount ();
		GameObject obj = null;
		if (EventManager.ShouldGenerateAllGreen ()) {
			// SetObjectType (BallType.Heal);
			//EventManager.StopGenerateAllGcreen ();
			obj = GetBall(PlayerController.BallType.Heal);
			UpdateBallParam(obj);
		}
		else {
			float value = Random.value;
			int length = m_ProbBall.Length;
			for (int i = 0; i < length; ++i)
			{
				PlayerController.BallType type = PlayerController.BallType.Count;
				if (value <= m_ProbBall[i])
				{
					type = (PlayerController.BallType)i;
					if (type == PlayerController.BallType.Damage)
					{
						if (EventManager.CheckStartHard() == false) {
							if (count < 3) {
								//m_Type = BallType.Damage;
								//SetObjectType (BallType.Damage);
								obj = GetBall(PlayerController.BallType.Damage);
								EventManager.StartRedBallCount ();
							} else {
								// Debug.Log (count);
								//m_Type = BallType.Heal;
								// SetObjectType (BallType.Heal);
								obj = GetBall(PlayerController.BallType.Heal);
								EventManager.ResetRedBallCount ();
							}
						}
						else
							obj = GetBall(PlayerController.BallType.Damage);
					}
					else
						obj = GetBall(type);
					break;
				}
				else
					value -= m_ProbBall[i];
			}
		}

		UpdateBallParam(obj);
		return obj;
	}
}
