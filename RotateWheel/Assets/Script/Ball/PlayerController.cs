using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public enum BallType
	{
		Heal,
		Damage,
		Destroy,
		Protect,
		Count
	}

	#region param
	public float m_Acceleration;
	public float m_Speed;
	public float m_MaxSpeed;

	public float m_DeltaScale;
	public float m_OriginalScale;

	public float m_RotateFactor;

	#endregion

	#region reference
	Text m_TextAmount;
	SpriteRenderer m_Renderer;
	public PlayerRotate Ref_Rotate;
	#endregion

	float m_CurrentAmount;
	Vector2 m_Direction;
	bool m_CanRun;
	int m_RotateDirection = 1;

	// int count;

	public bool red = false;

	BallType m_Type; //type of ball
	float mProtectionTime = 0.5f;

	void Awake()
	{
		if (m_Acceleration == 0)
			m_Acceleration = 3f;
		if (m_MaxSpeed == 0)
			m_MaxSpeed = 8f;
		m_Speed = 0f;

		m_CanRun = true;

		if (m_DeltaScale == 0)
			m_DeltaScale = 0.03f;
		m_OriginalScale = 0.1f;

		m_RotateFactor = 40;

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();

		m_Renderer = GetComponent<SpriteRenderer> ();
		m_TextAmount = GetComponentInChildren<Text> ();
	}

	void OnEnable()
	{
		EventManager.CanRunCallback += CanRun;	

		m_Direction = Random.insideUnitCircle;
		m_Direction.Normalize ();

		m_Speed = 0f;
		m_CanRun = true;
	}

	void OnDisable()
	{
		EventManager.CanRunCallback -= CanRun;
		m_CanRun = false;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;
	}

	bool CheckIsOutOfCamera ()
	{
		Vector3 pos = transform.position;

		bool result = false;
		if (pos.y >= Constant.CAMERA_UP_BOUND || 
			pos.y <= Constant.CAMERA_DOWN_BOUND ||
			pos.x >= Constant.CAMERA_RIGHT_BOUND ||
			pos.x <= Constant.CAMERA_LEFT_BOUND)
			{
				result = true;
				gameObject.SetActive(false);
			}

		return result;
	}

	public BallType GetBallType ()
	{
		return m_Type;
	}

	public void SetBallType (BallType type)
	{
		m_Type = type;

		if (m_Type == BallType.Damage)
			red = true;
		else
			red = false;
	}

	public void SetupParam (float amount)
	{
		m_CurrentAmount = amount;

		if (m_TextAmount != null) {
			string stramount = "" + Mathf.Abs(m_CurrentAmount);
			RectTransform rect = m_TextAmount.rectTransform;
			int length = stramount.Length;
			float newscale = (length - 2) > 0 ? m_DeltaScale * (stramount.Length - 2) : 0;
			if (newscale == 0)
				rect.localScale = new Vector2 (m_OriginalScale, m_OriginalScale);
			else
				rect.localScale = new Vector2 (m_OriginalScale - newscale, m_OriginalScale - newscale);
			m_TextAmount.text = stramount;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject obj = other.gameObject;

		if (obj.tag == Constant.TAG_WHEEL) {
			EventManager.SendInfoCallback (other.transform, this.m_CurrentAmount, m_Type);

			if (m_Type == BallType.Protect)
				StartCoroutine(WaitForProtectionRun());
				
			ParticleMgr.SInstance.PlayParticle(m_Type, transform.position, (m_Type == BallType.Damage) ? true : false);
			//Destroy (gameObject);
			gameObject.SetActive(false);
		}

		if (obj.tag == Constant.TAG_OBSTACLE || obj.tag == Constant.TAG_PLAYER) {

			m_Direction = Random.insideUnitCircle;
			m_Direction.Normalize ();
			m_RotateDirection *= -1;
		}
	}

	IEnumerator WaitForProtectionRun ()
	{
		yield return new WaitForSeconds(mProtectionTime);
	}
	
	// Update is called once per frame
	void Update () {
		// if (!mVisible)
		// 	gameObject.SetActive(false);
		if (this.CheckIsOutOfCamera())
			return;
			
		if (m_CanRun && EventManager.IsNotDesotroying()) {
			float speed = m_Acceleration * Time.deltaTime + m_Speed;
			if (speed >= m_MaxSpeed)
				speed = m_MaxSpeed;
		
			gameObject.transform.Translate (m_Direction * speed * Time.deltaTime);

			// no need to rotate
			// if (m_Type == BallType.Heal)
			// {
			// 	if (Ref_Rotate != null)
			// 	{
			// 		Ref_Rotate.UpdateRotate(m_RotateDirection * speed * Time.deltaTime * m_RotateFactor);
			// 	}
			// }

			
			//GetComponent<Rigidbody2D>().MovePosition(m_Direction * speed * Time.deltaTime);

			m_Speed = speed;
		}
	}


}
