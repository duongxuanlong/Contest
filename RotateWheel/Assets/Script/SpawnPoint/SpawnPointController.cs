﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour {

	public GameObject m_Player;
	// Use this for initialization

	public void Reset()
	{
		Destroy (gameObject);
	}

	public void GeneratePoint()
	{
		//GameObject obj = Instantiate (m_Player, gameObject.transform.position, Quaternion.identity) as GameObject;
		GameObject obj = EventManager.GetAvailablePlayer();
		if (obj != null) {
			obj.transform.position = gameObject.transform.position;
			obj.transform.rotation = gameObject.transform.rotation;
			// PlayerController ctrl = (PlayerController)obj.GetComponent<PlayerController> ();
			// if (ctrl != null)
			// 	ctrl.GenerateObjectType ();
			obj.SetActive (true);
		}
	}

//	void Update()
//	{
//		int gen = Random.Range (0, 2);
//		if (!hasgen) {
//			GeneratePoint ();
//			hasgen = true;
//		}
//	}
}
