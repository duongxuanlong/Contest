using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour {

	#region reference
	public GameObject m_LeftPreb;
	public GameObject m_RightPreb;

	//Add top down
	//public GameObject m_Top;
	//public GameObject m_Down;
	//public GameObject m_Player;

	private GameObject m_Left;
	private GameObject m_Right;

	private WheelController m_LeftController;
	private WheelController m_RightController;
	//private WheelController m_TopController;
	//private WheelController m_BottomController;
	#endregion

	#region param
	public float m_Speed;
	#endregion

	private Vector3 m_Euler;

	void Awake()
	{
		if (m_LeftPreb != null) {
			m_Left = Instantiate (m_LeftPreb, m_LeftPreb.transform.position, m_LeftPreb.transform.rotation);
			m_LeftController = m_Left.GetComponent<WheelController> ();
		}

		if (m_RightPreb != null) {
			m_Right = Instantiate (m_RightPreb, m_RightPreb.transform.position, m_RightPreb.transform.rotation);
			m_RightController = m_Right.GetComponent<WheelController> ();
		}
	}

	// Use this for initialization
	void Start () {
		m_Euler = gameObject.transform.eulerAngles;	
	}

	// Update is called once per frame
	void Update () {
		float direction = Input.GetAxis ("Horizontal");
		gameObject.transform.Rotate (Vector3.forward * direction * (-1) * m_Speed * Time.deltaTime);

		Vector3 next = gameObject.transform.eulerAngles;
		Vector3 delta = next - m_Euler;

		if (delta != Vector3.zero) {
			Vector3 position = gameObject.transform.position;
			if (m_LeftController != null) {
				//RotateObject (ref m_Left, position, delta);
				m_LeftController.UpdateWheel(position, delta);
			}

			if (m_RightController != null) {
				//RotateObject (ref m_Right, position, delta);
				m_RightController.UpdateWheel(position, delta);
			}
		}

		m_Euler = next;
	}

	//	private void RotateObject (ref GameObject obj, Vector3 position, Vector3 rotation)
	//	{
	//		Vector3 objpos = obj.transform.position;
	//		float x = position.x + (objpos.x - position.x) * Mathf.Cos (rotation.z * Mathf.PI / 180) - (objpos.y - position.y) * Mathf.Sin (rotation.z * Mathf.PI / 180);
	//		float y = position.y + (objpos.x - position.x) * Mathf.Sin (rotation.z * Mathf.PI / 180) + (objpos.y - position.y) * Mathf.Cos (rotation.z * Mathf.PI / 180);
	//
	//		obj.transform.position = new Vector3 (x, y, objpos.z);
	//		//obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(x, y, objpos.z), Time.fixedDeltaTime);
	//		//obj.transform.Translate(new Vector3(x, y, objpos.z));
	//		obj.transform.Rotate (rotation);
	//	}
}
