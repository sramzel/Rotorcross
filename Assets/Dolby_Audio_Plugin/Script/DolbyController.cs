using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices; //Allows us to use DLLImport

public class DolbyController : MonoBehaviour
{

  #if UNITY_ANDROID && !UNITY_EDITOR
  [DllImport("DSJavaPlugin")]
  public static extern bool isAvailable();
  [DllImport("DSJavaPlugin")]
  public static extern int initialize();
  [DllImport("DSJavaPlugin")]
  public static extern int setProfile(int profileid);
  [DllImport("DSJavaPlugin")]
  public static extern int suspendSession();
  [DllImport("DSJavaPlugin")]
  public static extern int restartSession();
  [DllImport("DSJavaPlugin")]
  public static extern void release();

  void Start() {
    if (isAvailable()) {
      InitDolby(10);
    }
  }

  void InitDolby(int limit) {
    if (initialize() > -1) {
      setProfile(2); /* Set Profile to "Game" */
    } else if (limit > 0) {
      limit --;
      StartCoroutine(Delay(limit));
    }
  }

  IEnumerator Delay(int limit){
    // Wait 100ms to make sure Dolby service is enabled
    yield return new WaitForSeconds(0.1f);
    InitDolby(limit);
  }

  void OnApplicationPause(bool pauseStatus) {
    if (!pauseStatus) return;
    suspendSession();
  }

  void OnApplicationFocus(bool focusStatus) {
    if (!focusStatus) return;
    restartSession();
  }

  void OnApplicationQuit() {
    release();
  }
  #endif
}