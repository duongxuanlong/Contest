using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public enum BallType
	{
		Heal,
		Damage,
		Destroy,
		Protect,
		Count
	}

	#region param
	public float m_Acceleration;
	public float m_Speed;
	public float m_MaxSpeed;

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

	public float m_DeltaScale;
	public float m_OriginalScale;
	#endregion

	#region reference
	public AnimationCurve m_Curve;

	Text m_TextAmount;
	SpriteRenderer m_Renderer;
	#endregion

	float m_CurrentAmount;
	Vector2 m_Direction;
	bool m_CanRun;

	int count;

	public bool red = false;

	BallType m_Type; //type of ball
	float[] m_ProbBall; //prob for balls

	float mProtectionTime = 0.5f;

	void Awake()
	{
		if (m_Acceleration == 0)
			m_Acceleration = 3f;
		if (m_MaxSpeed == 0)
			m_MaxSpeed = 8f;
		m_Speed = 0f;

		m_ProbBall = new float[(int)BallType.Count];

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

		m_CanRun = true;

		if (m_DeltaScale == 0)
			m_DeltaScale = 0.03f;
		m_OriginalScale = 0.1f;

		//Initialize probability
		GenerateProbability ();
		
		//m_Curve.Evaluate(0.5f);

		//m_Direction = Vector2.up * -1;

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();

		m_Renderer = GetComponent<SpriteRenderer> ();
		m_TextAmount = GetComponentInChildren<Text> ();

		// mVisible = true;
		//Set object type
		GenerateObjectType ();
	}

	void GenerateProbability ()
	{
		//Prob for balls
		float heal = 1 - (m_PercentDamage + m_PercentDestroy + m_PercentProtect);
		m_ProbBall [(int)BallType.Heal] = heal;
		m_ProbBall [(int)BallType.Damage] = m_PercentDamage;
		m_ProbBall [(int)BallType.Destroy] = m_PercentDestroy;
		m_ProbBall [(int)BallType.Protect] = m_PercentProtect;
		
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

	// void OnBecameInvisible ()
	// {
	// 	//Destroy (gameObject);
	// 	// gameObject.SetActive(false);
	// 	//Debug.Log ("Pos: " + gameObject.transform.position);
	// 	mVisible = false;
	// }

	// private void OnBecameVisible() {
	// 	mVisible = true;	
	// }

	void OnEnable()
	{
		EventManager.CanRunCallback += CanRun;	

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();
		m_Speed = 0f;
		m_CanRun = true;
	}

	void OnDisable()
	{
		EventManager.CanRunCallback -= CanRun;
		m_CanRun = false;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;
	}

	bool CheckIsOutOfCamera ()
	{
		Vector3 pos = transform.position;

		bool result = false;
		if (pos.y >= Constant.CAMERA_UP_BOUND || 
			pos.y <= Constant.CAMERA_DOWN_BOUND ||
			pos.x >= Constant.CAMERA_RIGHT_BOUND ||
			pos.x <= Constant.CAMERA_LEFT_BOUND)
			{
				result = true;
				gameObject.SetActive(false);
			}

		return result;
	}

	public void GenerateSpecialBall()
	{
		float prob = Random.value;
		//Debug.Log ("Special balls prob: " + prob);
		if (prob <= m_SpecialProb)
			SetObjectType (BallType.Destroy);
		else
			SetObjectType(BallType.Protect);

		// SetObjectType(BallType.Destroy);
	}

	public void GenerateObjectType()
	{
		count = EventManager.CheckRedBallCount ();
		if (EventManager.ShouldGenerateAllGreen ()) {
			//m_Type = BallType.Heal;
			SetObjectType (BallType.Heal);
			//EventManager.StopGenerateAllGcreen ();
		}
		else {
			float value = Random.value;
			int length = m_ProbBall.Length;
			for (int i = 0; i < length; ++i)
			{
				if (value <= m_ProbBall[i])
				{
					m_Type = (BallType)i;
					if (m_Type == BallType.Damage)
					{
						if (EventManager.CheckStartHard() == false) {
							if (count < 3) {
								//m_Type = BallType.Damage;
								SetObjectType (BallType.Damage);
								EventManager.StartRedBallCount ();
							} else {
								// Debug.Log (count);
								//m_Type = BallType.Heal;
								SetObjectType (BallType.Heal);
								EventManager.ResetRedBallCount ();
							}
						}
						else
							SetObjectType (BallType.Damage);
					}
					else
						SetObjectType(m_Type);
					break;
				}
				else
					value -= m_ProbBall[i];
			}

//			if (value <= m_PercentDamage) {
//				if (EventManager.CheckStartHard() == false) {
//					if (count < 3) {
//						SetObjectType (BallType.Damage);
//						EventManager.StartRedBallCount ();
//					} else {
//						Debug.Log (count);
//						SetObjectType (BallType.Heal);
//						EventManager.ResetRedBallCount ();
//					}
//				}
//				else
//					SetObjectType (BallType.Damage);
//			}
//			else
//				SetObjectType (BallType.Heal);
		}
	}

	public BallType GetBallType ()
	{
		return m_Type;
	}
		
	private void SetObjectType (BallType type)
	{
		m_Type = type;

		switch (type) {
		case BallType.Damage: // damage blue ball
			if (m_Renderer != null) {
				//m_Renderer.color = Color.white; //Temporary option
				m_Renderer.sprite = Resources.Load (Constant.DAMAGE, typeof(Sprite)) as Sprite;
			}
			int hp = Mathf.RoundToInt (EventManager.GetStatus ());
			int current = hp - m_RangeDam;

			float prob = Random.value;

			red = true;

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
					m_CurrentAmount = current + (i + 1);
					break;
				} else {
					prob -= m_ProbDam [i];
				}
			}
			if (m_CurrentAmount > 0) {
				if (EventManager.CheckStartVeryHard() == false) {
					m_CurrentAmount = 0 - Mathf.Round ((m_CurrentAmount / 1.6f));
				}
				if (EventManager.CheckStartVeryHard()) {
					m_CurrentAmount = 0 - Mathf.Round ((m_CurrentAmount / 1.2f));
				}
			}

			if (m_TextAmount != null) {
				string stramount = m_CurrentAmount.ToString ();
				RectTransform rect = m_TextAmount.rectTransform;
				int length = stramount.Length;
				float newscale = (length - 2) > 0 ? m_DeltaScale * (stramount.Length - 2) : 0;
				if (newscale == 0)
					rect.localScale = new Vector2 (m_OriginalScale, m_OriginalScale);
				else
					rect.localScale = new Vector2 (m_OriginalScale - newscale, m_OriginalScale - newscale);
				m_TextAmount.text = stramount;
			}
			break;
		case BallType.Heal: // heal pink ball
			red = false;
			if (m_Renderer) {
				//m_Renderer.color = Color.white; //Temporary optionv
				m_Renderer.sprite = Resources.Load (Constant.HEAL, typeof(Sprite)) as Sprite;
			}
			int greenhp = Mathf.RoundToInt (EventManager.GetStatus ());
			m_CurrentAmount = Random.Range (1, (greenhp/4));
			if (m_TextAmount != null) {
				string stramount = "+" + m_CurrentAmount;
				RectTransform rect = m_TextAmount.rectTransform;
				int length = stramount.Length;
				float newscale = (length - 2) > 0 ? m_DeltaScale * (stramount.Length - 2) : 0;
				if (newscale == 0)
					rect.localScale = new Vector2 (m_OriginalScale, m_OriginalScale);
				else
					rect.localScale = new Vector2 (m_OriginalScale - newscale, m_OriginalScale - newscale);
				m_TextAmount.text = stramount;
			}
			break;
		case BallType.Destroy: //Destroy ball
			if (m_Renderer) {
				m_Renderer.sprite = Resources.Load (Constant.DESTROY, typeof(Sprite)) as Sprite;
				//m_Renderer.color = Color.gray;
			}
			m_CurrentAmount = 0;

			if (m_TextAmount != null)
				m_TextAmount.text = "";
			
			break;
		case BallType.Protect: //Protect ball
			if (m_Renderer) {
				m_Renderer.sprite = Resources.Load (Constant.PROTECT, typeof(Sprite)) as Sprite;
				//m_Renderer.color = Color.yellow;
			}

			m_CurrentAmount = 0;

			if (m_TextAmount != null)
				m_TextAmount.text = "";
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject obj = other.gameObject;

		if (obj.tag == Constant.TAG_WHEEL) {
			EventManager.SendInfoCallback (other.transform, this.m_CurrentAmount, m_Type);

			if (m_Type == BallType.Protect)
				StartCoroutine(WaitForProtectionRun());
				
			ParticleMgr.SInstance.PlayParticle(m_Type, transform.position, (m_Type == BallType.Damage) ? true : false);
			//Destroy (gameObject);
			gameObject.SetActive(false);
		}

		if (obj.tag == Constant.TAG_OBSTACLE || obj.tag == Constant.TAG_PLAYER) {

//			//Vector2 opPos = Vector2.zero - (Vector2)obj.transform.position;
//			float new_x;
//			float new_y;
//			new_x = Random.value /*+ opPos.x*/;
//			new_y = Random.value /*+ opPos.y*/;
//
//			m_Direction.x = new_x;
//			m_Direction.y = new_y;

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			m_Direction = Random.insideUnitCircle;
			m_Direction.Normalize ();

		}
	}

	IEnumerator WaitForProtectionRun ()
	{
		yield return new WaitForSeconds(mProtectionTime);
	}
	
	// Update is called once per frame
	void Update () {
		// if (!mVisible)
		// 	gameObject.SetActive(false);
		if (this.CheckIsOutOfCamera())
			return;
			
		if (m_CanRun && EventManager.IsNotDesotroying()) {
			float speed = m_Acceleration * Time.deltaTime + m_Speed;
			if (speed >= m_MaxSpeed)
				speed = m_MaxSpeed;
		
			gameObject.transform.Translate (m_Direction * speed * Time.deltaTime);
			//GetComponent<Rigidbody2D>().MovePosition(m_Direction * speed * Time.deltaTime);

			m_Speed = speed;
		}
	}


}
