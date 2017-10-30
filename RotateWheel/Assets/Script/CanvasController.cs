using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	//This is unused
	//Second test
	//This file is unused
	public Text m_Score;
	public Text m_Best;
	public GameObject m_Tutorial;
	//public Text m_Tutorial;

	//public Text m_EndGame;
	//Button m_Reset;
	private float m_CurrentPoints;
	bool m_Init;
	PlayerController.BallType m_Type;

	void Awake()
	{
		m_CurrentPoints = 0f;

//		Text[] texts = GetComponentsInChildren<Text> ();
//		int length = texts.Length;

//		m_Score = texts [0];
//		m_Score.color = Constant.GREEN;
//		m_Score.text = "" + m_CurrentPoints;
//
//		m_EndGame = texts [1];
//		m_EndGame.enabled = false;

//		m_Reset = GetComponentInChildren<Button> ().GetComponent<Button> ();
//		m_Reset.onClick.AddListener (Reset);

		if (m_Score != null) {
			m_Score.color = Color.white;
			m_Score.text = "" + m_CurrentPoints;
		}

		m_Init = false;
		if (m_Best != null) {
			m_Best.color = Color.white;
			float best = 0f;
			if (GameController.m_Instance != null && GameController.m_Instance.IsReady()) {
				best = GameController.m_Instance.GetBestScore ();
				m_Init = true;
			}
			m_Best.text = "" + best;
		}

//		if (m_Tutorial != null)
//		if (m_Init && GameController.m_Instance.GetTutorialPhase () == Constant.TUTORIAL_PHASE_0)
//				m_Tutorial.text = "PRESS LEFT/RIGHT ARROW TO ROTATE";
			

//		if (m_EndGame != null)
//			m_EndGame.enabled = false;
	}

	void OnEnable()
	{
		EventManager.UpdatePointsCallback += UpdateUI;
		EventManager.EndGameCallback += EndGame;
		EventManager.SendBallTypeCallback += ReceiveBallType;
	}

	void OnDisable()
	{
		EventManager.UpdatePointsCallback -= UpdateUI;
		EventManager.EndGameCallback -= EndGame;
		EventManager.SendBallTypeCallback -= ReceiveBallType;
	}

	void ReceiveBallType (PlayerController.BallType type)
	{
		m_Type = type;
	}

	void UpdateUI (float amount)
	{
		if (m_Type == PlayerController.BallType.Damage ||
		    m_Type == PlayerController.BallType.Heal) {
			m_CurrentPoints += amount;
			if (m_Score != null)
				m_Score.text = "" + m_CurrentPoints;
			GameController.m_Instance.SetScore (m_CurrentPoints);

			if (m_Best != null) {
				if (m_CurrentPoints > GameController.m_Instance.GetBestScore ()) {
					m_Best.color = Constant.GREEN;
					m_Best.text = "" + m_CurrentPoints;
					EventManager.ScoreBest (m_CurrentPoints);
				}
			}
		}
	}

	void EndGame ()
	{
//		m_EndGame.text = "END GAME";
//		m_EndGame.enabled = true;
	}

	public void Reset()
	{
		int index = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (index);
	}

	void Update()
	{
		//m_Tutorial.SetActive (false);
//		if (m_Init)
//			return;

		if (GameController.m_Instance == null ||
		    !GameController.m_Instance.IsReady ())
			return;

		if (!m_Init) {
			m_Init = true;
			if (m_Best != null) {
				m_Best.text = "" + GameController.m_Instance.GetBestScore ();
			}
		}

		if (m_Tutorial != null)
		if (GameController.m_Instance.GetTutorialPhase () == Constant.TUTORIAL_PHASE_0)
			m_Tutorial.SetActive (true);
		else
			m_Tutorial.SetActive (false);
	}
}
