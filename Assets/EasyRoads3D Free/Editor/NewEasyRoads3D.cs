using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using EasyRoads3D;
public class NewEasyRoads3D : EditorWindow
{
public static NewEasyRoads3D instance;
private Vector3 scroll;
public GUISkin OOOODDDODC;
public GUISkin OCOQCDCDCO;
private string objectname = "";
private string backupFolder = "/EasyRoads3D";
private int objectType = 0;
private bool OOQDOOQQ;
public NewEasyRoads3D()
{





instance = this;
titleContent =  new GUIContent("New EasyRoads3D Object");
position = new Rect((Screen.width - 350.0f) / 2.0f, (Screen.height - 130.0f) / 2.0f, 350.0f, 130.0f);
minSize = new Vector2(350.0f, 130.0f);
maxSize = new Vector2(350.0f, 130.0f);
}
public void OnDestroy(){
instance = null;
}
public static NewEasyRoads3D Instance{
get
{
if( instance == null ){
new NewEasyRoads3D();
}
return instance;
}
}
public void OnGUI()
{
if(OOOODDDODC == null){
OCOQCDCDCO = GUI.skin;
OOOODDDODC = (GUISkin)Resources.Load("ER3DSkin", typeof(GUISkin));
}
if(objectname == "") objectname = GetNewRoadName();


GUILayout.Space(15);
GUILayout.Box("", GUILayout.MinWidth(340), GUILayout.MaxWidth(340), GUILayout.Height(70));
GUI.skin = OCOQCDCDCO;
GUILayout.BeginArea  (new Rect (5, 5, 336, 250));
GUILayout.Label("Set a name for the new EasyRoads3D Road Object");
GUILayout.Space(65);
GUILayout.BeginArea  (new Rect (50, 40, 250, 150));



RoadObjectScript.objectStrings = new string[1];
RoadObjectScript.objectStrings[0] = "Road Object";;

EditorGUILayout.BeginHorizontal();
GUILayout.Label("Object type",GUILayout.Width(75));
objectType = EditorGUILayout.Popup (objectType, RoadObjectScript.objectStrings, EditorStyles.toolbarPopup,   GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label("Object name",GUILayout.Width(75));
objectname = GUILayout.TextField(objectname,GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
GUILayout.EndArea();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.Space(195);
if(GUILayout.Button ("Create Object", EditorStyles.toolbarButton, GUILayout.Width(125))){
if(objectname == ""){
EditorUtility.DisplayDialog("Alert", "Please fill out a name for the new road object!", "Close");
}else{
bool flag = false;
string[] dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + backupFolder);
foreach(string nm in dirs){
string[] words = nm.Split('\\');
words = words[words.Length - 1].Split('/');
string nm1 = words[words.Length - 1];
if(nm1.ToUpper() == objectname.ToUpper()){
EditorUtility.DisplayDialog("Alert", "An EasyRoads3D object with the name '"+objectname+"' already exists!\r\n\r\nPlease use an unique name!", "Close");
flag = true;
break;
}
}
if(!flag){
GameObject go = (GameObject)MonoBehaviour.Instantiate(Resources.Load("EasyRoad3DObject", typeof(GameObject)));
instance.Close();
go.name = objectname;
go.transform.position = Vector3.zero;
RoadObjectScript script = go.GetComponent<RoadObjectScript>();
script.OOQDOOQQ = false;
script.autoUpdate = true;
script.surrounding = 3.0f;
script.indent = 3.0f;
script.geoResolution = 2.5f;
script.objectType = objectType;
script.materialType = 0;
if(objectType == 1){
script.objectText = "River";
script.forceY = true;
}
if(objectType == 2)script.geoResolution = 0.5f;
Selection.activeGameObject =  go;
}
}
}
EditorGUILayout.EndHorizontal();
GUILayout.EndArea();
}
public string GetNewRoadName(){

if(EditorPrefs.GetInt("ER3DbckLocation", 0) == 1){
if(OOCCCOCCCC.extensionPath == ""){
OOCCCOCCCC.extensionPath = GetExtensionPath();
}
backupFolder = OOCCCOCCCC.extensionPath + "Backups";
}

string path = Directory.GetCurrentDirectory() + backupFolder;
if( !Directory.Exists(path)){
try{
Directory.CreateDirectory( path);
}
catch(System.Exception e){
Debug.Log("Could not create directory: " + path + " " + e);
return "";
}
}
string[] dirs = Directory.GetDirectories(@Directory.GetCurrentDirectory() + backupFolder);
int c = 0;
int num;
foreach(string nm in dirs){
string[] words = nm.Split('\\');
words = words[words.Length - 1].Split('/');
string nm1 = words[words.Length - 1];
if(nm.IndexOf("RoadObject") != -1){
string str = nm1.Replace("RoadObject","");
if(int.TryParse(str, out num)){
if(num > c) c = num;
}
}
}
c++;
string n;
if(c < 10) n = "RoadObject0" + c.ToString();
else n = "RoadObject" + c.ToString();
return n;
}
public string GetExtensionPath(){
string extensionPath  = Path.GetDirectoryName( AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( this ) ) );

extensionPath = extensionPath.Replace("lib", "");
extensionPath = extensionPath.Replace("Editor", "");
extensionPath = extensionPath.Replace("scripts", "");

return "/" + extensionPath;
}
}
