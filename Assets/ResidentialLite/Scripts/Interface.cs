using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Interface : RequiredGameObjects {

	//GABROMEDIA@GMAIL.COM

	public GameObject inGameCanvas;
	public GameObject mainCamera;

	void Start() {
		Debugger();
		Initialize();
	}

	protected override void Initialize() {
		cameraBlur.enabled = false;
		houseImage.enabled = false;
	}

	//Check for missing image effect components
	protected override void Debugger(){
		if (cameraBlur == null) {
			Debug.LogError("Optimized Blur effect missing from Main Camera - RequiredComponents class breaks execution");
			Debug.Break();
		}
		if (inGameCanvas == null) {
			Debug.LogError("Canvas gameobject not in scene or deactivated. RequiredComponents class breaks execution");
			Debug.Break();
		}

		if (houseImage == null) {
			Debug.LogError("houseImage (Canvas child) gameobject not in scene or deactivated. RequiredComponents class breaks execution");
			Debug.Break();
		}
		if (mainCamera == null) {
			Debug.LogError("Main Camera not in scene or deactivated. RequiredComponents class breaks execution");
			Debug.Break();
		}

	}

}
