using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class Configuration : MonoBehaviour
{
	public Toggle[] FlightMode;
	public Toggle[] PidController;
	public InputField[] P;
	public InputField[] I;
	public InputField[] D;
	public InputField[] Rates;
	public InputField[] Force;
	public InputField Limit;
	public Multirotor player;
	public InputField PropDiam;
	public InputField PropPitch;
	public InputField PropMass;
	public InputField MotorRes;
	public InputField MotorCurrent;
	public InputField MotorKV;
	public InputField Voltage;
	public InputField AllUpWeight;
	public InputField AirResistance;
	public InputField Size;
	public InputField[] Exp;
	public InputField[] Mins;
	public InputField[] Maxs;
	public EventSystem EventSys;
	public InputField SaveName;
	public InputField PropEfficiency;
	public InputField VoltSag;
	public InputField CamAngle;
	public InputField CamFOV;
	public InputField NumAI;
	public InputField AISkill;
	public Toggle[] OSDToggles;
	public OSD osd;
	public HideUI[] panels;
	public GameObject playerPrefab;
	public Camera fpvCam;
	public GameObject fpvCamAxis;
	public Camera losCam;
	public float camAngle;
	public float camFOV;
	int numAI = 0;

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

	public string LapTime {
		get {return (player.lapTime).ToString();}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape))
			GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
		
		if (player.course != null) {
			IncrementLapTime (player);
			foreach (var multirotor in Multirotor.multirotors) {
				IncrementLapTime(multirotor);
			}
		}
	}

	void IncrementLapTime (Multirotor multirotor)
	{
		if (multirotor.warmUp) {
			multirotor.lapTime -= Time.deltaTime;
			if (multirotor.lapTime <= 0f) {
				multirotor.warmUp = false;
				multirotor.lapTime = 0f;
			}
		}
		else {
			multirotor.lapTime += Time.deltaTime;
		}
	}
	
	public void SetNumAI(String num){
		try {
			numAI = int.Parse(num);
			NumAI.text = numAI.ToString();
			if (player.course != null) Multirotor.SetNumAI (numAI, player);
		} catch (FormatException) {
		}
	}
	
	public void SetAIReset(bool reset){
		Multirotor.ResetAI = reset;
	}
	
	public void SetAISkill(String skill) {
		Multirotor.aiSkill = float.Parse (skill);
		foreach (var multirotor in Multirotor.multirotors) {
			multirotor.Inputs.SetSkill(UnityEngine.Random.Range(Multirotor.aiSkill - .5f, Multirotor.aiSkill + .5f));
		}
		AISkill.text = Multirotor.aiSkill.ToString();
	}

	void startCountDown ()
	{
		player.Reset ();
	}

	public void HidePanel(int index){
		panels[index].OnHide();
		if (panels [index].Hidden) {
			hiddenPanels |= (byte) (1 << index);
		} else {
			hiddenPanels &= (byte) ~(1 << index);
		}
		data ["HIDDEN_PANELS"] = hiddenPanels;
		if (ShouldSave) data.Save ();
	}
	
	public void HideOSD(int index){
		Text text = null;
		switch (index) {
		case 0:
			osd.speed.enabled = !osd.speed.enabled;
			text = osd.speed;
			break;
		case 1:
			osd.voltage.enabled = !osd.voltage.enabled;
			text = osd.voltage;
			break;
		case 2:
			osd.current.enabled = !osd.current.enabled;
			text = osd.current;
			break;
		}
		if (!text.enabled) {
			hiddenOSD |= (byte) (1 << index);
		} else {
			hiddenOSD &= (byte) ~(1 << index);
		}
		data ["OSD"] = hiddenOSD;
		if (ShouldSave) data.Save ();
	}
	
	public void SetViewMode (int viewMode)
	{
		var lockAxis = losCam.GetComponent<LockAxis> ();
		switch (viewMode) {
		case 1:
			lockAxis.follow = true;
			losCam.GetComponent<DepthOfField>().enabled = false;
			losCam.gameObject.SetActive(true);
			fpvCam.gameObject.SetActive(false);
			break;
		case 2:
			lockAxis.follow = false;
			losCam.GetComponent<DepthOfField>().enabled = true;
			losCam.gameObject.SetActive(true);
			fpvCam.gameObject.SetActive(false);
			break;
		case 3:
			losCam.gameObject.SetActive(false);
			fpvCam.gameObject.SetActive(true);
			break;
		}
		if (ShouldSave) player.Save();
	}

	public Camera GetCamera ()
	{
		return fpvCam.gameObject.activeSelf ? fpvCam : losCam;
	}

	public void SetFlightMode (int flightMode)
	{
		player.PidController.SetFlightMode (flightMode);
		if (ShouldSave) player.Save();
	}
	
	public void SetPID (int pid)
	{
		player.SetPID (pid);
		if (ShouldSave) player.Save();
		Refresh ();
	}
	
	public void SetPPitch (string value)
	{
		try {
			player.PidController.SetPID (value, 0, Constants.PITCH);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		P [Constants.PITCH].text = player.PidController.GetPID (0, Constants.PITCH);
	}
	
	public void SetIPitch (string value)
	{
		try {
			player.PidController.SetPID (value, 1, Constants.PITCH);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		I [Constants.PITCH].text = player.PidController.GetPID (1, Constants.PITCH);
	}
	
	public void SetDPitch (string value)
	{
		try {
			player.PidController.SetPID (value, 2, Constants.PITCH);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		D [Constants.PITCH].text = player.PidController.GetPID (2, Constants.PITCH);
	}
	
	public void SetPYaw (string value)
	{
		try {
			player.PidController.SetPID (value, 0, Constants.YAW);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		P [Constants.YAW].text = player.PidController.GetPID (0, Constants.YAW);
	}
	
	public void SetIYaw (string value)
	{
		try {
			player.PidController.SetPID (value, 1, Constants.YAW);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		I [Constants.YAW].text = player.PidController.GetPID (1, Constants.YAW);
	}
	
	public void SetDYaw (string value)
	{
		try {
			player.PidController.SetPID (value, 2, Constants.YAW);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		D [Constants.YAW].text = player.PidController.GetPID (2, Constants.YAW);
	}
	
	public void SetPRoll (string value)
	{
		try {
			player.PidController.SetPID (value, 0, Constants.ROLL);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		P [Constants.ROLL].text = player.PidController.GetPID (0, Constants.ROLL);
	}
	
	public void SetIRoll (string value)
	{
		try {
			player.PidController.SetPID (value, 1, Constants.ROLL);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		I [Constants.ROLL].text = player.PidController.GetPID (1, Constants.ROLL);
	}
	
	public void SetDRoll (string value)
	{
		try {
			player.PidController.SetPID (value, 2, Constants.ROLL);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		D [Constants.ROLL].text = player.PidController.GetPID (2, Constants.ROLL);
	}
	
	public void SetRatePitch (string value)
	{
		try {
			player.PidController.SetRate (value, Constants.PITCH);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Rates [Constants.PITCH].text = player.PidController.GetRate (Constants.PITCH);
	}
	
	public void SetRateYaw (string value)
	{
		try {
			player.PidController.SetRate (value, Constants.YAW);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Rates [Constants.YAW].text = player.PidController.GetRate (Constants.YAW);
	}
	
	public void SetRateRoll (string value)
	{
		try {
			player.PidController.SetRate (value, Constants.ROLL);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Rates [Constants.ROLL].text = player.PidController.GetRate (Constants.ROLL);
	}
	
	public void SetRateHorizon (string value)
	{
		try {
			player.PidController.SetRate (value, 3);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Rates [3].text = player.PidController.GetRate (3);
	}
	
	public void SetForceHorizon (string value)
	{
		try {
			player.PidController.SetForce (value, 0);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Force [0].text = player.PidController.GetForce (0);
	}
	
	public void SetForceAngle (string value)
	{
		try {
			player.PidController.SetForce (value, 1);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Force [1].text = player.PidController.GetForce (1);
	}
	
	public void SetForceP (string value)
	{
		try {
			player.PidController.SetForce (value, 2);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Force [2].text = player.PidController.GetForce (2);
	}
	
	public void SetForceI (string value)
	{
		try {
			player.PidController.SetForce (value, 3);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Force [3].text = player.PidController.GetForce (3);
	}
	
	public void SetLevelLimit (string value)
	{
		try {
			player.PidController.SetLevelLimit (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Limit.text = player.PidController.GetLevelLimit ();
	}
	
	public void SetExpPitch (string value)
	{
		try {
			player.Inputs.SetExp (value, Constants.PITCH);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Exp[Constants.PITCH].text = player.Inputs.GetExp (Constants.PITCH);
	}
	
	public void SetExpYaw (string value)
	{
		try {
			player.Inputs.SetExp (value, Constants.YAW);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Exp[Constants.YAW].text = player.Inputs.GetExp (Constants.YAW);
	}
	
	public void SetExpRoll (string value)
	{
		try {
			player.Inputs.SetExp (value, Constants.ROLL);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Exp[Constants.ROLL].text = player.Inputs.GetExp (Constants.ROLL);
	}
	
	public void SetExpThrottle (string value)
	{
		try {
			player.Inputs.SetExp (value, Constants.THROTTLE);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Exp[Constants.THROTTLE].text = player.Inputs.GetExp (Constants.THROTTLE);
	}
	
	public void SetMinPitch (string value)
	{
		try {
			player.Inputs.SetMin (value, Constants.PITCH);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Mins[Constants.PITCH].text = player.Inputs.GetMin (Constants.PITCH);
	}
	
	public void SetMinYaw (string value)
	{
		try {
			player.Inputs.SetMin (value, Constants.YAW);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Mins[Constants.YAW].text = player.Inputs.GetMin (Constants.YAW);
	}
	
	public void SetMinRoll (string value)
	{
		try {
			player.Inputs.SetMin (value, Constants.ROLL);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Mins[Constants.ROLL].text = player.Inputs.GetMin (Constants.ROLL);
	}
	
	public void SetMinThrottle (string value)
	{
		try {
			player.Inputs.SetMin (value, Constants.THROTTLE);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Mins[Constants.THROTTLE].text = player.Inputs.GetMin (Constants.THROTTLE);
	}
	
	public void SetMaxThrottle (string value)
	{
		try {
			player.Inputs.SetMax (value, Constants.THROTTLE);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Maxs[Constants.THROTTLE].text = player.Inputs.GetMax (Constants.THROTTLE);
	}
	
	public void SetMaxYaw (string value)
	{
		try {
			player.Inputs.SetMax (value, Constants.YAW);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Maxs[Constants.YAW].text = player.Inputs.GetMax (Constants.YAW);
	}
	
	public void SetMaxRoll (string value)
	{
		try {
			player.Inputs.SetMax (value, Constants.ROLL);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Maxs[Constants.ROLL].text = player.Inputs.GetMax (Constants.ROLL);
	}
	
	public void SetMaxPitch (string value)
	{
		try {
			player.Inputs.SetMax (value, Constants.PITCH);
			if (ShouldSave) player.Save ();
		} catch (FormatException) {
		}
		Maxs[Constants.PITCH].text = player.Inputs.GetMax (Constants.PITCH);
	}
	
	public void SetPropDiam (string value)
	{
		try {
			player.SetPropDiam (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		PropDiam.text = player.GetPropDiam ();
	}
	
	public void SetPropRad (string value)
	{
		try {
			player.SetPropRad (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		PropPitch.text = player.GetPropRad ();
	}
	
	public void SetPropWeight (string value)
	{
		try {
			player.SetPropWeight (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		PropMass.text = player.GetPropWeight ();
	}
	
	public void SetMotorKv (string value)
	{
		try {
			player.SetMotorKv (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		MotorKV.text = player.GetMotorKv ();
	}
	
	public void SetMotorRes (string value)
	{
		try {
			player.SetMotorRes (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		MotorRes.text = player.GetMotorRes ();
	}
	
	public void SetMotorCurrent (string value)
	{
		try {
			player.SetMotorCurrent (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		MotorCurrent.text = player.GetMotorCurrent ();
	}
	
	public void SetBatteryV (string value)
	{
		try {
			player.SetBatteryV (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Voltage.text = player.GetBatteryV ();
	}
	
	public void SetFrameWeight (string value)
	{
		try {
			player.SetFrameWeight (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		AllUpWeight.text = player.GetFrameWeight ();
	}
	
	public void SetAirResistance (string value)
	{
		try {
			player.SetAirResistance (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		AirResistance.text = player.GetAirResistance ();
	}
	
	public void SetSize (string value)
	{
		try {
			player.SetSize (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		Size.text = player.GetSize ();
	}
	
	public void SetPropEfficiency (string value)
	{
		try {
			player.SetPropEfficiency (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		PropEfficiency.text = player.GetPropEfficiency ();
	}
	
	public void SetVoltSag (string value)
	{
		try {
			player.SetVoltSag (value);
			if (ShouldSave) player.Save();
		} catch (FormatException) {
		}
		VoltSag.text = player.GetVoltSag ();
	}

	void UpdateCamFOV ()
	{
		fpvCam.fieldOfView = camFOV;
		for (int i = 0; i < fpvCam.transform.childCount; i++) {
			var child = fpvCam.transform.GetChild (i).GetComponent<Camera> ();
			if (child != null)
				child.fieldOfView = camFOV;
		}
	}
	
	public void SetCamFOV (string value)
	{
		try {
			camFOV = float.Parse (value);
			UpdateCamFOV ();
			data ["CAM_FOV"] = camFOV;
			if (ShouldSave) data.Save();
		} catch (FormatException) {
		}
		CamFOV.text = camFOV.ToString();
	}
	
	public void SetCamAngle (string value)
	{
		try {
			camAngle = float.Parse (value);
			fpvCamAxis.transform.localEulerAngles = new Vector3 (-camAngle, 0f, 0f);
			data ["CAM_ANGLE"] = camAngle;
			if (ShouldSave) data.Save();
		} catch (FormatException) {
		}
		CamAngle.text = camAngle.ToString();
	}

	public void Reset(){
		player.Reset ();
	}

	void Refresh ()
	{
		P [Constants.PITCH].text = player.PidController.GetPID (0, Constants.PITCH);
		I [Constants.PITCH].text = player.PidController.GetPID (1, Constants.PITCH);
		D [Constants.PITCH].text = player.PidController.GetPID (2, Constants.PITCH);
		P [Constants.YAW].text = player.PidController.GetPID (0, Constants.YAW);
		I [Constants.YAW].text = player.PidController.GetPID (1, Constants.YAW);
		D [Constants.YAW].text = player.PidController.GetPID (2, Constants.YAW);
		P [Constants.ROLL].text = player.PidController.GetPID (0, Constants.ROLL);
		I [Constants.ROLL].text = player.PidController.GetPID (1, Constants.ROLL);
		D [Constants.ROLL].text = player.PidController.GetPID (2, Constants.ROLL);
		Rates [Constants.PITCH].text = player.PidController.GetRate (Constants.PITCH);
		Rates [Constants.YAW].text = player.PidController.GetRate (Constants.YAW);
		Rates [Constants.ROLL].text = player.PidController.GetRate (Constants.ROLL);
		Rates [3].text = player.PidController.GetRate (3);
		Force [0].text = player.PidController.GetForce (0);
		Force [1].text = player.PidController.GetForce (1);
		Force [2].text = player.PidController.GetForce (2);
		Force [3].text = player.PidController.GetForce (3);
		Limit.text = player.PidController.GetLevelLimit ();
		Exp[Constants.PITCH].text = player.Inputs.GetExp (Constants.PITCH);
		Exp[Constants.YAW].text = player.Inputs.GetExp (Constants.YAW);
		Exp[Constants.ROLL].text = player.Inputs.GetExp (Constants.ROLL);
		Exp[Constants.THROTTLE].text = player.Inputs.GetExp (Constants.THROTTLE);
		Mins[Constants.PITCH].text = player.Inputs.GetMin (Constants.PITCH);
		Mins[Constants.YAW].text = player.Inputs.GetMin (Constants.YAW);
		Mins[Constants.ROLL].text = player.Inputs.GetMin (Constants.ROLL);
		Mins[Constants.THROTTLE].text = player.Inputs.GetMin (Constants.THROTTLE);
		Maxs[Constants.THROTTLE].text = player.Inputs.GetMax (Constants.THROTTLE);
		Maxs[Constants.YAW].text = player.Inputs.GetMax (Constants.YAW);
		Maxs[Constants.ROLL].text = player.Inputs.GetMax (Constants.ROLL);
		Maxs[Constants.PITCH].text = player.Inputs.GetMax (Constants.PITCH);
		PropDiam.text = player.GetPropDiam ();
		PropPitch.text = player.GetPropRad ();
		PropMass.text = player.GetPropWeight ();
		MotorKV.text = player.GetMotorKv ();
		MotorRes.text = player.GetMotorRes ();
		MotorCurrent.text = player.GetMotorCurrent ();
		Voltage.text = player.GetBatteryV ();
		AllUpWeight.text = player.GetFrameWeight ();
		AirResistance.text = player.GetAirResistance ();
		Size.text = player.GetSize ();
		PropEfficiency.text = player.GetPropEfficiency ();
		VoltSag.text = player.GetVoltSag ();
		CamFOV.text = camFOV.ToString ();
		CamAngle.text = camAngle.ToString ();
		FlightMode [player.PidController.flightMode].isOn = true;
		NumAI.text = numAI.ToString();
		AISkill.text = Multirotor.aiSkill.ToString();

	}

	SaveData data;

	byte hiddenOSD = 0;

	void Start ()
	{
		ShouldSave = false;
		data = Multirotor.GetSaveData ("PREF_CONFIG", "");
		string saveFile = null;
		if (data.HasKey ("SAVE_FILE_NAME")) {
			saveFile = data.GetValue<string> ("SAVE_FILE_NAME");
		}
		if (data.HasKey ("HIDDEN_PANELS")) {
			hiddenPanels = data.GetValue<byte> ("HIDDEN_PANELS");
		}
		if (data.HasKey ("OSD")) {
			hiddenOSD = data.GetValue<byte> ("OSD");
		}
		LoadCamera ();
		for (int i = 0; i < panels.Length; i++) {
			if (((hiddenPanels >> i) & 1) == 1) {
				panels[i].OnHide();
			}
		}
		for (int i = 0; i < 3; i++) {
			if ((((hiddenOSD) >> i ) & 1) == 1) {
				OSDToggles[i].isOn = false;
			}
		}
		PopulateList (saveFile);
		PidController [player.pidController].isOn = true;
		Refresh ();
		ShouldSave = true;
	}
	
	public GameObject profileSample;
	public Transform contentPanel;
	public Transform coursePanel;

	public Course[] courses;
	
	public ToggleGroup profileToggleGroup;
	public ToggleGroup courseToggleGroup;
	bool ShouldSave = true;
	byte hiddenPanels = 0;
	
	void PopulateList (string saveFile) {
		Toggle lastToggle = null;
		for (int i = 0; i < contentPanel.childCount; i++) {
			Destroy (contentPanel.GetChild(i).gameObject);
		}
		var fileInfo = new DirectoryInfo (Application.streamingAssetsPath).GetFiles();
		foreach (var file in fileInfo) {
			if (file.Name.ToUpper().StartsWith(player.SaveFileName + "_") && file.Extension.ToUpper().Equals(".UML")){
				var name = file.Name.Remove(file.Name.Length - 4, 4).Remove (0, player.SaveFileName.Length + 1);
				if (name.Length > 0){
					GameObject newButton = Instantiate (profileSample) as GameObject;
					ProfileItem button = newButton.GetComponent <ProfileItem> ();
					button.label.text = name;
					button.toggle.group = profileToggleGroup;
					newButton.transform.SetParent (contentPanel);
					button.toggle.onValueChanged.AddListener((isOn) => SetSaveFile(isOn, name));
					if (lastToggle == null || name == saveFile) {
						lastToggle = button.toggle;
					}
				}
			}
		}
		if (lastToggle == null) {
			SaveName.text = "Default";
			NewSaveFile ();
		} else {
			lastToggle.isOn = true;
		}

		for (int i = 0; i < courses.Length; i++) {
			Course course = courses [i];
			GameObject newButton = Instantiate (profileSample) as GameObject;
			ProfileItem button = newButton.GetComponent<ProfileItem> ();
			button.label.text = course.name;
			button.toggle.group = courseToggleGroup;
			newButton.transform.SetParent (coursePanel);
			button.toggle.onValueChanged.AddListener (isOn => SetCourse (isOn, course));
		}

	}

	void SetCourse (bool isOn, Course course)
	{
		if (isOn) {
			course.gameObject.SetActive(true);
			player.course = course;
			player.Reset();
			Multirotor.SetNumAI (numAI, player);
			startCountDown ();
		} else {
			course.gameObject.SetActive(false);
			Multirotor.SetNumAI (0, player);
			player.course = null;
		}
	}
	
	public void SetSaveFile (bool isOn, string name) {
		if (isOn) {
			ShouldSave = false;
			player.SaveName = name;
			player.LoadSaved ();
			LoadCamera ();
			PidController [player.pidController].isOn = true;
			Refresh ();
			ShouldSave = true;
			data ["SAVE_FILE_NAME"] = name;
			data.Save ();
		}
	}

	static bool HasInvalidChars (string text)
	{
		foreach (var c in Path.GetInvalidFileNameChars ()) {
			if (text.Contains (c.ToString ())) {
				return true;
			}
		}
		return false;
	}

	bool FileExists (string text)
	{
		var fileInfo = new DirectoryInfo (Application.streamingAssetsPath).GetFiles();
		foreach (var file in fileInfo) {
			if (file.Name.ToUpper ().StartsWith (player.SaveFileName + "_") && file.Extension.ToUpper ().Equals (".UML")) {
				var name = file.Name.Remove (file.Name.Length - 4, 4).Remove (0, 16);
				if (name == text) return true;
			}
		}
		return false;
	}
	
	public void NewSaveFile () {
		if (HasInvalidChars (SaveName.text)) {
			return;
		}
		var text = SaveName.text;
		if (FileExists (text)) {
			return;
		}
		ShouldSave = false;
		player.SaveName = "";
		player.LoadSaved ();
		LoadCamera ();
		player.SaveName = text;
		player.Save ();
		PopulateList (text);
		PidController [player.pidController].isOn = true;
		Refresh ();
		SaveName.text = "";
		ShouldSave = true;
	}
	
	public void CopySaveFile () {
		if (HasInvalidChars (SaveName.text)) {
			return;
		}
		var text = SaveName.text;
		if (FileExists (text)) {
			return;
		}
		ShouldSave = false;
		var lastPid = player.pidController;
		var lastSaveName = player.SaveName.Remove(0,1);
		for (int i = 0; i < 6; i++) {
			player.SaveName = lastSaveName;
			player.SetPID(i);
			player.SaveName = text;
			player.Save ();
		}
		player.SetPID(lastPid);
		player.Save ();
		PopulateList (text);
		PidController [player.pidController].isOn = true;
		Refresh ();
		SaveName.text = "";
		ShouldSave = true;
	}

	public void DeleteFile () {
		if (contentPanel.childCount <= 1)
			return;
		ShouldSave = false;
		for (int i = 0; i < 6; i++) {
			player.SetPID(i);
			File.Delete(Path.Combine(Application.streamingAssetsPath, player.PidController.SaveFileName + player.SaveName + ".uml"));
		}
		File.Delete(Path.Combine(Application.streamingAssetsPath, player.SaveFileName + player.SaveName + ".uml"));
		File.Delete(Path.Combine(Application.streamingAssetsPath, player.Inputs.SaveFileName + player.SaveName + ".uml"));
		PopulateList (null);
		PidController [player.pidController].isOn = true;
		Refresh ();
		ShouldSave = true;
	}

	void LoadCamera ()
	{
		if (data.HasKey ("CAM_ANGLE"))
			camAngle = data.GetValue<float> ("CAM_ANGLE");
		if (data.HasKey ("CAM_FOV"))
			camFOV = data.GetValue<float> ("CAM_FOV");
		UpdateCamFOV ();
		fpvCamAxis.transform.localEulerAngles = new Vector3 (-camAngle, 0f, 0f);
	}
}
