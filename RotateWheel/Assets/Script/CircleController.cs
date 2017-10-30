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

	public int m_MaxProtect = 1;
	private int m_CurProtect = 0;
	//PlayerController.BallType m_Type;
	#endregion

	private Vector3 m_Euler;
	//private int m_TotalParts;
	bool m_CanRun;

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

		//m_TotalParts = 2;
		m_CanRun = true;

		//Deal with protect ball
		if (m_MaxProtect == 0)
			m_MaxProtect = 1;
		if (m_CurProtect == 0)
			m_CurProtect = 0;
	}

	// Use this for initialization
	void Start () {
		m_Euler = gameObject.transform.eulerAngles;	
	}

	void OnEnable()
	{
		EventManager.ReducePartCallback += ReduceWheelPart;
		EventManager.CanRunCallback += CanRun;
		EventManager.GetStatusCallback += GetMaxHP;
		EventManager.SendBallTypeCallback += ReceiveBallType;
		EventManager.IsInProtectionCallback += IsInProtection;
		EventManager.ReduceProtectionCallback += ReduceProtection;
	}

	void OnDisable()
	{
		EventManager.ReducePartCallback -= ReduceWheelPart;
		EventManager.CanRunCallback -= CanRun;
		EventManager.GetStatusCallback -= GetMaxHP;
		EventManager.SendBallTypeCallback -= ReceiveBallType;
		EventManager.IsInProtectionCallback -= IsInProtection;
		EventManager.ReduceProtectionCallback -= ReduceProtection;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;
	}

	float GetMaxHP ()
	{
		if (m_LeftController.m_CurrentHP > m_RightController.m_CurrentHP)
			return m_LeftController.m_CurrentHP;
		else
			return m_RightController.m_CurrentHP;
	}

	void ReceiveBallType (PlayerController.BallType type)
	{
		//m_Type = type;
		if (type == PlayerController.BallType.Protect) {
			m_CurProtect = m_MaxProtect;
		}

//		if (type == PlayerController.BallType.Damage) {
//			m_CurProtect--;
//			if (m_CurProtect < 0)
//				m_CurProtect = 0;
//		}
	}

	void ReduceProtection ()
	{
		m_CurProtect--;
		if (m_CurProtect < 0)
			m_CurProtect = 0;
	}

	bool IsInProtection ()
	{
		if (m_CurProtect > 0) 
			return true;

		return false;
	}

	void ReduceWheelPart ()
	{
//		m_TotalParts--;
//
//		if (m_TotalParts <= 0)
//			EventManager.TriggerEndGame ();
//		else
//			EventManager.IncreaseDiff ();
		EventManager.TriggerEndGame();
	}

	// Update is called once per frame
	void Update () {
		if (m_CanRun) {
			float direction = Input.GetAxis ("Horizontal");

			if (direction != 0)
				EventManager.ModifyPhase ();

			gameObject.transform.Rotate (Vector3.forward * direction * (-1) * m_Speed * Time.deltaTime);

			Vector3 next = gameObject.transform.eulerAngles;
			Vector3 delta = next - m_Euler;

			if (delta != Vector3.zero) {
				Vector3 position = gameObject.transform.position;
				if (m_LeftController != null) {
					//RotateObject (ref m_Left, position, delta);
					m_LeftController.UpdateWheel (position, delta);
				}

				if (m_RightController != null) {
					//RotateObject (ref m_Right, position, delta);
					m_RightController.UpdateWheel (position, delta);
					m_RightController.UpdateAnimation ();
				}
			}

			if (m_LeftController != null)
				m_LeftController.UpdateAnimation ();

			if (m_RightController != null)
				m_RightController.UpdateAnimation ();

			m_Euler = next;
		}
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
