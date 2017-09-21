using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour {

	#region reference
	public GameObject m_Left;
	public GameObject m_Right;
	//public GameObject m_Player;
	#endregion

	#region param
	public float m_Speed;
	#endregion

	//private Rigidbody2D m_Body;

	private Vector3 m_Euler;

	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		m_Euler = gameObject.transform.eulerAngles;	
	}

	void FixedUpdate ()
	{
		
	}

	private void RotateObject (ref GameObject obj, Vector3 position, Vector3 rotation)
	{
		Vector3 objpos = obj.transform.position;
		float x = position.x + (objpos.x - position.x) * Mathf.Cos (rotation.z * Mathf.PI / 180) - (objpos.y - position.y) * Mathf.Sin (rotation.z * Mathf.PI / 180);
		float y = position.y + (objpos.x - position.x) * Mathf.Sin (rotation.z * Mathf.PI / 180) + (objpos.y - position.y) * Mathf.Cos (rotation.z * Mathf.PI / 180);

		obj.transform.position = new Vector3 (x, y, objpos.z);
		//obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(x, y, objpos.z), Time.fixedDeltaTime);
		//obj.transform.Translate(new Vector3(x, y, objpos.z));
		obj.transform.Rotate (rotation);
	}

	// Update is called once per frame
	void Update () {
		float direction = Input.GetAxis ("Horizontal");
//		if (direction < 0)
//			Debug.Log ("Left: " + direction);
//		else if (direction > 0)
//			Debug.Log ("Right: " + direction);
		gameObject.transform.Rotate (Vector3.forward * direction * (-1) * m_Speed * Time.deltaTime);
//		if (direction != 0)
//			m_Body.AddTorque (m_Torque, ForceMode2D.Impulse);
//		else
//			m_Body.AddTorque (0);

		Vector3 next = gameObject.transform.eulerAngles;
		Vector3 delta = next - m_Euler;

		if (delta != Vector3.zero) {
			Vector3 position = gameObject.transform.position;
			if (m_Left != null) {
				RotateObject (ref m_Left, position, delta);
			}

			if (m_Right != null) {
				RotateObject (ref m_Right, position, delta);
			}
		}

		m_Euler = next;
	}
}
