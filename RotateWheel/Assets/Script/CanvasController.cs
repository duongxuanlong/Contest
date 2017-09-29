using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	public Text m_Score;

	void Awake()
	{
		if (m_Score != null)
			m_Score.color = Constant.GREEN;
	}
}
