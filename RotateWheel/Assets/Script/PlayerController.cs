using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	#region param
	public float m_Acceleration;
	public float m_Speed;
	public float m_MaxSpeed;

	public int m_MaxDamage;
	public int m_MaxHeal;

	public int m_RangeDam;
	public int m_SpecialRange;
	public float m_SpecialPercent;
	public float[] m_Prob;

	public float m_PercentDamage;
	#endregion

	#region reference
	public AnimationCurve m_Curve;

	Text m_TextAmount;
	SpriteRenderer m_Renderer;
	#endregion

	float m_CurrentAmount;
	Vector2 m_Direction;
	bool m_CanRun;

	void Awake()
	{
		if (m_Acceleration == 0)
			m_Acceleration = 3f;
		if (m_MaxSpeed == 0)
			m_MaxSpeed = 8f;
		m_Speed = 0f;
		
		if (m_MaxDamage == 0)
			m_MaxDamage = -10;
		if (m_MaxHeal == 0)
			m_MaxHeal = 5;
		if (m_PercentDamage == 0)
			m_PercentDamage = 0.5f;
		if (m_RangeDam == 0)
			m_RangeDam = 11;
		m_Prob = new float[m_RangeDam];
		if (m_SpecialRange == 0)
			m_SpecialRange = 2;
		if (m_SpecialPercent == 0)
			m_SpecialPercent = 0.1f;
		m_CanRun = true;

		//Initialize probability for each dam
		GenerateProbability ();
		
		//m_Curve.Evaluate(0.5f);

		//m_Direction = Vector2.up * -1;

		m_Direction = Random.insideUnitCircle;
		//Debug.Log ("Direction :" + m_Direction.ToString ());
		m_Direction.Normalize ();
//		if (Random.Range (0, 2) == 0)
//			m_Direction.x *= -1;
//		if (Random.Range (0, 2) == 0)
//			m_Direction.y *= -1;

		m_Renderer = GetComponent<SpriteRenderer> ();
		m_TextAmount = GetComponentInChildren<Text> ();

		//Set object type
		if (EventManager.ShouldGenerateAllGreen ()) {
			SetObjectType (1);
			EventManager.StopGenerateAllGcreen ();
		} else {
			float value = Random.value;
			if (value <= m_PercentDamage)
				SetObjectType (0);
			else
				SetObjectType (1);
		}
	}

	void GenerateProbability ()
	{
		float special = m_SpecialPercent / m_SpecialRange;
		float rest = (1 - m_SpecialPercent) / (m_RangeDam - m_SpecialRange);
		int index = m_RangeDam - m_SpecialRange;
		for (int i = 0; i < m_RangeDam; i++) {
			if (i < index)
				m_Prob [i] = rest;
			else
				m_Prob [i] = special;
		}
	}

	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}

	void OnEnable()
	{
		EventManager.CanRunCallback += CanRun;	
	}

	void OnDisable()
	{
		EventManager.CanRunCallback -= CanRun;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;
	}

	private void SetObjectType (int type)
	{
		switch (type) {
		case 0: // damage blue ball
			if (m_Renderer != null)
				m_Renderer.sprite = Resources.Load (Constant.DAMAGE, typeof(Sprite)) as Sprite;
			
			//m_CurrentAmount = Random.Range (m_MaxDamage, -1);
//			int range = (m_RangeDam - m_SpecialRange);
//			int hp = Mathf.RoundToInt (EventManager.GetStatus ()) - 1;
//			int current = hp / range;
//			current *= range;
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
				if (prob <= m_Prob [i]) {
					m_CurrentAmount = current + (i + 1);
					break;
				} else {
					prob -= m_Prob [i];
				}
			}
			if (m_CurrentAmount > 0)
				m_CurrentAmount = 0 - m_CurrentAmount;

			if (m_TextAmount != null) {
				m_TextAmount.text = "" + m_CurrentAmount;
				m_TextAmount.color = Constant.RED;
			}
			break;
		case 1: // heal pink ball
			if (m_Renderer)
				m_Renderer.sprite = Resources.Load (Constant.HEAL, typeof(Sprite)) as Sprite;
			m_CurrentAmount = Random.Range (1, m_MaxHeal);
			if (m_TextAmount != null) {
				m_TextAmount.text = "+" + m_CurrentAmount;
				m_TextAmount.color = Constant.GREEN;
			}
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject obj = other.gameObject;

		if (obj.tag == Constant.TAG_WHEEL) {
			EventManager.SendHPCallback (other.transform, this.m_CurrentAmount);
			Destroy (gameObject);
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
	
	// Update is called once per frame
	void Update () {
		if (m_CanRun) {
			float speed = m_Acceleration * Time.deltaTime + m_Speed;
			if (speed >= m_MaxSpeed)
				speed = m_MaxSpeed;
		
			gameObject.transform.Translate (m_Direction * speed * Time.deltaTime);
			//GetComponent<Rigidbody2D>().MovePosition(m_Direction * speed * Time.deltaTime);

			m_Speed = speed;
		}
	}


}
