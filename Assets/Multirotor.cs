using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[RequireComponent (typeof(Inputs))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Configuration))]
public class Multirotor : MonoBehaviour
{
	public Course course;
	public int coursePosition;
	GateDetector currentGate;
	Inputs inputs;
	Rigidbody rigidBody;
	string saveName = "";
	int gateIsEntered;
	HashSet<Prop> propsEntered = new HashSet<Prop>();
	public bool warmUp;
	public static bool ResetAI;

	int gatesPassed;
	
	public BestTime bestTime = new BestTime();

	public class BestTime {
		Dictionary<Course, float> times = new Dictionary<Course, float>();
		public float this[Course course] {
			get { return times.Keys.Contains (course) ? times [course] : float.MaxValue; }
			set { times [course] = value; }
		}
	}

	public string SaveName {
		get {
			return Inputs.playerNum < 0 ? "" : "_" + saveName;
		}
		set {
			saveName = value;
		}
	}

	public string SaveFileName {
		get {
			return "PREF_MULTIROTOR";
		}
	}

	public void Save ()
	{
		var data = GetSaveData (SaveFileName, SaveName);
		data ["PID"] = pidController;
		data ["PROP_DIAM"] = propDiam;
		data ["PROP_PITCH"] = PITCH;
		data ["PROP_MASS"] = propMass;
		data ["MOTOR_KV"] = z;
		data ["MOTOR_RES"] = r;
		data ["MOTOR_CUR"] = i0;
		data ["BATTERY_V"] = y;
		data ["FRAME_MASS"] = rigidBody.mass/scale;
		data ["FRAME_RES"] = airResistance;
		data ["PROP_E"] = PROP_E;
		data ["FRAME_SIZE"] = frameSize;
		data.Save ();
		var pidData = GetSaveData (PidController.SaveFileName, SaveName);
		PidController.Save (pidData);
		pidData.Save ();
		var inputData = GetSaveData (Inputs.SaveFileName, "");
		Inputs.Save (inputData);
		inputData.Save ();
		Debug.Log ("Saved " + saveName); 
	}

	public static SaveData GetSaveData (string fileName, string saveName)
	{
		SaveData data;
		var path = Path.Combine (Application.streamingAssetsPath, fileName + saveName) + ".uml";
		try {
			data = SaveData.Load (path);
		} catch (System.InvalidOperationException) {
			try {
				path = Path.Combine (Application.streamingAssetsPath, fileName) + ".uml";
				data = SaveData.Load (path);
				data.fileName = fileName + saveName;
			} catch (System.InvalidOperationException) {
				data = new SaveData (fileName);
			}
		}
		return data;
	}

	public void LoadSaved ()
	{
		var data = GetSaveData (SaveFileName, SaveName);
		if (data.HasKey ("PROP_DIAM"))
			propDiam = data.GetValue<float> ("PROP_DIAM");
		if (data.HasKey ("PROP_PITCH"))
			PITCH = data.GetValue<float> ("PROP_PITCH");
		if (data.HasKey ("PROP_MASS"))
			propMass = data.GetValue<float> ("PROP_MASS");
		if (data.HasKey ("MOTOR_KV"))
			z = data.GetValue<float> ("MOTOR_KV");
		if (data.HasKey ("MOTOR_RES"))
			r = data.GetValue<float> ("MOTOR_RES");
		if (data.HasKey ("MOTOR_CUR"))
			i0 = data.GetValue<float> ("MOTOR_CUR");
		if (data.HasKey ("BATTERY_V"))
			y = data.GetValue<float> ("BATTERY_V");
		if (data.HasKey ("FRAME_MASS"))
			rigidBody.mass = data.GetValue<float> ("FRAME_MASS")*scale;
		if (data.HasKey ("FRAME_RES"))
			airResistance = data.GetValue<float> ("FRAME_RES");
		if (data.HasKey ("PID"))
			pidController = data.GetValue<int> ("PID");
		if (data.HasKey ("PROP_E"))
			PROP_E = data.GetValue<float> ("PROP_E");
		if (data.HasKey ("FRAME_SIZE"))
			frameSize = data.GetValue<float> ("FRAME_SIZE");
		if (Inputs.playerNum >= 0) setSize (frameSize);
		InitInputs ();
		InitPidController ();
		calculateTerms ();
		Debug.Log ("Loaded " + saveName);
	}

	public Inputs Inputs {
		get {
			if (inputs == null) 
				inputs = GetComponent<Inputs> ();
			return inputs;
		}
	}

	public Rigidbody RigidBody {
		get {
			return rigidBody;
		}
	}

	public void GateEntered (GateDetector gate)
	{
		if (gateIsEntered == 0) {
			currentGate = gate;
		}

		if (currentGate == gate) {
			gateIsEntered++;
		}
	}

	public void GateExited (GateDetector gate)
	{
		if (currentGate == gate) {
			gateIsEntered--;
			if (gateIsEntered == 0) {
				if (propsEntered.Count == Props.Length && course.gates[coursePosition] == currentGate) {
					coursePosition++;
					gatesPassed++;
					if (gatesPassed > course.gates.Length) {
						if (lapTime < bestTime[course]) bestTime[course] = lapTime;
						lapTime = 0f;
						gatesPassed = 1;
					}
					coursePosition%=course.gates.Length;
					Debug.Log("Course Position: " + coursePosition);
				}
				propsEntered.Clear ();
			}
		}
	}
	
	public void PropEntered (Prop prop)
	{
		propsEntered.Add (prop);
	}

	void calculateTerms ()
	{
		h = 360 / 60 * 0.0174532925f * propMass * 7 / 12;
		var scaledRadius = propDiam;
		var scaledPitch = PITCH;
		K1 = -a * r * Mathf.Pow (scaledRadius, 4.5f) * PROP_E * Time.fixedDeltaTime * z * z;
		K2 = 4 * a * b * r * scaledPitch * Mathf.Pow (scaledRadius, 4.5f) * PROP_E * Time.fixedDeltaTime * z * z;
		K3 = a * r * Mathf.Pow (scaledRadius, 4.5f) * PROP_E * Time.fixedDeltaTime * z * z;
		K4 = 4 * c * d * Mathf.Sqrt (scaledPitch) * Time.fixedDeltaTime + h * r * Mathf.Sqrt (scaledPitch) * scaledRadius * scaledRadius * z * z;
		K5 = 2 * a * b * r * scaledPitch * Mathf.Pow (scaledRadius, 4.5f) * PROP_E * Time.fixedDeltaTime * z * z;
		K6 = 360 / 60 * 0.0174532925f * (scaledRadius / 2f * scaledRadius / 2f * propMass * 7f / 12f) / Time.fixedDeltaTime;
		K7 = scaledRadius * PROP_E / 2f;
		K8 = a * Mathf.Pow (scaledRadius * 2, 3.5f) / Mathf.Sqrt (scaledPitch);
		K9 = b * scaledPitch;
		C1 = 4 * c * d * i0 * r * Mathf.Sqrt (scaledPitch) * Time.fixedDeltaTime * z;
		C2 = 4 * c * d * Mathf.Sqrt (scaledPitch) * Time.fixedDeltaTime * z;
		C3 = h * r * Mathf.Sqrt (scaledPitch) * scaledRadius * scaledRadius * z * z;
		C4 = 4 * c * d * Mathf.Sqrt (scaledPitch) * Time.fixedDeltaTime + h * r * Mathf.Sqrt (scaledPitch) * scaledRadius * scaledRadius * z * z;
	}
	
	public Vector3 startPos;
	Quaternion startRot;

	public void addTorqueAtPosition(Vector3 torque, Vector3 posToApply, Rigidbody body){
		Vector3 torqueAxis = torque.normalized;
		Vector3 ortho = new Vector3(1,0,0);
		if ((torqueAxis - ortho).magnitude < float.Epsilon) {
			ortho = new Vector3(0,1,0);
		}
		// ref: OrthoNorm(V3 &a, V3 &b) { a.normalise(); V3 tmp=a.cross(b); tmp.normalise(); b = tmp.cross(b); }
		Vector3.OrthoNormalize(ref torqueAxis, ref ortho);
		// calculate force 
		Vector3 force = Vector3.Cross((0.5f*torque), (ortho));
		rigidBody.AddForceAtPosition(force/scale, (posToApply+ortho*scale));
		rigidBody.AddForceAtPosition(-force/scale, posToApply-ortho*scale);
	}

	Vector3[] mountPos0;
	Vector3[] propDiam0;

	void Start ()
	{
		mixer = Mixer.Mixers (mixerIndex);
		startPos = transform.position;
		startRot = transform.rotation;
		rigidBody = GetComponent<Rigidbody> ();
		motorSpeeds = new float[mixer.GetLength(0)];
		forces = new float[mixer.GetLength (0)];
		mountPos0 = new Vector3[mounts.Length];
		for (int i = 0; i < mounts.Length; i++) {
			mountPos0[i] = mounts[i].transform.localPosition;
		}
		propDiam0 = new Vector3[Props.Length];
		for (int i = 0; i < Props.Length; i++) {
			propDiam0[i] = Props[i].transform.localScale;
		}

		LoadSaved ();
		Debug.Log ("Multirotor loaded");
	}

	bool alreadyScaledTensor;

	void CalcluateInertiaTensor ()
	{
		rigidBody.inertiaTensor = new Vector3 (
			4f*.28f*rigidBody.mass * (frameSize * frameSize / 8f) + 1f/12f*.46f*rigidBody.mass*(.115f*.115f+.035f*.035f) + 1f/12f*.46f*rigidBody.mass*(frameSize*frameSize/2f), 
			4f*.28f*rigidBody.mass * (frameSize * frameSize / 4f) + 1f/12f*.46f*rigidBody.mass*(.115f*.115f+.021f*.021f) + 1f/12f*.46f*rigidBody.mass*(frameSize*frameSize), 
			4f*.28f*rigidBody.mass * (frameSize * frameSize / 8f) + 1f/12f*.46f*rigidBody.mass*(.035f*.035f+.021f*.021f) + 1f/12f*.46f*rigidBody.mass*(frameSize*frameSize/2f)
		);
	}
	
	public void setSize(float frameSize){
		this.frameSize = frameSize;
		float biggestDifference;
		int n = 0;
		for (int i = 0; i < mounts.Length; i++) {
			mounts[i].transform.localPosition = mountPos0 [i];
		}
		do {
			n++;
			biggestDifference = 0f;
			foreach (var mount in mounts) {
				foreach (var mount2 in mounts) {
					var difference = Vector3.Distance (mount.transform.position, mount2.transform.position);
					if (difference > biggestDifference) {
						biggestDifference = difference;
					}
				}
			}
			var newScale = frameSize / biggestDifference;
			for (int i = 0; i < mounts.Length; i++) {
				var mount = mounts [i];
				var arm = arms [i];
				var newMountPos = mount.transform.localPosition * newScale;
				mount.transform.localPosition = newMountPos;
				arm.center = newMountPos;
				arm.size = new Vector3 (newMountPos.x * 2f, arm.size.y, arm.size.z);
			}
		} while (n < 1000);
		
		var com = new Vector3 ();
		foreach (Prop prop in Props) {
			com += transform.position - prop.transform.position;
		}
		com /= Props.Length;
		rigidBody.centerOfMass = com;

		CalcluateInertiaTensor ();
	}

	float maxForce = 0f;
	public Prop[] Props;
	public GameObject[] mounts;
	public BoxCollider[] arms;
	public GameObject levelCenter;
	float[] motorSpeeds;
	float MAX_OUTPUT = 1000;
	public int pidController = 0;
	public int mixerIndex = 0;
	public float frameSize = .25f;
	public void SetPID (int pid)
	{
		pidController = pid;
		InitPidController ();
	}
	
	public PIDController PidController { get; private set; }
	
	public const int
		multiWiiClean = 0,
		rewriteClean = 1,
		luxFloat = 2,
		multiWii23 = 3,
		harakiri = 5;
	
	void InitPidController ()
	{
		switch (pidController) {
		case multiWiiClean:
			PidController = new PIDController.pidMultiWiiClean ();
			break;
		case rewriteClean:
			PidController = new PIDController.pidRewriteClean ();
			break;
		case luxFloat:
			PidController = new PIDController.pidLuxFloat ();
			break;
		case multiWii23:
			PidController = new PIDController.pidMultiWii23 ();
			break;
		case harakiri:
			PidController = new PIDController.pidHarakiri ();
			break;
		}
		var data = GetSaveData (PidController.SaveFileName, SaveName);
		PidController.LoadSaved (data);
	}

	void InitInputs ()
	{
		var data = GetSaveData (Inputs.SaveFileName, SaveName);
		Inputs.LoadSaved (data);
	}
	
	public void SetPropDiam (string value)
	{
		propDiam = float.Parse (value) / 39.3701f;
		propDiam = Mathf.Min (frameSize*.71f, propDiam);
		for (int i = 0; i < Props.Length; i++) {
			var scale = Props [i].transform.localScale;
			scale.x = propDiam0 [i].x * 7.5f * propDiam;
			scale.y = propDiam0 [i].y * 7.5f * propDiam;
			scale.z = propDiam0 [i].z * 100f * PITCH * propDiam;
			Props [i].transform.localScale = scale;
		}
	}
	
	public void SetPropRad (string value)
	{
		PITCH = float.Parse (value) / 39.3701f;
		PITCH = Mathf.Min (PITCH, propDiam * 1.8825f);
		for (int i = 0; i < Props.Length; i++) {
			var scale = Props [i].transform.localScale;
			scale.x = propDiam0 [i].x * 7.5f * propDiam;
			scale.y = propDiam0 [i].y * 7.5f * propDiam;
			scale.z = propDiam0 [i].z * 100f * PITCH * propDiam;
			Props [i].transform.localScale = scale;
		}
		calculateTerms ();
	}
	
	public void SetPropWeight (string value)
	{
		propMass = float.Parse (value) / 1000f;
		calculateTerms ();
	}
	
	public void SetMotorKv (string value)
	{
		z = float.Parse (value);
		calculateTerms ();
	}
	
	public void SetMotorRes (string value)
	{
		r = float.Parse (value);
		calculateTerms ();
	}
	
	public void SetMotorCurrent (string value)
	{
		i0 = float.Parse (value);
		calculateTerms ();
	}
	
	public void SetBatteryV (string value)
	{
		y = float.Parse (value);
	}
	
	public void SetFrameWeight (string value)
	{
		rigidBody.mass = float.Parse (value)*scale / (1000f);
		CalcluateInertiaTensor ();
	}
	
	public void SetAirResistance (string value)
	{
		rigidBody.drag = float.Parse (value);
	}
	
	public void SetSize (string value)
	{
		setSize (float.Parse (value)/1000f);
	}
	
	public void SetPropEfficiency (string value)
	{
		PROP_E = 1f / float.Parse (value);
		calculateTerms ();
	}
	
	public void SetVoltSag (string value)
	{
		x1 = -float.Parse (value);
		calculateTerms ();
	}
	
	public string GetPropDiam ()
	{
		return (propDiam * 39.3701f).ToString ();
	}
	
	public string GetPropRad ()
	{
		return (PITCH * 39.3701f).ToString ();
	}
	
	public string GetPropWeight ()
	{
		return (propMass * 1000f).ToString ();
	}
	
	public string GetMotorKv ()
	{
		return z.ToString ();
	}
	
	public string GetMotorRes ()
	{
		return r.ToString ();
	}
	
	public string GetMotorCurrent ()
	{
		return i0.ToString ();
	}
	
	public string GetBatteryV ()
	{
		return y.ToString ();
	}
	
	public string GetFrameWeight ()
	{
		return (rigidBody.mass * (1000f/scale)).ToString ();
	}
	
	public string GetAirResistance ()
	{
		return rigidBody.drag.ToString ();
	}
	
	public string GetSize ()
	{
		return ((frameSize)*1000f).ToString ();
	}
	
	public string GetPropEfficiency ()
	{
		return (1f / PROP_E).ToString ();
	}
	
	public string GetVoltSag ()
	{
		return (-x1).ToString ();
	}
	float PITCH = 0.1143f;
	float propDiam = 0.1524f;
	float PROP_E = 6f;
	float propMass = .003f;
	const float a = 0.00268041127787f;
	const float b = 0.0166666625433f;
	const float c = 1352;
	const float d = .00706f;
	float z = 1800;
	float i0 = .8f;
	float r = .43f;
	float h;
	float x1 = -.04f;
	float y = 11.1f;
	float K1, K2, K3, K4, K5, K6, K7, K8, K9, C1, C2, C3, C4;
	float airResistance = .005f;
	Int16[] axisPID = new Int16[3];
	public float maxV = 11.1f;
	public float totalCurrent;
	public byte brokenProp = 0;

	public float lapTime;

	float[,] mixer;

	float[] forces;

	public Vector3 interiaTensor;

	void Update(){
		for (int i = 0; i < forces.Length; i++) {
			var prop = Props [i];
			Debug.DrawRay (prop.transform.position, (transform.TransformDirection (new Vector3 (0f, forces[i], 0f))));
		}
		interiaTensor = rigidBody.inertiaTensor;
		Debug.DrawRay (transform.position, (transform.TransformDirection (new Vector3 (torque, 0, 0f))));
	}

	float torque;

	float scale = 1000;

	void FixedUpdate ()
	{
		if (!warmUp) {
			Profiler.BeginSample("PID Controller");
			var rcCommand = Inputs.RCInput;
			var inclination = Inputs.Angle;
			var gyroADC = Inputs.Gyro;
			var cycleTime = Inputs.CycleTime;
			PidController.GetPID (axisPID, inclination, rcCommand, gyroADC, cycleTime);
			float throttle = Inputs.RCInput [Constants.THROTTLE];
			float[] newMotorSpeeds = new float[mixer.GetLength (0)];
			for (int i = 0; i < newMotorSpeeds.Length; i++) {
				newMotorSpeeds [i] = throttle > 0 ? ((throttle * mixer [i, 0] + axisPID [Constants.PITCH] * mixer [i, 2] + axisPID [Constants.ROLL] * mixer [i, 1] - axisPID [Constants.YAW] * mixer [i, 3])) : 0f;
			}
		
			float overMax = 0f;
			for (int i = 0; i < newMotorSpeeds.Length; i++) {
				overMax = Mathf.Max (overMax, newMotorSpeeds [i] - MAX_OUTPUT);         
			}
		
			Profiler.EndSample ();
			Profiler.BeginSample ("Multirotor Physics");
			totalCurrent = 0f;
			torque = 0f;
			for (int i = 0; i < newMotorSpeeds.Length; i++) {
				float motorTorque;
				if (((brokenProp >> i) & 1) == 0) {
					var motorVelocityGlobal = rigidBody.velocity;
					Vector3 motorVelocity = transform.InverseTransformDirection (motorVelocityGlobal);
					var x = motorVelocity [Constants.YAW];
					var v = Mathf.Min (Mathf.Max (newMotorSpeeds [i] - overMax, 0f) / MAX_OUTPUT * y, maxV);
					newMotorSpeeds [i] = Mathf.Max ((Mathf.Sqrt (Mathf.Pow (x * K1 + C4, 2f) - K2 * (C1 - C2 * v - motorSpeeds [i] * C3)) + x * K3 - K4) / K5, 0f);
					forces [i] = Mathf.Max (0f, newMotorSpeeds [i] * K8 * (K9 * newMotorSpeeds [i] - x));
					motorTorque = (forces [i] * K7) + (newMotorSpeeds [i] - motorSpeeds [i]) * K6;
					motorSpeeds [i] = newMotorSpeeds [i];
					if (motorSpeeds [i] != 0) {
						Props [i].transform.Rotate (new Vector3 (0f, 0f, -mixer [i, 3] * motorSpeeds [i] * Time.fixedDeltaTime / 60f * 360f));
					}
					if (forces [i] > 0) {
						RigidBody.AddForceAtPosition (transform.TransformDirection (new Vector3 (0f, forces [i] * scale, 0f)), (Props [i].transform.position));
					}
					var current = (v - motorSpeeds [i] / z) / r;
					totalCurrent += Math.Max (current, 0f);
				} else {
					motorTorque = -motorSpeeds [i] * K6;
					motorSpeeds [i] = 0f;
				}
			
				if (motorTorque != 0f)
					addTorqueAtPosition (transform.TransformDirection (new Vector3 (0f, mixer [i, 3] * motorTorque * scale, 0f)), Props [i].transform.position, rigidBody);
			}
			if ((brokenProp != 0 || isCollided > 0 && Vector3.Dot(transform.up,Vector3.up) < 0) && RigidBody.IsSleeping ()) {
				brokenProp = 0;
				Reset ();
			}
			maxV = (y + x1 * totalCurrent);
			var levelDirection = transform.position - levelCenter.transform.position;
			var levelDistance = (levelDirection).magnitude;
			if (levelDistance > 600f) {
				transform.position += levelDirection / levelDistance * (600f - levelDistance);
			}
			Profiler.EndSample ();
		}
	}

	int isCollided;
	
	void OnCollisionEnter (Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) {
			if (mixer != null) {
				for (int i = 0; i < mixer.GetLength(0); i++) {
					if (contact.thisCollider == Props[i].GetComponent<BoxCollider>()) {
						if (Mathf.Abs(motorSpeeds[i]) > 4000 || rigidBody.velocity.sqrMagnitude > 240) brokenProp |= (byte)(1 << i);
					}
				}
				isCollided++;
			}
		}
	}
	
	void OnCollisionExit (Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) {
			isCollided--;
			return;
		}
	}

	public void Reset ()
	{
		Inputs.Reset ();
		if (Inputs.playerNum < 0) {
			Inputs.currentWaypoint = startPos;
			setAICourse ();
		} else {
			if (ResetAI) SetNumAI (multirotors.Count, this);
			if (course != null) {
				warmUp = true;
				lapTime = 3f;
			}
		}
		RigidBody.angularVelocity = Vector3.zero;
		RigidBody.velocity = Vector3.zero;
		transform.position = startPos;
		transform.rotation = startRot;
		axisPID = new Int16[3];
		maxV = 11.1f;
		totalCurrent = 0f;
		motorSpeeds = new float[mixer.GetLength(0)];
		InitPidController ();
		isCollided = 0;
		currentGate = null;
		gateIsEntered = 0;
		coursePosition = 0;
		gatesPassed = 0;
	}
	
	public void setAICourse (){
		Inputs.currentWaypoint.y += .5f;
		Inputs.nextWaypoint = Inputs.course.waypoints [0].transform.position;
		Inputs.waypoint = Inputs.course.waypoints.Length - 1;
	}
	
	public static List<Multirotor> multirotors = new List<Multirotor>();

	static public void SetNumAI (int numInt, Multirotor player)
	{
		foreach (var multirotor in multirotors) {
			Destroy (multirotor.gameObject);
		}
		multirotors.Clear ();
		while (multirotors.Count < numInt) {
			var multirotor = Instantiate (player.aiPrefab);
			Debug.Log (multirotor);
			multirotor.course = player.course;
			multirotor.Inputs.course = player.course.waypoints;
			multirotor.transform.position += multirotor.transform.right * 2 * multirotor.frameSize * multirotors.Count;
			multirotor.Inputs.currentWaypoint = multirotor.transform.position;
			multirotor.Inputs.SetSkill (UnityEngine.Random.Range (aiSkill - .5f, aiSkill + .5f));
			multirotor.setAICourse ();
			multirotor.warmUp = true;
			multirotor.lapTime = 3f;
			multirotor.levelCenter = player.levelCenter;
			multirotor.gameObject.SetActive (true);
			multirotors.Add (multirotor);
		}
	}


	public Multirotor aiPrefab;
	
	public static float aiSkill = 3f;
}
