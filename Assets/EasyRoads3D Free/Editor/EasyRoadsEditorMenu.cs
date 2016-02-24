using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using EasyRoads3D;
using EasyRoads3DEditor;
public class EasyRoadsEditorMenu : ScriptableObject {







[MenuItem( "GameObject/Create Other/EasyRoads3D/New EasyRoads3D Object" )]
public static void  CreateEasyRoads3DObject ()
{

RoadObjectScript[] scrpts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
if(scrpts.Length >= 1){
EditorUtility.DisplayDialog("Alert", "The Free version supports only one road editor object in the OCOCCQOCDD!\n\nPlease finalize or delete the current road object or upgrade to the full version before creating a new road object.", "Close");
Selection.activeGameObject = scrpts[0].gameObject;
return;
}
Terrain[] terrains = (Terrain[]) FindObjectsOfType(typeof(Terrain));
if(terrains.Length == 0){
EditorUtility.DisplayDialog("Alert", "No Terrain objects found! EasyRoads3D objects requires a terrain object to interact with. Please create a Terrain object first", "Close");
return;
}



if(NewEasyRoads3D.instance == null){
NewEasyRoads3D window = (NewEasyRoads3D)ScriptableObject.CreateInstance(typeof(NewEasyRoads3D));
window.ShowUtility();
}



}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Back Up/Terrain Height Data" )]
public static void  GetTerrain ()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.OODOOQQOOC(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Restore/Terrain Height Data" )]
public static void  SetTerrain ()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.ODCCDODCOQ(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Back Up/Terrain Splatmap Data" )]
public static void  OQDCQQCQQD()
{
if(GetEasyRoads3DObjects()){

OQOCOOCCQO.OQDCQQCQQD(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Restore/Terrain Splatmap Data" )]
public static void  OOODOQQDOD ()
{
if(GetEasyRoads3DObjects()){
string path = "";
if(EditorUtility.DisplayDialog("Road Splatmap", "Would you like to merge the terrain splatmap(s) with a road splatmap?", "Yes", "No")){
path = EditorUtility.OpenFilePanel("Select png road splatmap texture", "", "png");
}


OQOCOOCCQO.OOOQODCQDD(true, 100, 4, path, Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Back Up/Terrain Vegetation Data" )]
public static void  OCQQOODQDD()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.OCQQOODQDD(Selection.activeGameObject, null, "");
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Back Up/All Terrain Data" )]
public static void  GetAllData()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.OODOOQQOOC(Selection.activeGameObject);
OQOCOOCCQO.OQDCQQCQQD(Selection.activeGameObject);
OQQDODDCQC.OCQQOODQDD(Selection.activeGameObject, null,"");
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Restore/Terrain Vegetation Data" )]
public static void  ODCOOOQCQQ()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.ODCOOOQCQQ(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "GameObject/Create Other/EasyRoads3D/Restore/All Terrain Data" )]
public static void  RestoreAllData()
{
if(GetEasyRoads3DObjects()){

OQQDODDCQC.ODCCDODCOQ(Selection.activeGameObject);
OQOCOOCCQO.OOOQODCQDD(true, 100, 4, "", Selection.activeGameObject);
OQQDODDCQC.ODCOOOQCQQ(Selection.activeGameObject);

}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}


}
public static bool GetEasyRoads3DObjects(){
RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
bool flag = false;
foreach (RoadObjectScript script in scripts) {
if(script.OODODOQQOD == null){
script.OOCCOODQQD(null, null, null);
}
flag = true;
}
return flag;
}
static private void OCDCQCQODD(RoadObjectScript target){
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object - " + target.gameObject.name,"Initializing", 0);

RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
List<Transform> rObj = new List<Transform>();


#if UNITY_4_3

#else

#endif
foreach(RoadObjectScript script in scripts) {
if(script.transform != target.transform) rObj.Add(script.transform);
}
if(target.ODODQOQO == null){
target.ODODQOQO = target.OODODOQQOD.OOCCQODDOD();
target.ODODQOQOInt = target.OODODOQQOD.OCOCDOODOC();
}
target.OCQDOCOQDO(0.5f, true, false);

List<tPoint> hitODQCCQQQDD = target.OODODOQQOD.ODQCOOQQDD(Vector3.zero, target.raise, target.obj, target.OOQDOOQQ, rObj, target.handleVegetation);
List<Vector3> changeArr = new List<Vector3>();
float stepsf = Mathf.Floor(hitODQCCQQQDD.Count / 10);
int steps = Mathf.RoundToInt(stepsf);
for(int i = 0; i < 10;i++){
changeArr = target.OODODOQQOD.ODCCCQQQCC(hitODQCCQQQDD, i * steps, steps, changeArr);
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object - " + target.gameObject.name,"Updating Terrain", i * 10);
}

changeArr = target.OODODOQQOD.ODCCCQQQCC(hitODQCCQQQDD, 10 * steps, hitODQCCQQQDD.Count - (10 * steps), changeArr);
target.OODODOQQOD.OODQQQOCDC(changeArr, rObj);

target.OOCQDDOQOC();
EditorUtility.ClearProgressBar();

}
private static void SetWaterScript(RoadObjectScript target){
for(int i = 0; i < target.OOQCDQCQCQ.Length; i++){
if(target.OODODOQQOD.road.GetComponent(target.OOQCDQCQCQ[i]) != null && i != target.selectedWaterScript)DestroyImmediate(target.OODODOQQOD.road.GetComponent(target.OOQCDQCQCQ[i]));
}
if(target.OOQCDQCQCQ[0] != "None Available!"  && target.OODODOQQOD.road.GetComponent(target.OOQCDQCQCQ[target.selectedWaterScript]) == null){
#if UNITY_5
//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(target.OODODOQQOD.road, "Assets/EasyRoads3D/Editor/EasyRoadsEditorMenu.cs (460,4)", target.OOQCDQCQCQ[target.selectedWaterScript]);
			#elif UNITY_5
			//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(target.OODODOQQOD.road, "Assets/EasyRoads3D/Editor/EasyRoadsEditorMenu.cs (460,4)", target.OOQCDQCQCQ[target.selectedWaterScript]);
			#else
target.OODODOQQOD.road.AddComponent(target.OOQCDQCQCQ[target.selectedWaterScript]);
#endif

}
}
public static Vector3 ReadFile(string file)
{
Vector3 pos = Vector3.zero;
if(File.Exists(file)){
StreamReader streamReader = File.OpenText(file);
string line = streamReader.ReadLine();
line = line.Replace(",",".");
string[] lines = line.Split("\n"[0]);
string[] arr = lines[0].Split("|"[0]);
float.TryParse(arr[0],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.x);
float.TryParse(arr[1],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.y);
float.TryParse(arr[2],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.z);
}
return pos;
}
}
