using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public abstract class RequiredGameObjects : MonoBehaviour {

	//This class is inherited by Interface and Informer classes

	public static bool isDayTime;
	public Image houseImage;

	protected AudioSource audioSource;

	[HideInInspector]
	public BlurOptimized cameraBlur;

	//Initialize all components
	void Awake(){
		cameraBlur = Camera.main.GetComponent<BlurOptimized>();
		audioSource = this.GetComponent<AudioSource>();
	}

	protected abstract void Debugger();
	protected abstract void Initialize();
}
