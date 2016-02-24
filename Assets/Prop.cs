using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour {

	bool isCollided = false;

	public bool IsCollided {
		get {
			return isCollided;
		}
	}
	
	
	void OnCollisionEnter(Collision collision) {
		isCollided = true;
	}
	
	void OnCollisionExit(Collision collision) {
		isCollided = false;
	}
}
