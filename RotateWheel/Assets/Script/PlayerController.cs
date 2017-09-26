using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	#region param
	public float m_Acceleration;
	public float m_Speed;
	public float m_MaxSpeed;
	#endregion

	#region reference
	public AnimationCurve m_Curve;
	#endregion

	#region range
	public float m_Min;
	public float m_Max;
	#endregion

	private Vector2 m_Direction;

	void Awake()
	{
		m_Acceleration = 2f;
		m_Speed = 0f;
		m_MaxSpeed = 5f;
		//m_Curve = new AnimationCurve ();
		m_Curve.Evaluate(0.5f);

		//m_Direction = Vector2.up * -1;

		m_Min = -2f;
		m_Max = 2f;

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();
	}
	// Use this for initialization
	void Start () {
		
	}

	public void Reset (){
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject obj = other.gameObject;

		if (obj.tag == Constant.TAG_WHEEL) {
			Vector2 opPos = Vector2.zero - (Vector2)obj.transform.position;
			int random = Random.Range (0, 2);
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

			Debug.Log ("Direction: " + m_Direction.ToString ());

			m_Direction.Normalize ();

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			//m_Speed = 0f;
		}

		if (obj.tag == Constant.TAG_OBSTACLE || obj.tag == Constant.TAG_PLAYER) {
			float x = Random.Range (0, m_Direction.x);
			float y = Random.Range (0, m_Direction.y);
			m_Direction.x = -x;
			m_Direction.y = -y;

			Debug.Log ("Direction: " + m_Direction.ToString ());

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
