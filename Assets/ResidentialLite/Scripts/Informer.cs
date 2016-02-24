using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(AudioSource))]
public class Informer : RequiredGameObjects {


	//GABROMEDIA@GMAIL.COM
	//Casts ray from camera, recognizes buildings and displays GUI

	Houses lastInstance;
	public float raycastDistance;
	public AudioClip clickSound;

	//Initialize and check for missing gameObjects/components
	void Start () {
		Initialize();
		Debugger();
	}

	//Simple raycast to mouse position
	void Update(){

			if (Input.GetMouseButtonDown(0)) {
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit, raycastDistance)) {

					//Deselect previous selection
					if (lastInstance)
							DeselectPrevious(lastInstance);

					//Obtain component - in the demo scene, selecting residential or commercial classes act the same
					Houses house = hit.transform.GetComponent<Houses>();

					if (house !=null && !cameraBlur.enabled) {

						if (house.GetType() == typeof(Residential)) {
							Select(house, clickSound);
						}
					}
				}
			}

			//Adjust blur effect runtime based on whether GUI's being displayed or not
			if (houseImage.enabled && !cameraBlur.enabled)
				cameraBlur.enabled = true;
			else if (!houseImage.enabled && cameraBlur.enabled)
				cameraBlur.enabled = false;

			//Deselect previous selection on right mouse click
			if (Input.GetMouseButtonDown(1))
				DeselectPrevious(lastInstance);
		
	}

	//Play audio
	public void PlayAudio(AudioClip audioclip) {
		audioSource.Stop();
		audioSource.clip = audioclip;
		audioSource.Play();
	}

	//Select house, load assigned GUI image (if not found, throw error), enable GUI and blur camera. Store current instance in
	//lastInstance var
	void Select(Houses selection, AudioClip selectionSound) {
		PlayAudio(selectionSound);
		selection.selected = !selection.selected;
		if (selection.houseSprite)
			houseImage.sprite = selection.houseSprite;
		else
			Debug.LogError("GUI image is not assigned to this particular instance: " + selection.transform.name);
		houseImage.enabled = true;
		cameraBlur.enabled = true;
		lastInstance = selection;
	}

	//Deselect previous instance, disable GUI, null out Image component image
	void DeselectPrevious(Houses selectedInstance){
		if (selectedInstance != null) {
			houseImage.sprite = null;
			selectedInstance.selected = false;
			houseImage.enabled = false;
		}
	}

	protected override void Initialize() {
		//No instructions for Informer class
	}

	//Check for components
	protected override void Debugger() {
		if (audioSource == null) {
			GameObject g = GameObject.Find("SceneManager");
			g.AddComponent<AudioSource>();
			Debug.Log("<color=white>Audiosource component wasn't found on SceneManager gameObject, component added runtime</color>");
		}
		if (clickSound == null) {
			Debug.LogError("Click sound clip not assigned to Informer class in Inspector!");
		}
	}
}
