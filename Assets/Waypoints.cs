using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Waypoints : MonoBehaviour {

	public GameObject[] waypoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var lastGameObject = waypoints [waypoints.Length - 1];
		foreach (GameObject gameObject in waypoints) {
			Debug.DrawLine(lastGameObject.transform.position, gameObject.transform.position);
			lastGameObject = gameObject;
		}
	}
}
