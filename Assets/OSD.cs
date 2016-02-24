using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OSD : MonoBehaviour {
	public Text speed;
	public Text voltage;
	public Text current;
	public Image target;
	public Configuration configuration;
	Canvas canvas;
	public Multirotor multirotor;
	Rigidbody rigidBody;
	public Text time;
	public Text bestTime;

	// Use this for initialization
	void Start () {
		rigidBody = multirotor.GetComponent<Rigidbody> ();
		canvas = GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		speed.text = Mathf.Round(rigidBody.velocity.magnitude*3.6f).ToString() + " kph";
		voltage.text = (Mathf.Round(multirotor.maxV*10f)/10f).ToString() + " V";
		current.text = Mathf.Round(multirotor.totalCurrent).ToString() + " A";

		var course = multirotor.course;
		if (course == null) {
			target.rectTransform.position = new Vector3 (0f, 0f, 0f);
		} else {
			var gateDetector = course.gates [multirotor.coursePosition];
			var gatePosition = gateDetector.GetPosition ();
			var newPosition = configuration.GetCamera ().WorldToScreenPoint (gatePosition);
			if (newPosition.z < 0) {
				target.enabled = false;
				newPosition.x = Mathf.Max (0f, Mathf.Min (newPosition.x, Screen.width));
				newPosition.y = Mathf.Max (0f, Mathf.Min (newPosition.y, Screen.height));
			} else {
				target.enabled = true;
				newPosition.x = Mathf.Max (0f, Mathf.Min (newPosition.x, Screen.width));
				newPosition.y = Mathf.Max (0f, Mathf.Min (newPosition.y, Screen.height));
				if (gatePosition.y > multirotor.transform.position.y)
					target.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
				else
					target.transform.eulerAngles = new Vector3 (0f, 0f, 180f);
			}
			target.rectTransform.position = newPosition;
		}

		if (multirotor.course != null) {
			time.gameObject.SetActive (true);
			bestTime.gameObject.SetActive (true);
			if (multirotor.warmUp) {
				time.text = ((int)(multirotor.lapTime + 1)).ToString ();
			} else {
				time.text = ((int)(multirotor.lapTime * 100f) / 100f).ToString ();
			}
			var bestCourseTime = multirotor.bestTime [course];
			if (bestCourseTime < float.MaxValue) bestTime.text = ((int)(bestCourseTime * 100f) / 100f).ToString ();
			else bestTime.gameObject.SetActive (false);
		} else {
			time.gameObject.SetActive (false);
			bestTime.gameObject.SetActive (false);
		}
	}

	public Vector2 WorldToCanvas(Canvas canvas,
	                                    Vector3 world_position,
	                                    Camera camera){
		if (camera == null) {
			camera = Camera.main;
		}
		
		var viewport_position = camera.WorldToViewportPoint (world_position);
		var canvas_rect = canvas.GetComponent<RectTransform> ();
		
		return new Vector2 ((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
		                   (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
	}
}
