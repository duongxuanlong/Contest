using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region param
	public float m_Acceleration;
	public float m_Speed;
	public float m_MaxSpeed;
	#endregion

	#region reference
	public GameObject m_Left;
	public GameObject m_Right;
	#endregion

	private Vector2 m_Direction;

	void Awake()
	{
		m_Acceleration = 3f;
		m_Speed = 0f;
		m_MaxSpeed = 8f;

		m_Direction = Vector2.up * -1;
	}
	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == Constant.TAG_WHEEL) {
			Vector2 opPos = Vector2.zero - (Vector2)coll.gameObject.transform.position;
			System.Random ran = new System.Random ();
			int x_max = ran.Next (0, 2);
			float new_x;
			float new_y;
			if (x_max == 0) {
				new_x = opPos.x;
				new_y = ran.Next (0, (int)Mathf.Abs(opPos.y));
				if (opPos.y < 0)
					new_y = 0 - new_y;
			} else {
				new_y = opPos.y;
				new_x = ran.Next (0, (int)Mathf.Abs(opPos.x));
				if (opPos.x < 0)
					new_x = 0 - new_x;
			}
			m_Direction.x = new_x;
			m_Direction.y = new_y;

			m_Speed = 0f;
		}
			
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == Constant.TAG_WHEEL) {
			Vector2 opPos = Vector2.zero - (Vector2)other.gameObject.transform.position;
			int random = Random.Range (0, 2);
			Debug.Log ("random: " + random);
			float new_x;
			float new_y;
			if (random == 0) {
				new_x = opPos.x;
				new_y = Random.Range (0, opPos.y == 0 ? new_x + Mathf.Abs(opPos.y) : Mathf.Abs(opPos.y));
				if (opPos.y <= 0)
					new_y = 0 - new_y;
			} else {
				new_y = opPos.y;
				new_x = Random.Range (0, opPos.x == 0 ? new_y + Mathf.Abs (opPos.x) : Mathf.Abs(opPos.x));
				if (opPos.x <= 0)
					new_x = 0 - new_x;
			}
			m_Direction.x = new_x;
			m_Direction.y = new_y;

			Debug.Log ("Direction: " + m_Direction.ToString ());

			m_Speed = 0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float speed = m_Acceleration * Time.deltaTime + m_Speed;
		if (speed >= m_MaxSpeed)
			speed = m_MaxSpeed;
		
		gameObject.transform.Translate (m_Direction * speed * Time.deltaTime);

		m_Speed = speed;
	}


}
