using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	public Text m_Score;
	public Text m_EndGame;
	private float m_CurrentPoints;

	void Awake()
	{
		m_CurrentPoints = 0f;
		if (m_Score != null) {
			m_Score.color = Constant.GREEN;
			m_Score.text = "" + m_CurrentPoints;
		}

		if (m_EndGame != null)
			m_EndGame.enabled = false;
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
		if (m_Score != null)
			m_Score.text = "" + m_CurrentPoints;
	}

	void EndGame ()
	{
		m_EndGame.enabled = true;
	}

	public void Reset()
	{
		int index = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (index);
	}
}
