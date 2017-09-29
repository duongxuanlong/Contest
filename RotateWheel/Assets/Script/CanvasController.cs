using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	public Text m_Score;
	private float m_CurrentPoints;

	void Awake()
	{
		m_CurrentPoints = 0f;
		if (m_Score != null) {
			m_Score.color = Constant.GREEN;
			m_Score.text = "" + m_CurrentPoints;
		}
	}

	void OnEnable()
	{
		EventManager.UpdateUICallback += UpdateUI;
	}

	void OnDisable()
	{
		EventManager.UpdateUICallback -= UpdateUI;
	}

	void UpdateUI (float amount)
	{
		m_CurrentPoints += amount;
		if (m_Score != null)
			m_Score.text = "" + m_CurrentPoints;
	}

	public void Reset()
	{
		int index = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (index);
	}
}
