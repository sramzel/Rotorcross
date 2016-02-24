using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Inputs : MonoBehaviour {
	
	public int playerNum = 0;
	private float lastIPitch;
	private float lastIRoll;
	private float lastIYaw; 
	float[] exp = new float[4];
	float[] min = new float[4];
	float[] mid = new float[4];
	float[] max = new float[4];
	float[] inputScale = new float[4];
	public Transform player;
	public Waypoints course;

	public int waypoint = 0;
	float t;

	public string SaveFileName
	{
		get {return "PREF_INPUTS";}
	}

	public void Save (ISaveData data)
	{
		for (int axis = 0; axis < 4; axis ++) {
			data ["EXP_" + axis] = exp [axis];
			data ["MIN_" + axis] = min [axis];
			data ["MAX_" + axis] = max [axis];
		}
	}

	public void LoadSaved (ISaveData data)
	{
		for (int n = 0; n < 4; n ++) {
			if (data.HasKey ("EXP_" + n)) exp[n] = data.GetValue<float> ("EXP_" + n);
			if (data.HasKey ("MIN_" + n)) min[n] = data.GetValue<float> ("MIN_" + n);
			if (data.HasKey ("MAX_" + n)) max[n] = data.GetValue<float> ("MAX_" + n);
			mid [n] = (min [n] + max [n]) / 2f;
			inputScale[n] = (max [n] - min [n]) / 2f;
		};
	}

	public Int32 CycleTime {
		get; private set;
	}
	
	public Int32[] RCInput {
		get; private set;
	}
	
	public Int32[] Angle {
		get; private set;
	}
	
	public Int32[] Gyro {
		get; private set;
	}
	
	public void SetExp (string value, short axis)
	{
		exp [axis] = float.Parse (value);
	}
	
	public void SetMin (string value, short axis)
	{
		min [axis] = float.Parse (value);
		mid [axis] = (min [axis] + max [axis]) / 2f;
		inputScale[axis] = (max [axis] - min [axis]) / 2f;
	}
	
	public void SetMax (string value, short axis)
	{
		max [axis] = float.Parse (value);
		mid [axis] = (min [axis] + max [axis]) / 2f;
		inputScale[axis] = (max [axis] - min [axis]) / 2f;
	}
	
	public string GetExp (short axis)
	{
		if (axis < 4) return exp[axis].ToString();
		return "N/A";
	}
	
	public string GetMin (short axis)
	{
		if (axis < 4) return min[axis].ToString();
		return "N/A";
	}
	
	public string GetMax (short axis)
	{
		if (axis < 4) return max[axis].ToString();
		return "N/A";
	}

	public void Reset(){
		waypoint = 0;
		lastIPitch = 0;
		lastIRoll = 0;
		lastIYaw = 0; 
		lastError = 0;
	}

	Multirotor multirotor;
	void Start(){
		multirotor = GetComponent<Multirotor> ();
		Gyro = new Int32[3];
		RCInput = new Int32[4];
		Angle = new Int32[3];
		CycleTime = (Int32) (Time.fixedDeltaTime * 1000000);
		Debug.Log ("Inputs loaded");
	}
	
	Vector3 setPoint;
	Vector3 pos;

	public void SetSkill (float aiSkill)
	{
		posAdd = aiSkill;
		aiPosP = aiSkill / 30f;
		aiPosD = aiSkill / 60f;
	}
	
	void Update(){
		if (course != null) {
			Debug.DrawRay (course.waypoints [waypoint].transform.position, course.waypoints [(waypoint + 1) % course.waypoints.Length].transform.position - course.waypoints [waypoint].transform.position);
			Debug.DrawRay (transform.position, pos);
		}
	}

	float lastError;
	
	public float aiP = 250;
	
	public float aiI = 150;
	
	public float aiD = 500;
	
	public float aiThrottleP = 250;
	
	public float aiThrottleI = 150;
	
	public float aiThrottleD = 500;

	public float aiPosP = 1/25f;
	
	public float aiPosD = 100;
	
	public float posAdd = 100;

	public float throttleBias;

	public float turnBias;

	public float risk;

	public float addAmount;
	float lastErrorRoll;
	float lastErrorPitch;
	float lastErrorYaw;

	public float foresight;

	public float yawCos;

	public Vector3 currentWaypoint;

	public Vector3 nextWaypoint;

	// Update is called once per frame
	void FixedUpdate() {
		if (playerNum >= 0) {
			var inputLookVertical = (Input.GetAxis ("LookVertical") - mid[Constants.PITCH])/inputScale[Constants.PITCH];
			var inputHorizontal = (Input.GetAxis ("Horizontal") - mid[Constants.YAW])/inputScale[Constants.YAW];
			var inputLookHorizontal = (Input.GetAxis ("LookHorizontal") - mid[Constants.ROLL])/inputScale[Constants.ROLL];
			var inputVertical = (Input.GetAxis ("Vertical") - min[Constants.THROTTLE])/inputScale[Constants.THROTTLE];
			RCInput[Constants.PITCH] = (Int32) (Mathf.Pow (Math.Abs(inputLookVertical), exp [Constants.PITCH]) * Mathf.Sign (inputLookVertical) * 500);
			RCInput[Constants.YAW] = (Int32) (Mathf.Pow (Math.Abs(inputHorizontal), exp [Constants.YAW]) * Mathf.Sign (inputHorizontal) * 500);
			RCInput[Constants.ROLL] = (Int32) (Mathf.Pow (Math.Abs(inputLookHorizontal), exp [Constants.ROLL]) * Mathf.Sign (inputLookHorizontal) * 500);
			RCInput[Constants.THROTTLE] = (Int32) (Mathf.Pow (Math.Abs(inputVertical), exp [Constants.THROTTLE]) * Mathf.Sign (inputVertical) * 500);
		} else {
			if (multirotor.brokenProp == 0 && course.waypoints != null) {
				var path = (nextWaypoint - currentWaypoint);
				var pathNormalized = path.normalized;
				var dot = Vector3.Dot ((transform.position) - currentWaypoint, pathNormalized);
				var closestPoint = pathNormalized * (dot) + currentWaypoint;
				setPoint = closestPoint - transform.position;
				var error = setPoint.magnitude;
				pos = setPoint + pathNormalized * Mathf.Max(0, posAdd - aiPosP*error - aiPosD*(lastError + error));
				lastError = error;
				if ((dot + foresight * Mathf.Abs(Vector3.Dot(multirotor.RigidBody.velocity, pathNormalized)))/path.magnitude > 1f) {
					waypoint++;
					waypoint %= course.waypoints.Length;
					currentWaypoint = course.waypoints [waypoint].transform.position;
					nextWaypoint = course.waypoints [(waypoint + 1) % course.waypoints.Length].transform.position;
				}
				var errorYaw = pos[Constants.YAW];
				var errorRoll = pos[Constants.ROLL];
				var errorPitch = pos[Constants.PITCH];
				var pid = new Vector3();
				lastIYaw += errorYaw*Time.fixedDeltaTime;
				lastIPitch += errorPitch*Time.fixedDeltaTime;
				lastIRoll += errorRoll*Time.fixedDeltaTime;

				pid[Constants.YAW] = Mathf.Min(Mathf.Max(-500f, (errorYaw * aiThrottleP + lastIYaw * aiThrottleI + (errorYaw-lastErrorYaw) * aiThrottleD)), 500f);
				var pidErrorRoll = Mathf.Min (Mathf.Max (-500f, (errorRoll * aiP + lastIRoll * aiI + (errorRoll-lastErrorRoll) * aiD)), 500f);
				var pidErrorPitch = Mathf.Min (Mathf.Max (-500f, (errorPitch * aiP + lastIPitch * aiI + (errorPitch-lastErrorPitch) * aiD)), 500f);
				var pid2 = transform.InverseTransformDirection(new Vector3(pidErrorPitch, 0f, pidErrorRoll));
				RCInput[Constants.THROTTLE] = (int)(pid[Constants.YAW]+500f+Mathf.Abs(pid2[Constants.ROLL]*throttleBias)+Mathf.Abs(pid2[Constants.PITCH]*throttleBias));
				var angleLimit = (1000f - Mathf.Max(0f, pid[Constants.YAW]))/1000f;
				RCInput[Constants.PITCH] = (int)(pid2[Constants.ROLL]*angleLimit);
				RCInput[Constants.ROLL] = (int)(pid2[Constants.PITCH]*angleLimit);
				RCInput[Constants.YAW] = -(int)(pid2[Constants.PITCH]*turnBias*angleLimit);
				lastErrorRoll = errorRoll;
				lastErrorPitch = errorPitch;
				lastErrorYaw = errorYaw;
			} else {
				RCInput[Constants.THROTTLE] = 0;
				RCInput[Constants.PITCH] = 0;
				RCInput[Constants.ROLL] = 0;
				RCInput[Constants.YAW] = 0;
			}
		}

		Vector3 angle = transform.localEulerAngles;
		if (angle [Constants.PITCH] > 180f) angle[Constants.PITCH] -= 360f;
		if (angle [Constants.YAW] > 180f) angle[Constants.YAW] -= 360f;
		if (angle [Constants.ROLL] > 180f) angle[Constants.ROLL] -= 360f;
		if (angle [Constants.PITCH] < -180f) angle[Constants.PITCH] += 360f;
		if (angle [Constants.YAW] < -180f) angle[Constants.YAW] += 360f;
		if (angle [Constants.ROLL] < -180f) angle[Constants.ROLL] += 360f;
		angle *= 10f;

		Angle [Constants.PITCH] = (Int32)angle [Constants.PITCH];
		Angle [Constants.YAW] = (Int32)(-angle [Constants.YAW]);
		Angle [Constants.ROLL] = (Int32)(-angle [Constants.ROLL]);

		Vector3 gyro = transform.InverseTransformDirection (multirotor.RigidBody.angularVelocity) * 57.2957795f*4f;

		Gyro [Constants.PITCH] = (Int32) gyro [Constants.PITCH];
		Gyro [Constants.YAW] = (Int32) (-gyro [Constants.YAW]);
		Gyro [Constants.ROLL] = (Int32) (-gyro [Constants.ROLL]);
	}
}
