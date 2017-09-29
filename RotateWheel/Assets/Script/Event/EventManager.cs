using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	//public delegate void DelSendHP (Transform identity, int amount);
	public delegate void DelReceiveHP (Transform identity, float amount);
	public delegate void DelUpdateUI (float amount);

	//public static DelSendHP SendHPCallback;
	public static event DelReceiveHP ReceiveHPCallback;

	public static event DelUpdateUI UpdateUICallback;

	public static void SendHPCallback (Transform identity, float amount)
	{
		if (ReceiveHPCallback != null)
			ReceiveHPCallback (identity, amount);

		if (UpdateUICallback != null && amount > 0)
			UpdateUICallback (amount);
	}
}
