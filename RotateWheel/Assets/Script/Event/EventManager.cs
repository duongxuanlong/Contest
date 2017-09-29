using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public delegate void SendHP (Transform identity, int amount);

	public static SendHP SendHPCallback;
}
