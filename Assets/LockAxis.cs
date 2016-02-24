using UnityEngine;
using System.Collections;

public class LockAxis : MonoBehaviour {
	public Multirotor target;
	public Camera fpvCam;
	Camera losCam;
	public bool follow = true;

	void Start(){
		losCam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		var t = Time.fixedDeltaTime;
		var oldRotation = transform.rotation;
		transform.LookAt (target.transform);
		if (follow) {
			transform.rotation = Quaternion.Lerp (oldRotation, transform.rotation, 35f*t);
			var localEulerAngles = target.transform.localEulerAngles;
			var scale = .25f;
			var newPosition = new Vector3(
				target.transform.position.x - Mathf.Sin (target.transform.eulerAngles.y*0.0174533f)*scale,
				target.transform.position.y + .3f,
				target.transform.position.z - Mathf.Cos (target.transform.eulerAngles.y*0.0174533f)*scale
				);

				// car's up-vector has a down facing component
			transform.position = Vector3.Lerp (transform.position, newPosition, 7.5f * t);
			losCam.fieldOfView = fpvCam.fieldOfView;
		} else {
			var distance = Vector3.Distance (gameObject.transform.position, target.transform.position);
			losCam.fieldOfView = Mathf.Lerp (losCam.fieldOfView, target.frameSize * 5f * fpvCam.fieldOfView / Mathf.Max (distance, 2.5f), t);
		}
	}
}
