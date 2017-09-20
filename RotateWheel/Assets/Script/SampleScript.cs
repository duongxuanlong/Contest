using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour {
	public Vector3 m_Position;

	public GameObject m_Left;
	public GameObject m_Right;
	// Use this for initialization

	private Vector3 m_CurrentEulerAngle;
	private Quaternion m_CurrentQua;
	//private Rigidbody2D m_Body;
	void Start () {
		//m_Body = GetComponent<Rigidbody2D> ();
		m_Position = gameObject.transform.position;
		m_CurrentEulerAngle = gameObject.transform.eulerAngles;
		m_CurrentQua = Quaternion.LookRotation (gameObject.transform.eulerAngles);
	}

	void FixedUpdate()
	{
		//Vector3 origin = gameObject.transform.position;
		Vector3 origin = m_Position;
		Vector3 angle = gameObject.transform.eulerAngles - m_CurrentEulerAngle;
		Quaternion next = Quaternion.LookRotation (gameObject.transform.eulerAngles);

		if (m_Left != null && angle != Vector3.zero) {
			RotateObject (ref m_Left, origin, angle);
			//RotateObject(ref m_Left, origin, next);
		}

		if (m_Right != null && angle != Vector3.zero) {
			RotateObject (ref m_Right, origin, angle);
			//RotateObject(ref m_Right, origin, next);
		}

		m_CurrentEulerAngle = gameObject.transform.eulerAngles;
		m_CurrentQua = next;
	}

	// Update is called once per frame
	void Update () {
		
	}

	private void RotateObject (ref GameObject obj, Vector3 position, Quaternion next)
	{
		Quaternion delta = next;// - m_CurrentQua;
		Vector3 objpos = obj.transform.position;
		float x = position.x + (objpos.x - position.x) * Mathf.Cos (delta.eulerAngles.z * Mathf.PI / 180) - (objpos.y - position.y) * Mathf.Sin (delta.eulerAngles.z * Mathf.PI / 180);
		float y = position.y + (objpos.x - position.x) * Mathf.Sin (delta.eulerAngles.z * Mathf.PI / 180) + (objpos.y - position.y) * Mathf.Cos (delta.eulerAngles.z * Mathf.PI / 180);

		//obj.transform.position = new Vector3 (x, y, objpos.z);
		obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(x, y, objpos.z), Time.fixedDeltaTime);
		//obj.transform.Translate(new Vector3(x, y, objpos.z));
		obj.transform.rotation = Quaternion.Slerp(m_CurrentQua, next, Time.fixedDeltaTime);
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
}
