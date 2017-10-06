using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

	public Text m_Score;
	public Text m_Best;

	void Awake ()
	{
		if (m_Score != null)
			m_Score.text = "" + GameController.m_Instance.GetScore ();

		if (m_Best != null) {
			if (GameController.m_Instance.GetScore() >= GameController.m_Instance.GetBestScore () 
				&& GameController.m_Instance.GetBestScore() > 0)
				m_Best.color = Constant.GREEN;
			m_Best.text = "" + GameController.m_Instance.GetBestScore ();
		}
	}

	public void Replay()
	{
		SceneManager.LoadScene (Constant.SCENE_MAIN);	
	}
}
