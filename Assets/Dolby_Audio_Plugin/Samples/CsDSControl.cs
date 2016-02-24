using UnityEngine;
using System.Collections;

using System.Runtime.InteropServices;

public class CsDSControl : MonoBehaviour {
	// C#Script Option Button Size
	private float optionButtonWidth = 150.0f;
	private float optionButtonHeight = 50.0f;
	
	private bool isMenuPopUp = false;
	
	// Pop-Up Menu size
	private float menuWidth = 250.0f;
	private float menuHeight = 500.0f;
	private float menuButtonWidth = 250.0f;
	private float menuButtonHeight = 50.0f;
	
	// Dolby Profiles
	private enum PROFILE
	{
	    MOVIE = 0,
		MUSIC = 1,
		GAME = 2,
		VOICE = 3
	};
	
	// Buttons Names on Menu List.
	private string strDLBAvail = "DolbyAudioProcessing Availability";
	private string strDLBInit = "Initialize";
	private string strSetPro = "Set Profile";
	private string strPauseDS = "Suspend Session";
	private string strRestoreDS = "Restart Session";
	private string strReleaseDS = "Release";
	
	// Index of Profile id
	//private int curGetProfileId = -1;
	//private int curSetProfileId = -1;
	private PROFILE curProfile = PROFILE.MOVIE;
	
	// Window for printing logs
	private Rect logWindowRect = new Rect(20.0f, 0.0f, 300.0f, 200.0f);
	private bool bShowWindow = false;
	private string profileName = "";
	private string logText = "";
	
	// Import functions from libDSPlugin.so
	[DllImport("DSPlugin")]
	public static extern  bool isAvailable();
	[DllImport("DSPlugin")]
	public static extern  int initialize();
    [DllImport("DSPlugin")]
	public static extern  int setProfile(int profileid);
	[DllImport("DSPlugin")]
	public static extern  int suspendSession();
	[DllImport("DSPlugin")]
	public static extern  int restartSession();
	[DllImport("DSPlugin")]
	public static extern void release();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// OnGUI is called for rendering and handling GUI events.
	void OnGUI () {
		// Draw Option Button on the right middle.
		Rect optionButtonRect = new Rect (Screen.width - optionButtonWidth, (Screen.height - optionButtonHeight) / 2 + optionButtonHeight, optionButtonWidth, optionButtonHeight);
        if (GUI.Button(optionButtonRect, "CS Option Menu")) {
			isMenuPopUp = !isMenuPopUp;
		}
		
		if (!isMenuPopUp) {
			return;
		} else {
			Rect menuRect = new Rect((Screen.width - menuWidth) * 0.5f,
									(Screen.height - menuHeight) * 0.5f,
									menuWidth,
									menuHeight);
			
			GUILayout.BeginArea(menuRect);
				// Whether DS is available.
				if (MenuButton(strDLBAvail)) {
					bool bAvail = isAvailable();
					
					bShowWindow = true;
					logText = strDLBAvail + "\n [Return]: " + bAvail;
					LOGI(logText);
				}
				
				// Whether DS has been initialized.
				if (MenuButton(strDLBInit)) {
					int ret = initialize();
					
					bShowWindow = true;
					logText = strDLBInit + "\n [Return]: " + ret;
					LOGI(logText);
				}
				
				// Set DS profile 
				if (MenuButton(strSetPro)) {

				    switch(curProfile){
				    case PROFILE.MOVIE:
				    case PROFILE.MUSIC:
				    case PROFILE.GAME:
				    case PROFILE.VOICE:
					    break;
					default:
					    curProfile = PROFILE.MOVIE;
					    break;
					}
					
					int ret = setProfile((int)curProfile);
					bShowWindow = true;
				    profileName = curProfile.ToString();
					logText = strSetPro + "\n[Index]: " + (int)curProfile + " (" + profileName + ")" + "\n[Return]: " + ret;
					LOGI(logText);
					
					curProfile = curProfile + 1;

				}
				
				// Pause DS.
				// Only for testing here. Maybe, it should implemented in "OnApplicationPause()" function.
				if (MenuButton(strPauseDS)) {
					int ret = suspendSession();
					
					bShowWindow = true;
					logText = strPauseDS + "\n [Return]: " + ret;
					LOGI(logText);
				}
				
				// Restore DS.
				// Only for testing here. Maybe, it should implemented in "OnApplicationFocus()" function.
				if (MenuButton(strRestoreDS)) {
					int ret = restartSession();
					
					bShowWindow = true;
					logText = strRestoreDS + "\n [Return]: " + ret;
					LOGI(logText);
				}
				
				// Release DS.
				// Only for testing here. Maybe, it should implemented in "OnApplicationQuit()" function.
				if (MenuButton(strReleaseDS)) {
					release();
					
					bShowWindow = true;
					logText = strReleaseDS;
					LOGI(logText);
				}
			GUILayout.EndArea();
			
			// Used for printing log on UI.
			if (bShowWindow) {
				showLogWindow();
			}
		}
	}
	
	// MenuButton
	bool MenuButton (string btnName) {
		bool wasPressed = false;
		
		// Starts a horizontal group
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUIStyle btnStyle = GUI.skin.GetStyle("Button");
		if (GUI.Button(GUILayoutUtility.GetRect(menuButtonWidth, menuButtonHeight), btnName, btnStyle)) {
			wasPressed = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		return wasPressed;
	}
	
	// Show logText On UI
	void WindowFunction (int windowID) {
		// Draw any Controls inside the window here
		float rectWidth = logWindowRect.width * 0.8f;
		float rectHeight = logWindowRect.height / 3;
		
		GUILayout.FlexibleSpace();
		
		// Text Box
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUI.Box(GUILayoutUtility.GetRect(rectWidth, rectHeight), logText);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		// Button
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUI.Button(GUILayoutUtility.GetRect(rectWidth, rectHeight), "OK")) {
			LOGI("WindowFunction(), OK Button has been clicked!");
			bShowWindow = false;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
	}
	
	// Show GUI Log Window
	void showLogWindow() {
		GUI.Window(0, logWindowRect, WindowFunction, "Log Window");
	}
	
	private static void LOGI(string str) {
		Debug.Log(str);
	}
	
	// Overridable Base Class Functions
	// TIPS: All of the follow 3 functions will be invoked by game automatically!!!
	
	// Sent to all game objects when the player pauses.
	void OnApplicationPause(bool pauseStatus) {
		// TODO: We can invoke "suspendSession()" function, while game in pause state.
		//if (pauseStatus) {
		//    int ret = suspendSession();
		//	LOGI("void OnApplicationPause(bool pauseStatus), suspendSession = " + ret);
		//}
		LOGI("void OnApplicationPause(bool pauseStatus), pauseStatus = " + pauseStatus);
	}
	
	// Sent to all game objects when the player gets or looses focus.
	void OnApplicationFocus(bool focusStatus) {
		// TODO: We can invoke "restartSession()" function, while game gets focus(the value of "focus" should be "true").
		// The restartSession API paired for the suspendSession API and it returned -1 and did nothing if the suspendSession API was not called fristly.
		//if (focusStatus) {
		//    int ret = restartSession();
		//	LOGI("void OnApplicationFocus(bool focusStatus), restartSession = " + ret);
		//}
		LOGI("void OnApplicationFocus(bool focusStatus), focusStatus = " + focusStatus);
	}
	
	// Sent to all game objects before the application is quit.
	void OnApplicationQuit() {
		// TODO: We can invoke "release()" function, while game begins to quit.
		//release();
		LOGI("void OnApplicationQuit()");
	}
}
