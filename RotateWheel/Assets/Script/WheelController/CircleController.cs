using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour {

	#region reference
	public GameObject m_LeftPreb;
	public GameObject m_RightPreb;
	public AudioClip Ref_Audio_IBM;
	public AudioClip Ref_Audio_EndGame;

	//Add top down
	//public GameObject m_Top;
	//public GameObject m_Down;
	//public GameObject m_Player;

	AudioSource m_AudioSource;

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

		if (m_AudioSource == null)
			m_AudioSource = GetComponent<AudioSource>();

		if (Ref_Audio_IBM != null)
		{
			m_AudioSource.Stop();
			m_AudioSource.clip = Ref_Audio_IBM;
			m_AudioSource.loop = true;
			m_AudioSource.Play();
		}
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
		if (m_AudioSource != null)
		{
			m_AudioSource.Stop();
			m_AudioSource.loop = false;

			if (Ref_Audio_EndGame != null)
				m_AudioSource.PlayOneShot(Ref_Audio_EndGame);
		}
		EventManager.TriggerEndGame();
	}

	// Update is called once per frame
	void Update () {
		if (m_CanRun) {
			float direction = 0;
			
			#if UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (!(UNITY_IOS || UNITY_ANDROID) && UNITY_WEBGL)
				direction = Input.GetAxis ("Horizontal");
			#elif (UNITY_IOS || UNITY_ANDROID)
				if (Input.touchCount > 0)
				{
					Vector2 pos = Input.GetTouch(0).position;
					float half_screen_width = Screen.width / 2;
					if (pos.x > half_screen_width)
						direction = 1;
					else
						direction = -1;
				}
			#endif

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
