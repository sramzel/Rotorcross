using UnityEngine;
using System.Collections;

public class TreeCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Tree Collided");
	}
}
