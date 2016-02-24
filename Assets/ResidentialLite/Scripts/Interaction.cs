using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interaction : MonoBehaviour {

	//GABROMEDIA@GMAIL.COM

	//Light configuration script, which animates the directional light and changes skybox material, switches
	//street lights on and off.

	Animator anim;

	public Material daySky;
	public Material nightSky;

	public Toggle dayNightSwitch;
	public Toggle dayNight;
	public Slider dirIntensity;

	Light dirLight;

	void Start() {
		Debugger();
		Initialize();
	}

	void FixedUpdate() {
		DayTimeNightTimeCheck();
		RotateSun();
		SetIntensity();
	}

	void SetIntensity() {
		dirLight.intensity = dirIntensity.value;
	}

	void DayTimeNightTimeCheck() {
		if (dayNightSwitch.isOn) 
			SwitchToDay(true, daySky);
		else 
			SwitchToDay(false, nightSky);

	}

	private void SwitchToDay(bool state, Material skyMaterial) {
		RequiredGameObjects.isDayTime = state;
		RenderSettings.skybox = skyMaterial;
		dirLight.enabled = state;
	}

	void RotateSun(){
		dayNight.interactable = (RequiredGameObjects.isDayTime) ? true : false;
		dirIntensity.interactable = (RequiredGameObjects.isDayTime) ? true : false;

		if (dayNight.isOn) {
			anim.SetBool("Daytime", true);
		}
		else {
			anim.SetBool("Daytime", false);
		}
	}

	void Initialize() {
		dirLight = GameObject.Find("Directional Light").GetComponent<Light>();
		anim = dirLight.GetComponent<Animator>();
	}

	void Debugger(){
		GameObject g = GameObject.Find("Directional Light");
		if (g != null)
			dirLight = g.GetComponent<Light>();
		else {
			Debug.LogError("Directional Light gameobject not found in scene! Interaction class breaks execution");
			Debug.Break();
		}
		if (dayNight == null) {
			Debug.LogError("Toggle gameobject (Canvas child) not in scene or deactivated. RequiredComponents class breaks execution");
			Debug.Break();
		}
		if (dayNightSwitch == null) {
			Debug.LogError("Toggle 1 gameobject (Canvas child) not in scene or deactivated. RequiredComponents class breaks execution");
			Debug.Break();
		}
	}
}
