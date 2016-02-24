using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateDetector : MonoBehaviour {
	int childrenEntered = 0;

	public Vector3 offset;

	void Start(){
	}

	Multirotor getMultirotor (GameObject child)
	{
		Transform parent = child.transform;
		Multirotor multirotor = null;
		do {
			multirotor = parent.GetComponent<Multirotor>();
			parent = parent.parent;
		} while (parent != null && multirotor == null);
		return multirotor;
	}
	
	void OnTriggerEnter(Collider other) {
		var multirotor = getMultirotor (other.gameObject);
		var prop = other.GetComponent<Prop> ();
		if (prop == null) {
			multirotor.GateEntered (this);
		} else {
			multirotor.PropEntered(prop);
		}
	}

	void OnTriggerExit(Collider other) {
		var multirotor = getMultirotor (other.gameObject);
		if (other.GetComponent<Prop> () == null) {
			if (multirotor.course != null) multirotor.GateExited (this);
		}
	}

	public Vector3 GetPosition ()
	{
		return transform.position + offset;
	}
}
