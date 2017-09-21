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
	public GameObject m_Left;
	public GameObject m_Right;
	public Button m_Reset;
	//public GameObject m_Player;
	#endregion

	#region Reset param
	private Vector3 m_LeftAngular;
	private Vector3 m_RightAngular;
	#endregion

	private Vector2 m_Direction;

	void Awake()
	{
		/*m_Acceleration = 3f;
		m_Speed = 0f;
		m_MaxSpeed = 8f;*/

		m_Direction = Vector2.up * -1;

		if (m_Reset != null)
			m_Reset.GetComponent<Button> ().onClick.AddListener (Reset);

		m_LeftAngular = m_Left.transform.eulerAngles;
		m_RightAngular = m_Right.transform.eulerAngles;
	}
	// Use this for initialization
	void Start () {
		
	}

	public void Reset (){
		if (m_Left != null) {
			m_Left.transform.position = new Vector3 (-4f, 0, 0);
			m_Left.transform.eulerAngles = m_LeftAngular;
		}

		if (m_Right != null) {
			m_Right.transform.position = new Vector3 (4f, 0, 0);
			m_Right.transform.eulerAngles = m_RightAngular;
		}

		gameObject.transform.position = new Vector3 (0, 3.5f, 0);

		m_Speed = 0;
		m_Direction = new Vector2 (0, -1);
	}

//	void OnCollisionEnter2D(Collision2D coll) {
//		if (coll.gameObject.tag == Constant.TAG_WHEEL) {
//			Vector2 opPos = Vector2.zero - (Vector2)coll.gameObject.transform.position;
//			System.Random ran = new System.Random ();
//			int x_max = ran.Next (0, 2);
//			float new_x;
//			float new_y;
//			if (x_max == 0) {
//				new_x = opPos.x;
//				new_y = ran.Next (0, (int)Mathf.Abs(opPos.y));
//				if (opPos.y < 0)
//					new_y = 0 - new_y;
//			} else {
//				new_y = opPos.y;
//				new_x = ran.Next (0, (int)Mathf.Abs(opPos.x));
//				if (opPos.x < 0)
//					new_x = 0 - new_x;
//			}
//			m_Direction.x = new_x;
//			m_Direction.y = new_y;
//
//			m_Direction.Normalize ();
//			m_Speed = 0f;
//		}
//			
//	}

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
			m_Direction.Normalize ();

			//Debug.Log ("Direction: " + m_Direction.ToString ());

			//m_Speed = 0f;
		}

		if (other.gameObject.tag == Constant.TAG_OBSTACLE) {
			float x = Random.Range (0, m_Direction.x);
			float y = Random.Range (0, m_Direction.y);
			m_Direction.x = -x;
			m_Direction.y = -y;
			m_Direction.Normalize ();
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
