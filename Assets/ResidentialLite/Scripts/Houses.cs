using UnityEngine;
using System.Collections;

public abstract class Houses : MonoBehaviour {

	//This class is inherited by all classes created for different buildings. (eg. residential, commercial)

	[HideInInspector]
	public bool selected;

	public Sprite houseSprite;

	protected Color originalColor;
	private Color selectedColor = Color.green;
	protected Renderer r;

	protected void Selecting(){

		if (RequiredGameObjects.isDayTime) {
			if (selected) {
				r.material.color = selectedColor;
			}
			else {
				r.material.color = originalColor;
			}
		}
		else
			r.material.color = originalColor;
	}
}
