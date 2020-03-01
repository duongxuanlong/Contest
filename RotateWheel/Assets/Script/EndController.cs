using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

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

		Analytics.CustomEvent(Constant.TRACKING_GAME_OVER, new Dictionary<string, object> {
			{ Constant.PARAM_SESSION_ID, AnalyticsSessionInfo.sessionId.ToString()},
			{ Constant.PARAM_SCORE, GameController.m_Instance.GetScore().ToString()},
			{ Constant.PARAM_BEST_SCORE, GameController.m_Instance.GetBestScore().ToString()}
		});
	}

	void Start ()
	{
		// reset watch ads count
		Constant.WATCH_ADS_COUNT = 0;
	}

	public void Replay()
	{
		// SceneManager.LoadScene (Constant.SCENE_MAIN);	
		Analytics.CustomEvent(Constant.TRACKING_REPLAY, new Dictionary<string, object>{
			{ Constant.PARAM_SESSION_ID, AnalyticsSessionInfo.sessionId.ToString()}
		});
		SceneManager.LoadScene(Constant.SCENE_LOADING);
	}
}
