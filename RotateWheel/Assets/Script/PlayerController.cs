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

	public float m_PercentDamage;
	#endregion

	#region reference
	public AnimationCurve m_Curve;

	Text m_TextAmount;
	SpriteRenderer m_Renderer;
	#endregion

	#region range
	public float m_Min;
	public float m_Max;
	#endregion

	float m_CurrentAmount;
	Vector2 m_Direction;

	void Awake()
	{
		if (m_Acceleration == 0)
			m_Acceleration = 2f;
		if (m_MaxSpeed == 0)
			m_MaxSpeed = 5f;
		m_Speed = 0f;
		
		if (m_MaxDamage == 0)
			m_MaxDamage = -10;
		if (m_MaxHeal == 0)
			m_MaxHeal = 5;
		if (m_PercentDamage == 0)
			m_PercentDamage = 0.3f;

		m_Min = -2f;
		m_Max = 2f;
		
		//m_Curve.Evaluate(0.5f);

		//m_Direction = Vector2.up * -1;

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();

		m_Renderer = GetComponent<SpriteRenderer> ();
		m_TextAmount = GetComponentInChildren<Text> ();

		//Set object type
		float value = Random.value;
		if (value <= m_PercentDamage)
			SetObjectType (0);
		else
			SetObjectType (1);
	}

	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}

	public void SetObjectType (int type)
	{
		switch (type) {
		case 0: // damage blue ball
			if (m_Renderer != null)
				m_Renderer.sprite = Resources.Load (Constant.DAMAGE, typeof(Sprite)) as Sprite;
			m_CurrentAmount = Random.Range (m_MaxDamage, -1);
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
			Vector2 opPos = Vector2.zero - (Vector2)obj.transform.position;
			//int random = Random.Range (0, 2);
			//Debug.Log ("random: " + random);
			float new_x;
			float new_y;

			new_x = Random.Range (m_Min, m_Max) + opPos.x;
			new_y = Random.Range (m_Min, m_Max) + opPos.y;

//			if (random == 0) {
//				new_x = opPos.x;
//				new_y = Random.Range (0, opPos.y == 0 ? new_x + Mathf.Abs(opPos.y) : Mathf.Abs(opPos.y));
//				if (opPos.y <= 0)
//					new_y = 0 - new_y;
//			} else {
//				new_y = opPos.y;
//				new_x = Random.Range (0, opPos.x == 0 ? new_y + Mathf.Abs (opPos.x) : Mathf.Abs(opPos.x));
//				if (opPos.x <= 0)
//					new_x = 0 - new_x;
//			}

			m_Direction.x = new_x;
			m_Direction.y = new_y;

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			m_Direction.Normalize ();

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			//m_Speed = 0f;
		}

		if (obj.tag == Constant.TAG_OBSTACLE || obj.tag == Constant.TAG_PLAYER) {

			float x = Random.Range (0, m_Direction.x);
			float y = Random.Range (0, m_Direction.y);
			m_Direction.x = -x;
			m_Direction.y = -y;

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			m_Direction.Normalize ();
			if (obj.tag == Constant.TAG_OBSTACLE)
				Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float speed = m_Acceleration * Time.deltaTime + m_Speed;
		if (speed >= m_MaxSpeed)
			speed = m_MaxSpeed;
		
		gameObject.transform.Translate (m_Direction * speed * Time.deltaTime);
		//GetComponent<Rigidbody2D>().MovePosition(m_Direction * speed * Time.deltaTime);

		m_Speed = speed;
	}


}
