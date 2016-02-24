using UnityEngine;
using System.Collections;

public class CopyPosition : MonoBehaviour {

	public GameObject target;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position + offset;
	}
}
