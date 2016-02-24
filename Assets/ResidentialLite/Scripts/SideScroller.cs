using UnityEngine;
using System.Collections;

public class SideScroller : MonoBehaviour {

	//GABROMEDIA@GMAIL.COM

	//Simple sidescroller script with manually set boundaries and lowering effect.

	int width;
	int height;
	float sensitivity = 20;

	public int boundary;
	public float scrollSpeed;

	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;

	Transform anchor;
	Camera cam;

	Informer informer;

	void Start() {
		anchor = GameObject.Find("Anchor").transform;
		cam = Camera.main;

		informer = GetComponent<Informer>();

		width = Screen.width;
		height = Screen.height;
	}

	void Update(){

		cam.transform.LookAt(anchor);

		AnchorMovement();

		float mouseInput = Input.GetAxisRaw("Mouse ScrollWheel") * sensitivity;
		int status;

		if (mouseInput > 0)
			status = 1;
		else if (mouseInput < 0)
			status = 2;
		else
			status = 3;

		cam.transform.position = Vector3.Lerp(cam.transform.position, target (status), Time.fixedDeltaTime * 5);

	}



	private Vector3 target (int status) {
		Vector3 bottom = new Vector3(cam.transform.position.x, 2, cam.transform.position.z);
		Vector3 top = new Vector3(cam.transform.position.x, 30, cam.transform.position.z);
		switch (status) {
		case 1:
			return bottom;
		case 2:
			return top;
		default:
			return cam.transform.position;
		}
	}

	private void AnchorMovement() {

		if (!informer.cameraBlur.enabled) {

			Vector3 aPos = anchor.position;

			//Right
			if (Input.mousePosition.x > width - boundary && aPos.x < xMax) {
				Vector3 moveDirection = new Vector3(aPos.x +10.0F, aPos.y, aPos.z);
				anchor.position = Vector3.Lerp(aPos, moveDirection, Time.deltaTime * scrollSpeed);
			}
			//Left
			if (Input.mousePosition.x < 0 + boundary && aPos.x > xMin) {
				Vector3 moveDirection = new Vector3(aPos.x -10.0F, aPos.y, aPos.z);
				anchor.position = Vector3.Lerp(aPos, moveDirection, Time.deltaTime * scrollSpeed);
			}
			//Forward
			if (Input.mousePosition.y > height - boundary && aPos.z < yMax) {
				Vector3 moveDirection = new Vector3(aPos.x, aPos.y, aPos.z + 10.0F);
				anchor.position = Vector3.Lerp(aPos, moveDirection, Time.deltaTime * scrollSpeed);
			}
			//Backward
			if (Input.mousePosition.y < 0 + boundary && aPos.z > yMin) {
				Vector3 moveDirection = new Vector3(aPos.x, aPos.y, aPos.z - 10.0F);
				anchor.position = Vector3.Lerp(aPos, moveDirection, Time.deltaTime * scrollSpeed);
			}
		}
	}
}