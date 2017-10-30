using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WheelController : MonoBehaviour {

	#region param
	public float m_HP = 50f;
	#endregion

	private Text m_HPText;
	public float m_CurrentHP;
	bool m_CanRun;

	public AudioClip green;
	public AudioClip red;


	SpriteRenderer m_Renderer;

	AudioSource audiosource;

	PlayerController.BallType m_Type;

	void Awake()
	{
		m_HPText = gameObject.GetComponentInChildren<Text> ();
		m_CanRun = true;
	}

	void Start ()
	{
		m_CurrentHP = m_HP;
		m_HPText.text = "" + m_CurrentHP;
		audiosource = GetComponent<AudioSource> ();
	}

	void OnEnable()
	{
		EventManager.ReceiveHPCallback += OnUpdateHP;
		EventManager.CanRunCallback += CanRun;
		EventManager.SendBallTypeCallback += ReceiveBallType;
	}

	void OnDisable()
	{
		EventManager.ReceiveHPCallback -= OnUpdateHP;
		EventManager.CanRunCallback -= CanRun;
		EventManager.SendBallTypeCallback -= ReceiveBallType;
	}

	void ReceiveBallType (PlayerController.BallType type)
	{
		m_Type = type;
	}

	void CanRun (bool run)
	{
		m_CanRun = run;
	}

	void OnUpdateHP(Transform identity, float amount)
	{
		if (gameObject.transform == identity) {
			//In protection mode
			if (m_Type == PlayerController.BallType.Damage
			    && EventManager.IsInProtection ()) {
				EventManager.ReduceProtection ();
				/*this.GetComponent<Animator> ().enabled = false;
				if (left) {
					m_Renderer.sprite = Resources.Load (Constant.PADLEFT, typeof(Sprite)) as Sprite;
				}
				if (right) {
					m_Renderer.sprite = Resources.Load (Constant.PADRIGHT, typeof(Sprite)) as Sprite;
				}*/
				return;
			}

			//Receive dam or heal
			if (m_Type == PlayerController.BallType.Damage ||
			    m_Type == PlayerController.BallType.Heal) {
				float newhp = m_CurrentHP + amount;
				if (newhp <= 0) {
					Destroy (gameObject);
					EventManager.ReducePart ();
					return;
				}
				m_CurrentHP = newhp;
				m_HPText.text = "" + m_CurrentHP;
			}
		}
	}

	public void UpdateWheel(Vector3 position, Vector3 rotation)
	{
		if (m_CanRun) {
			Vector3 objpos = gameObject.transform.position;
			float x = position.x + (objpos.x - position.x) * Mathf.Cos (rotation.z * Mathf.PI / 180) - (objpos.y - position.y) * Mathf.Sin (rotation.z * Mathf.PI / 180);
			float y = position.y + (objpos.x - position.x) * Mathf.Sin (rotation.z * Mathf.PI / 180) + (objpos.y - position.y) * Mathf.Cos (rotation.z * Mathf.PI / 180);

			gameObject.transform.position = new Vector3 (x, y, objpos.z);
			gameObject.transform.Rotate (rotation);
		}
	}

	public void UpdateAnimation()
	{
		if (EventManager.IsInProtection ()) {
			Debug.Log ("fdf");
			this.GetComponent<Animator> ().enabled = true;
		} else {
			this.GetComponent<Animator> ().enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		GameObject obj = other.gameObject;

		if (obj.tag == "Player") {
			if(obj.GetComponent<PlayerController>().red)
				audiosource.PlayOneShot (red);
			else
				audiosource.PlayOneShot (green);
		}
	}
}
