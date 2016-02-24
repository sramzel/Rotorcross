using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class Residential : Houses {

	void Start() {

		//Initialize original albedo color
		r = GetComponent<Renderer>();
		originalColor = r.material.color;


	}

	//Check if house is selected and adjust color
	void Update() {
		Selecting();
	}

}
