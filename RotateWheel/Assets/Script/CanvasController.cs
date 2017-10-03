using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	Text m_Score;
	Text m_EndGame;
	Button m_Reset;
	private float m_CurrentPoints;

	void Awake()
	{
		m_CurrentPoints = 0f;

		Text[] texts = GetComponentsInChildren<Text> ();
		int length = texts.Length;

		m_Score = texts [0];
		m_Score.color = Constant.GREEN;
		m_Score.text = "" + m_CurrentPoints;

		m_EndGame = texts [1];
		m_EndGame.enabled = false;

		m_Reset = GetComponentInChildren<Button> ().GetComponent<Button> ();
		m_Reset.onClick.AddListener (Reset);

//		if (m_Score != null) {
//			m_Score.color = Constant.GREEN;
//			m_Score.text = "" + m_CurrentPoints;
//		}
//
//		if (m_EndGame != null)
//			m_EndGame.enabled = false;
	}

	void OnEnable()
	{
		EventManager.UpdatePointsCallback += UpdateUI;
		EventManager.EndGameCallback += EndGame;
	}

	void OnDisable()
	{
		EventManager.UpdatePointsCallback -= UpdateUI;
		EventManager.EndGameCallback -= EndGame;
	}

	void UpdateUI (float amount)
	{
		m_CurrentPoints += amount;
		//if (m_Score != null)
			m_Score.text = "" + m_CurrentPoints;
	}

	void EndGame ()
	{
		m_EndGame.text = "END GAME";
		m_EndGame.enabled = true;
	}

	public void Reset()
	{
		int index = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (index);
	}
}
