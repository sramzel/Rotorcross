using UnityEngine;
using System.Collections;

public class CopyRotation : MonoBehaviour {
	public Transform target;
	
	// Update is called once per frame
	void Update () {
		transform.rotation = target.rotation;
	}
}
