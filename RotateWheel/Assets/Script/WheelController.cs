using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour {

	public void UpdateWheel(Vector3 position, Vector3 rotation)
	{
		Vector3 objpos = gameObject.transform.position;
		float x = position.x + (objpos.x - position.x) * Mathf.Cos (rotation.z * Mathf.PI / 180) - (objpos.y - position.y) * Mathf.Sin (rotation.z * Mathf.PI / 180);
		float y = position.y + (objpos.x - position.x) * Mathf.Sin (rotation.z * Mathf.PI / 180) + (objpos.y - position.y) * Mathf.Cos (rotation.z * Mathf.PI / 180);

		gameObject.transform.position = new Vector3 (x, y, objpos.z);
		gameObject.transform.Rotate (rotation);
	}
}
