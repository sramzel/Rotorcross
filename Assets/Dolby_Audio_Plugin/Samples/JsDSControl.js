import System.Runtime.InteropServices;

#pragma strict

class JsDSControl extends MonoBehaviour
{
	// JavaScript Option Button Size
	private var optionButtonWidth = 150.0f;
	private var optionButtonHeight = 50.0f;
	
	private var isMenuPopUp = false;
	
	// Pop-Up Menu size
	private var menuWidth = 250.0f;
	private var menuHeight = 500.0f;
	private var menuButtonWidth = 250.0f;
	private var menuButtonHeight = 50.0f;
	
	// Dolby Profiles
	private enum PROFILE
	{
	    MOVIE = 0,
		MUSIC = 1,
		GAME = 2,
		VOICE = 3
	}; 
	
	// Buttons Names on Menu List.
	private var strDLBAvail = "DolbyAudioProcessing Availability";
	private var strDLBInit = "Initialize";
	private var strSetPro = "Set Profile";
	private var strPauseDS = "Suspend Session";
	private var strRestoreDS = "Restart Session";
	private var strReleaseDS = "Release";
	
	// Index of Profile id
	//private var curGetProfileId = -1;
	private var curSetProfileId = -1;
	private var curProfile = PROFILE.MOVIE;
	
	// Window for printing logs
	private var logWindowRect = new Rect(20.0f, 0.0f, 300.0f, 200.0f);
	private var bShowWindow = false;
	private var profileName = "";
	private var logText = "";
	private var profileCount = 0;
	private var ret = 0;
	
	// Import functions from libDSPlugin.so
	@DllImport ("DSPlugin")
	static private function isAvailable() : boolean {};
	@DllImport ("DSPlugin")
	static private function initialize() : int {};
	@DllImport ("DSPlugin")
	static private function setProfile(profileid : int) : int {};
	@DllImport ("DSPlugin")
	static private function suspendSession() : int {};
	@DllImport ("DSPlugin")
	static private function restartSession() : int {};
	@DllImport ("DSPlugin")
	static private function release() {};
	
	// Initializes the target variable.
    function Start () {
    }
	
	// Moves the object forward 1 meter a second
    function Update () {
    }

	function OnGUI () {
		// Draw Option Button on the right middle.
		var optionButtonRect : Rect = new Rect (Screen.width - optionButtonWidth, (Screen.height - optionButtonHeight) / 2 - optionButtonHeight, optionButtonWidth, optionButtonHeight);
        if (GUI.Button (optionButtonRect, "JS Option Menu")) {
			isMenuPopUp = !isMenuPopUp;
		}
		
		if (!isMenuPopUp) {
			return;
		} else {
			var menuRect : Rect = new Rect((Screen.width - menuWidth) * 0.5f,
											(Screen.height - menuHeight) * 0.5f,
											menuWidth,
											menuHeight);
			GUILayout.BeginArea(menuRect);
				// Whether DS is available.
				if (MenuButton(strDLBAvail)) {
					var bAvail : boolean = isAvailable();
					
					bShowWindow = true;
					logText = strDLBAvail + "\n [Return]: " + bAvail;
					LOGI(logText);
				}	
				// Whether DS has been initialized.
				if (MenuButton(strDLBInit)) {
					ret = initialize();
					
					bShowWindow = true;
					logText = strDLBInit + "\n [Return]: " + ret;
					LOGI(logText);
				}
				
				// Set DS profile.
				if (MenuButton(strSetPro)) {
					
					switch(curProfile)
					{
					case curProfile.MOVIE:
					case curProfile.MUSIC:
					case curProfile.GAME:
					case curProfile.VOICE:
					    break;
					default:
					    curProfile = curProfile.MOVIE;
						break;
					}
					ret = setProfile(curProfile);
						
					bShowWindow = true;
					profileName = curProfile.ToString();
					curSetProfileId = curProfile;
					logText = strSetPro + "\n[Index]: " + curSetProfileId + " (" + profileName + ")" + "\n[Return]: " + ret;
					LOGI(logText);
					
					curProfile = curProfile + 1;
				}
				
				// Pause DS.
				// Only for testing here. Maybe, it should implemented in "OnApplicationPause()" function.
				if (MenuButton(strPauseDS)) {
					ret = suspendSession();
					
					bShowWindow = true;
					logText = strPauseDS + "\n [Return]: " + ret;
					LOGI(logText);
				}
				
				// Restore DS.
				// Only for testing here. Maybe, it should implemented in "OnApplicationFocus()" function.
				if (MenuButton(strRestoreDS)) {
					ret = restartSession();
					
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
	function MenuButton (btnName : String) : boolean {
		var wasPressed : boolean = false;
		
		// Starts a horizontal group
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUI.Button(GUILayoutUtility.GetRect(menuButtonWidth, menuButtonHeight), btnName)) {
			wasPressed = true;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		return wasPressed;
	}
	
	function LOGI(str : String) : void {
		Debug.Log(str);
	}
	
	// Show logText On UI
	function WindowFunction (windowID : int) : void  {
		// Draw any Controls inside the window here
		var rectWidth = logWindowRect.width * 0.8f;
		var rectHeight = logWindowRect.height / 3;
		
		GUILayout.FlexibleSpace();
		
		// Text Box
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		var btnStyle = GUI.skin.GetStyle("Button");
		GUI.Box(GUILayoutUtility.GetRect(rectWidth, rectHeight), logText, btnStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();
		
		// OK Button
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
	function showLogWindow() {
		GUI.Window(0, logWindowRect, WindowFunction, "Log Window");
	}
	
	// Overridable Base Class Functions
	// TIPS: All of the follow 3 functions will be invoked by game automatically!!!
	
	// Sent to all game objects when the player pauses.
	function OnApplicationPause(pauseStatus: boolean) {
		// TODO: We can invoke "suspendSession()" function, while game in pause state.
		//if (pauseStatus) {
		//    var ret = suspendSession();
		//	LOGI("function OnApplicationPause(pauseStatus: boolean), suspendSession = " + ret);
		//}
		LOGI("function OnApplicationPause(pauseStatus: boolean), pauseStatus = " + pauseStatus);
	}
	
	// Sent to all game objects when the player gets or looses focus.
	function OnApplicationFocus(focusStatus: boolean) {
		// TODO: We can invoke "restartSession()" function, while game gets focus(the value of "focus" should be "true").
		// The restartSession API paired for the suspendSession API and it returned -1 and did nothing if the suspendSession API was not called fristly.
		//if (focusStatus) {
		//    var ret = restartSession();
		//	LOGI("function OnApplicationFocus(focusStatus: boolean), restartSession = " + ret);
		//}
		LOGI("function OnApplicationFocus(focusStatus: boolean), focusStatus = " + focusStatus);
	}
	
	// Sent to all game objects before the application is quit.
	function OnApplicationQuit() {
		// TODO: We can invoke "release()" function, while game begins to quit.
		//release();
		LOGI("function OnApplicationQuit()");
	}

}
