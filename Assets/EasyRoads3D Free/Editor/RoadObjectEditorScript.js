import System.Collections.Generic;
import System.IO;
import EasyRoads3D;
import EasyRoads3DEditor;
@CustomEditor(RoadObjectScript)
class RoadObjectEditorScript extends Editor
{
var counter : int;
var pe : float;
var tv : boolean;
var tvDone : boolean;
var debugDone : boolean;
var res : boolean;
var col : Collider;

function OnEnable(){

target.backupLocation = EditorPrefs.GetInt("ER3DbckLocation", 0);

if(target.extensionPath == ""){
OOCCCOCCCC.extensionPath = GetExtensionPath();
}
if(target.OODODOQQOD == null){
OQQCDOOQOQ();

OQQDODDCQC.CheckTerrainData(target.OODODOQQOD);
target.OOCCOODQQD(null, null, null);
}

target.ODODQOQO = target.OODODOQQOD.OOCCQODDOD();
target.ODODQOQOInt = target.OODODOQQOD.OCOCDOODOC();
if(target.splatmapLayer >= target.ODODQOQO.Length)target.splatmapLayer = 4;
if(target.customMesh != null){
if(target.customMesh.GetComponent(typeof(Collider))){
col = target.customMesh.GetComponent(typeof(Collider));
}else if(OQOCOOCCQO.terrain != null){
col = OQOCOOCCQO.terrain.GetComponent(typeof(TerrainCollider));
}
}else if(OQOCOOCCQO.terrain != null){
col = OQOCOOCCQO.terrain.GetComponent(typeof(TerrainCollider));
}

if(OQQCDOOQOQ()){
OQOCOOCCQO.OOCDCOQQCD();
}
target.OCCQQCQQOQs = new GameObject[0];




}
function OnInspectorGUI(){

EasyRoadsGUIMenu(true, true, target);
}
function OnSceneGUI() {
if(target.OODODOQQOD == null){
OQQCDOOQOQ();
target.OOCCOODQQD(null, null, null);
if(target.OCOCCQOCDD != EditorApplication.currentScene && target.OODODOQQOD == null){
OQQDCCCQDQ.terrainList.Clear();
target.OCOCCQOCDD = EditorApplication.currentScene;
}

}

OnScene();

}
function EasyRoadsGUIMenu(flag : boolean, senderIsMain : boolean,  nRoadScript : RoadObjectScript) : int {





if(target.OOCCDOCCDC == null || target.OOCOQDCQQQ == null || target.OQQOQOOOOO == null || target.OOCCDOCCDC.Length == 0 ){
target.OOCCDOCCDC = new boolean[5];
target.OOCOQDCQQQ = new boolean[5];
target.OQQOQOOOOO = nRoadScript;

target.OQDODQDOCO = target.OODODOQQOD.ODQOOCQCQQ();
target.ODODQOQO = target.OODODOQQOD.OOCCQODDOD();
target.ODODQOQOInt = target.OODODOQQOD.OCOCDOODOC();
}
origAnchor = GUI.skin.box.alignment;
if(target.OOOODDDODC == null){
target.OOOODDDODC = Resources.Load("ER3DSkin", GUISkin);
target.OOOCCQQQOC = Resources.Load("ER3DLogo", Texture2D);
}
if(!flag) target.OOCDQQDODQ();
if(target.OQOQOOOQQO == -1) target.OCCQQCQQOQ = null;
var origSkin : GUISkin = GUI.skin;
GUI.skin = target.OOOODDDODC;
EditorGUILayout.Space();

EditorGUILayout.BeginHorizontal ();
GUILayout.FlexibleSpace();
target.OOCCDOCCDC[0] = GUILayout.Toggle(target.OOCCDOCCDC[0] ,new GUIContent("", " Add road markers. "),"AddMarkers",GUILayout.Width(40), GUILayout.Height(22));
if(target.OOCCDOCCDC[0] == true && target.OOCOQDCQQQ[0] == false) {
target.OOCDQQDODQ();
target.OOCCDOCCDC[0] = true;  target.OOCOQDCQQQ[0] = true;
}
target.OOCCDOCCDC[1]  = GUILayout.Toggle(target.OOCCDOCCDC[1] ,new GUIContent("", " Insert road markers. "),"insertMarkers",GUILayout.Width(40),GUILayout.Height(22));
if(target.OOCCDOCCDC[1] == true && target.OOCOQDCQQQ[1] == false) {
target.OOCDQQDODQ();
target.OOCCDOCCDC[1] = true;  target.OOCOQDCQQQ[1] = true;
}
target.OOCCDOCCDC[2]  = GUILayout.Toggle(target.OOCCDOCCDC[2] ,new GUIContent("", " Process the terrain and create road geometry. "),"terrain",GUILayout.Width(40),GUILayout.Height(22));

if(target.OOCCDOCCDC[2] == true && (target.OOCOQDCQQQ[2] == false || target.doTerrain)) {

if(target.markers <= 2){
EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
target.OOCCDOCCDC[2] = false;
}else{
if(target.disableFreeAlerts)EditorUtility.DisplayDialog("Alert", "Switching back to 'Edit Mode' is not supported in the free version.\n\nClick Close to generate the road mesh and deform the terrain. This process can take some time depending on the terrains heightmap resolution and the optional vegetation removal, please be patient!\n\nYou can always restore the terrain using the EasyRoads3D terrain restore option in the main menu.\n\nNote: you can disable displaying this message in General Settings.", "Close");
if(!flag){
EditorUtility.DisplayDialog("Alert", "The Unity Terrain Object does not accept height values < 0. The river floor will be equal or higher then the water level. Position all markers higher above the terrain!", "Close");
target.OOCCDOCCDC[2] = false;
}else{
tvDone = false;
target.OOCDQQDODQ();
target.OOCCDOCCDC[2] = true;  target.OOCOQDCQQQ[2] = true;
target.OCCDOQDQQD = true;
target.doTerrain = false;
target.markerDisplayStr = "Show Markers";
if(target.objectType < 2){




#if UNITY_4_3

#else

#endif




if(!target.displayRoad){
target.displayRoad = true;
target.OODODOQQOD.OQOODQOQCQ(true, target.OOODODOQDC);
}
OQQDCCCQDQ.OOCOCQQOOC = false;

OCDCQCQODD(target);
if(target.OOQDOOQQ)target.OOCQDDOQOC();



}else{

target.OODODOQQOD.ODCDQOOOOO(target.transform, false);
}
} 
if(target.disableFreeAlerts)EditorUtility.DisplayDialog("Finished!", "The terrain data has been updated.\n\nIf you want to keep these changes and add more road objects it is recommended to update the terrain backup data using the EasyRoads3D terrain backup options in the main menu. By doing this you will not loose the current terrain changes if later in the development process you want to restore the terrain back to the current status.\n\nYou can also duplicate the terrain object in the project panel and keep that as the terrain backup.\n\nNote: you can disable displaying this message in General Settings.", "Close");





}
}

target.OOCCDOCCDC[3]  = GUILayout.Toggle(target.OOCCDOCCDC[3] ,new GUIContent("", " General settings. "),"settings",GUILayout.Width(40),GUILayout.Height(22));
if(target.OOCCDOCCDC[3] == true && target.OOCOQDCQQQ[3] == false) {
target.OOCDQQDODQ();
target.OOCCDOCCDC[3] = true;  target.OOCOQDCQQQ[3] = true;
}
target.OOCCDOCCDC[4]  = GUILayout.Toggle(target.OOCCDOCCDC[4] ,new GUIContent("", "Version and Purchase Info"),"info",GUILayout.Width(40),GUILayout.Height(22));
if(target.OOCCDOCCDC[4] == true && target.OOCOQDCQQQ[4] == false) {
target.OOCDQQDODQ();
target.OOCCDOCCDC[4] = true;  target.OOCOQDCQQQ[4] = true;
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.skin = null;
GUI.skin = origSkin;
target.OQOQOOOQQO = -1;
for(var i : int  = 0; i < 5; i++){
if(target.OOCCDOCCDC[i]){
target.OQOQOOOQQO = i;
target.OCCDDDDDCD = i;
}
}
if(target.OQOQOOOQQO == -1) target.OOCDQQDODQ();
var markerMenuDisplay : int = 1;
if(target.OQOQOOOQQO == 0 || target.OQOQOOOQQO == 1) markerMenuDisplay = 0;
else if(target.OQOQOOOQQO == 2 || target.OQOQOOOQQO == 3 || target.OQOQOOOQQO == 4) markerMenuDisplay = 0;

if(target.OCCDOQDQQD && !target.OOCCDOCCDC[2] && Application.isPlaying){
EditorPrefs.SetBool("ERv2isPlaying", true);

}







if(target.OCCDOQDQQD && !target.OOCCDOCCDC[2]){ 
target.OOCCDOCCDC[2] = true;
target.OOCOQDCQQQ[2] = true;
if(target.disableFreeAlerts)EditorUtility.DisplayDialog("Alert", "Switching back to 'Edit Mode' to add markers or change other settings is not supported in the free version.\n\nDrag the road mesh to the root of the hierarchy and delete the EasyRoads3D editor object once the road is ready!\n\nYou can use Undo to restore the terrain.", "Close");
}
GUI.skin.box.alignment = TextAnchor.UpperLeft;
if(target.OQOQOOOQQO >= 0 && target.OQOQOOOQQO != 4){
if(target.OQDODQDOCO == null || target.OQDODQDOCO.Length == 0){

target.OQDODQDOCO = target.OODODOQQOD.ODQOOCQCQQ();
target.ODODQOQO = target.OODODOQQOD.OOCCQODDOD();
target.ODODQOQOInt = target.OODODOQQOD.OCOCDOODOC();
}
EditorGUILayout.BeginHorizontal();
GUILayout.Box(target.OQDODQDOCO[target.OQOQOOOQQO], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
}
if(target.OQOQOOOQQO == -1 && target.OCCQQCQQOQ != null) Selection.activeGameObject =  target.OCCQQCQQOQ.gameObject;
GUI.skin.box.alignment = origAnchor;

if(target.erInit == "" || (OQQDCCCQDQ.debugFlag && !debugDone)){
debugDone = true;
target.erInit = "done";
target.OODODOQQOD.erInit = target.erInit;



this.Repaint();

}
if(target.erInit != "" && res){

target.OCQDOCOQDO(target.geoResolution, false, false);
res = false;
}
if(target.erInit.Length == 0){
}else if(target.OQOQOOOQQO == 0 || target.OQOQOOOQQO == 1){
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Refresh Surfaces", GUILayout.Width(200))){
target.ODDOOCQODD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
}else if(target.OQOQOOOQQO == 3){

GUI.skin.box.alignment = TextAnchor.MiddleLeft;
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
if(target.objectType != 2){
GUILayout.Space(10);
var oldDisplay : boolean = target.displayRoad;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Display object", "This will activate/deactivate the road object transforms"), GUILayout.Width(125) );
target.displayRoad = EditorGUILayout.Toggle (target.displayRoad);
EditorGUILayout.EndHorizontal();
if(oldDisplay != target.displayRoad){
target.OODODOQQOD.OQOODQOQCQ(target.displayRoad, target.OOODODOQDC);
}
}
if(target.materialStrings == null){target.materialStrings = new String[2]; target.materialStrings[0] = "Diffuse Shader"; target.materialStrings[1] = "Transparent Shader"; }
if(target.materialStrings.Length == 0){target.materialStrings = new String[2]; target.materialStrings[0] = "Diffuse Shader"; target.materialStrings[1] = "Transparent Shader"; }
var cm : int = target.materialType;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Surface Material", "The material type used for the road surfaces."), GUILayout.Width(125) );
target.materialType = EditorGUILayout.Popup (target.materialType, target.materialStrings,   GUILayout.Width(115));
EditorGUILayout.EndHorizontal();
if(cm != target.materialType) target.OODODOQQOD.ODOCQCOCDQ(target.materialType);
if(target.materialType == 1){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("        Surface Opacity", "This controls the transparacy level of the surface objects."), GUILayout.Width(125) );
var so : float = target.surfaceOpacity;
target.surfaceOpacity = EditorGUILayout.Slider(target.surfaceOpacity, 0, 1,  GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
if(so != target.surfaceOpacity) target.OODODOQQOD.ODQQODCDDD(target.surfaceOpacity);
}
EditorGUILayout.Space();
if(target.objectType < 2){
var od: boolean = target.multipleTerrains;
}
GUI.enabled = true;
GUI.enabled = false;
var cl = target.backupLocation;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Backup Location", "Use outside Assets folder unless you are using the asset server."), GUILayout.Width(125) );
target.backupLocation = EditorGUILayout.Popup (target.backupLocation, target.backupStrings,   GUILayout.Width(115));
EditorGUILayout.EndHorizontal();
if(target.backupLocation != cl){
if(target.backupLocation == 1){
if(EditorUtility.DisplayDialog("Backup Location", "Changing the backup location to inside the assets folder is only recommended when you want to synchronize EasyRoads3D backup files with the assetserver.\n\nWould you like to continue?", "Yes", "No")){
EditorPrefs.SetInt("ER3DbckLocation", target.backupLocation);
OQQDODDCQC.SwapFiles(target.backupLocation);
EditorUtility.DisplayDialog("Confirmation", "The backup location has been updated, all backup folders and files have been copied to the new location.\n\nUse CTRL+R to update the assets folder!", "Close");
}else target.backupLocation = 0;
}else{
if(EditorUtility.DisplayDialog("Backup Location", "The backup location will be changed to outside the assets folder.\n\nWould you like to continue?", "Yes", "No")){
EditorPrefs.SetInt("ER3DbckLocation", target.backupLocation);
OQQDODDCQC.SwapFiles(target.backupLocation);
EditorUtility.DisplayDialog("Confirmation", "The backup location has been updated, all backup folders and files have been copied to the new location.\n\nUse CTRL+R to update the assets folder!", "Close");
}else target.backupLocation = 1;
}
}
GUI.enabled = true;
od = OQQDCCCQDQ.debugFlag;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Enable Debugging", "This will enable debugging."), GUILayout.Width(125) );;
OQQDCCCQDQ.debugFlag = EditorGUILayout.Toggle (OQQDCCCQDQ.debugFlag);
EditorGUILayout.EndHorizontal();
if(od != OQQDCCCQDQ.debugFlag && OQQDCCCQDQ.debugFlag) debugDone = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Free version alerts", "Uncheck to disable free version alerts."), GUILayout.Width(125) );;
target.disableFreeAlerts = EditorGUILayout.Toggle (target.disableFreeAlerts);
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField("Object Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
EditorGUILayout.BeginHorizontal();
var wd : float = target.roadWidth;
if(target.objectType == 0)GUILayout.Label(new GUIContent("    Road width", "The width of the road") ,  GUILayout.Width(125));
else GUILayout.Label(new GUIContent("    River Width", "The width of the river") ,  GUILayout.Width(125));
target.roadWidth = EditorGUILayout.FloatField(target.roadWidth, GUILayout.Width(40) );
EditorGUILayout.EndHorizontal();
if(wd != target.roadWidth) target.OCQDOCOQDO(target.geoResolution, false, false);
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Default Indent", "The distance from the left and right side of the road to the part of the terrain levelled at the same height as the road"),  GUILayout.Width(125));
target.indent = EditorGUILayout.FloatField(target.indent, GUILayout.Width(40));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Raise Markers", "This will raise the position of the markers (m)."), GUILayout.Width(125) );;
target.raiseMarkers = EditorGUILayout.FloatField (target.raiseMarkers, GUILayout.Width(40));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Force Y Position", "When toggled on, ne road markers will inherit the y position of the previous marker."), GUILayout.Width(125) );;
target.forceY = EditorGUILayout.Toggle (target.forceY);
EditorGUILayout.EndHorizontal();
if(target.forceY){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("        Y Change", "The marker will be raised / lowered according this amount for every 100 meters."), GUILayout.Width(125) );;
target.yChange = EditorGUILayout.FloatField (target.yChange, GUILayout.Width(40));
EditorGUILayout.EndHorizontal();
}
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Surrounding", "This represents the distance over which the terrain will be gradually leveled to the original terrain height"),  GUILayout.Width(125));
target.surrounding = EditorGUILayout.FloatField(target.surrounding, GUILayout.Width(40));
EditorGUILayout.EndHorizontal();
var OldClosedTrack : boolean = target.OOQDOOQQ;
EditorGUILayout.BeginHorizontal();
if(target.objectType == 0)GUILayout.Label(new GUIContent("    Closed Track", "This will connect the 'start' and 'end' of the road"), GUILayout.Width(125) );
else if(target.objectType == 1)GUILayout.Label(new GUIContent("    Closed River", "This will connect the 'start' and 'end' of the river"), GUILayout.Width(125) );
else GUILayout.Label(new GUIContent("    Closed Object", "This will connect the 'start' and 'end' of the object"), GUILayout.Width(125) );
target.OOQDOOQQ = EditorGUILayout.Toggle (target.OOQDOOQQ);
EditorGUILayout.EndHorizontal();
if(OldClosedTrack != target.OOQDOOQQ){
target.ODDOOCQODD();
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
OldClosedTrack = target.iOS;
GUILayout.Label(new GUIContent("    iOS Platform", "This will prepare the road mesh for the iOS Platform"), GUILayout.Width(125) );
target.iOS = EditorGUILayout.Toggle (target.iOS);
EditorGUILayout.EndHorizontal();
if(OldClosedTrack != target.iOS){
target.OODODOQQOD.iOS = target.iOS;
target.ODDOOCQODD();
}
GUI.enabled = true;
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Geometry Resolution", "The polycount of the generated surfaces. It is recommended to use a low resolution while creating the road. Use the maximum resolution when processing the final terrain."), GUILayout.Width(125) );
var gr : float = target.geoResolution;
target.geoResolution = EditorGUILayout.Slider(target.geoResolution, 0.5, 5,  GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
if(gr != target.geoResolution) target.OCQDOCOQDO(target.geoResolution, false, false);
EditorGUILayout.BeginHorizontal();
OldClosedTrack = target.iOS;
GUI.enabled = false;
GUILayout.Label(new GUIContent("    Tangents", "This will automatically calculate mesh tangents data required for bump mapping. Note that this will take a little bit more processing time."), GUILayout.Width(125) );
target.applyTangents = EditorGUILayout.Toggle (target.applyTangents);
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
EditorGUILayout.Space();
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField("Render Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
GUI.enabled = false;
if(OQOCOOCCQO.selectedTerrain == null)OQOCOOCCQO.OOCDCOQQCD();
var st : int = OQOCOOCCQO.selectedTerrain;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Active Terrain", "The terrain that will be updated"), GUILayout.Width(125) );
OQOCOOCCQO.selectedTerrain = EditorGUILayout.Popup (OQOCOOCCQO.selectedTerrain, OQOCOOCCQO.terrainStrings,   GUILayout.Width(115));
EditorGUILayout.EndHorizontal();
if(st != OQOCOOCCQO.selectedTerrain)OQOCOOCCQO.ODQQQCQQCC();
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Update Vegetation", "When toggled on tree and detail objects near the road will be removed when rendering the terrain."), GUILayout.Width(125) );;
target.handleVegetation = EditorGUILayout.Toggle (target.handleVegetation);
EditorGUILayout.EndHorizontal();
if(target.handleVegetation){
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("      Tree Distance (m)", "The distance from the left and the right of the road up to which terrain trees should be removed."), GUILayout.Width(125) );
var tr : float = target.OCQCQQOCCQ;
target.OCQCQQOCCQ = EditorGUILayout.Slider(target.OCQCQQOCCQ, 0, 25,  GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
if(tr != target.OCQCQQOCCQ) target.OODODOQQOD.OCQCQQOCCQ = target.OCQCQQOCCQ;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("      Detail Distance (m)", "The distance from the left and the right of the road up to which terrain detail opbjects should be removed."), GUILayout.Width(125) );
tr = target.ODCOOQCCCQ;
target.ODCOOQCCCQ = EditorGUILayout.Slider(target.ODCOOQCCCQ, 0, 25,  GUILayout.Width(150));
EditorGUILayout.EndHorizontal();
if(tr != target.ODCOOQCCCQ) target.OODODOQQOD.ODCOOQCCCQ = target.ODCOOQCCCQ;
GUI.enabled = true;
}
EditorGUILayout.Space();


}else if(target.OQOQOOOQQO == 2){

EditorGUILayout.Space();
EditorGUILayout.BeginVertical("Box");
if(target.objectType == 0)EditorGUILayout.LabelField("Road Settings", EditorStyles.boldLabel);
else EditorGUILayout.LabelField("River Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
GUILayout.Space(10);
var oldRoad : boolean = target.renderRoad;
var oldRoadResolution : float = target.roadResolution;
var oldRoadUV : float = target.tuw;
var oldRaise : float = target.raise;
var oldSegments : int = target.OdQODQOD;
var oldOOQQQDOD : float = target.OOQQQDOD;
var oldOOQQQDODOffset : float = target.OOQQQDODOffset;
var oldOOQQQDODLength : float = target.OOQQQDODLength;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Render"," When active, terrain matching road geometry will be created."), GUILayout.Width(105) );
target.renderRoad = EditorGUILayout.Toggle (target.renderRoad);
EditorGUILayout.EndHorizontal();
if(target.renderRoad){
if(target.objectType == 0){
if(target.roadTexture == null){
mat = Resources.Load("roadMaterial", typeof(Material));
target.roadTexture = mat.mainTexture;
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Material"," The road texture."), GUILayout.Width(105) );
if(GUILayout.Button (target.roadTexture, GUILayout.Width(75), GUILayout.Height(75))){
}
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
}
if(target.objectType == 0){
GUILayout.Space(10);
if(target.OOQDOOQQ){
GUI.enabled = false;
if(target.blendFlag){
target.blendFlag = false;
}
}
var bf = target.blendFlag;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Blend the start and / or end"," When active, the start and / or end of the road will blend with the terrain according the below settings."), GUILayout.Width(175) );
target.blendFlag = EditorGUILayout.Toggle (target.blendFlag);
EditorGUILayout.EndHorizontal();
if(target.blendFlag != bf)target.OODODOQQOD.blendFlag = target.blendFlag; 
if(target.blendFlag && !bf){
if(target.startBlendDistance > 0)OQQDCCCQDQ.FadeBlend(0, target.startBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
if(target.endBlendDistance > 0)OQQDCCCQDQ.FadeBlend(1, target.endBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
}else if(!target.blendFlag && bf){
OQQDCCCQDQ.ResetBlend(target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh);
}
if(!target.blendFlag)GUI.enabled = false;
var bd = target.startBlendDistance;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("   Start Distance"," The distance over which the road will be blended with terrain at the start."), GUILayout.Width(100) );
target.startBlendDistance = EditorGUILayout.Slider(target.startBlendDistance, 0, 20,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
if(bd != target.startBlendDistance){
target.OODODOQQOD.startBlendDistance = target.startBlendDistance;
if(target.startBlendDistance > 0)OQQDCCCQDQ.FadeBlend(0, target.startBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
if(target.endBlendDistance > 0)OQQDCCCQDQ.FadeBlend(1, target.endBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
}
bd = target.endBlendDistance;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("   End Distance"," The distance over which the road will be blended with terrain at the end."), GUILayout.Width(100) );
target.endBlendDistance = EditorGUILayout.Slider(target.endBlendDistance, 0, 20,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
if(bd != target.endBlendDistance){
target.OODODOQQOD.endBlendDistance = target.endBlendDistance;
if(target.startBlendDistance > 0)OQQDCCCQDQ.FadeBlend(0, target.startBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
if(target.endBlendDistance > 0)OQQDCCCQDQ.FadeBlend(1, target.endBlendDistance, target.OODODOQQOD.road.GetComponent(typeof(MeshFilter)).sharedMesh, target.OODODOQQOD.OdQODQOD);
}
GUI.enabled = true;
GUILayout.Space(10);
}
EditorGUILayout.BeginHorizontal();
GUI.enabled = false;
GUILayout.Label(new GUIContent(" Road Segments"," The number of segments over the width of the road."), GUILayout.Width(105) );
target.OdQODQOD = EditorGUILayout.IntSlider(target.OdQODQOD, 1, 10,  GUILayout.Width(175));
GUI.enabled = true;
EditorGUILayout.EndHorizontal();
if(target.OdQODQOD > 1){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Bumpyness"," The bumypness of the surface of the road."), GUILayout.Width(95) );
target.OOQQQDOD = EditorGUILayout.Slider(target.OOQQQDOD, 0, 1,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Bumpyness Offset"," The bumypness variation of the road."), GUILayout.Width(95) );
target.OOQQQDODOffset = EditorGUILayout.Slider(target.OOQQQDODOffset, 0, 1,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Bumpyness Density"," The bumypness density on the road."), GUILayout.Width(95) );
target.OOQQQDODLength = EditorGUILayout.Slider(target.OOQQQDODLength, 0.01, 1,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Resolution"," The resolution level of the road geometry."), GUILayout.Width(95) );
target.roadResolution = EditorGUILayout.IntSlider(target.roadResolution, 1, 10,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
if(target.objectType == 0){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" UV Mapping"," Use the slider to control texture uv mapping of the road geometry."), GUILayout.Width(95) );
target.tuw = EditorGUILayout.Slider(target.tuw, 1, 30,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Raise (cm)","Optionally increase this setting when parts of the terrain stick through the road geometry. It is recommended to adjust these areas using the terrain tools!"), GUILayout.Width(95) );
target.raise = EditorGUILayout.Slider(target.raise, 0, 100, GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
GUILayout.Space(10);
}else{
}
GUILayout.Space(5);
GUI.enabled = false;
if(target.applyTangents)GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Calculate Tangents", GUILayout.Width(175))){
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Save Geometry", GUILayout.Width(175))){
target.ODQCDOCCDO();
Debug.Log("Road object geometry saved");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Finalize Object", GUILayout.Width(175))){
var bflag = false;
for(i=0;i<target.ODODQQOD.Length;i++){
if(target.ODODQQOD[i]){
bflag = true;
break;
}
}
if(target.autoODODDQQO || target.sosBuild == true)bflag = false;
if(EditorUtility.DisplayDialog("Important!", "This will unlink the road from the EasyRoads3D editor object and the EasyRoads3D object will be destroyed!\n\nWould you like to continue?", "Yes", "No")){
if(bflag){
if(EditorUtility.DisplayDialog("Important!", "This object includes activated side objects that have not yet been build!\n\nAre you sure you would you like to continue?", "Yes", "No")){
bflag = false;
}
}
if(!bflag){
target.OODODOQQOD.FinalizeObject(target.gameObject);
DestroyImmediate(target.gameObject);
}
}
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
}
EditorGUILayout.Space();
if(oldRoad != target.renderRoad || oldRoadResolution != target.roadResolution || oldRoadUV != target.tuw || oldRaise != target.raise || oldSegments != target.OdQODQOD || target.OOQQQDOD != oldOOQQQDOD || target.OOQQQDODOffset != oldOOQQQDODOffset || target.OOQQQDODLength != oldOOQQQDODLength){

target.OQQDOQDOQD();

}
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField("Terrain Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
GUILayout.Space(5);
var oldApplySplatmap : boolean = target.applySplatmap;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Apply Splatmap"," When active, the road will be added to the terrain splatmap. The quality highly depends on the terrain Control Texture Resolution size."), GUILayout.Width(125) );
target.applySplatmap = EditorGUILayout.Toggle (target.applySplatmap);
EditorGUILayout.EndHorizontal();
if(target.applySplatmap){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Terrain texture", "This represents the terrain texture which will be used for the road spatmap."), GUILayout.Width(125) );
target.splatmapLayer = EditorGUILayout.IntPopup (target.splatmapLayer, target.ODODQOQO, target.ODODQOQOInt,   GUILayout.Width(120));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Expand"," Use this setting to increase the size of the splatmap."), GUILayout.Width(125) );
target.expand = EditorGUILayout.IntSlider(target.expand,0, 3, GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Smooth Level"," Use this setting to blur the road splatmap for smoother results."), GUILayout.Width(125) );
target.splatmapSmoothLevel = EditorGUILayout.IntSlider (target.splatmapSmoothLevel, 0, 3,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Offset x"," Moves the road splatmap in the x direction."), GUILayout.Width(125) );
target.offsetX = EditorGUILayout.IntField (target.offsetX, GUILayout.Width(50));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Offset y"," Moves the road splatmap in the y direction."), GUILayout.Width(125) );
target.offsetY= EditorGUILayout.IntField (target.offsetY, GUILayout.Width(50));
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Opacity","Use this setting to blend the road splatmap with the terrain splatmap."), GUILayout.Width(125) );
target.opacity = EditorGUILayout.Slider (target.opacity, 0, 1,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
GUILayout.Space(5);
GUI.enabled = target.OQOQOOOCQO;
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Apply Changes", GUILayout.Width(175))){
target.OOCQDDOQOC();

if(target.OOQDOOQQ)target.OOCQDDOQOC();
target.OQOQOOOCQO = false;
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
}
GUILayout.Space(5);
if(oldApplySplatmap != target.applySplatmap){
target.OOCQDDOQOC();

if(target.OOQDOOQQ)target.OOCQDDOQOC();
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Terrain Smoothing:", "This will smoothen the terrain near the surface edges according the below distance."), GUILayout.Width(175) );
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Edges (m)","This represents the smoothen distance."), GUILayout.Width(125) );
target.smoothDistance = EditorGUILayout.Slider (target.smoothDistance, 0, 5,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
GUILayout.Space(5);
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Update Edges", GUILayout.Width(175))){

#if UNITY_4_3

#else

#endif
target.OODODOQQOD.OODDQCOQCQ(target.smoothDistance, 0);
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent(" Surrounding (m)","This represents the smoothen distance."), GUILayout.Width(125) );
target.smoothSurDistance = EditorGUILayout.Slider (target.smoothSurDistance, 0, 15,  GUILayout.Width(175));
EditorGUILayout.EndHorizontal();
GUILayout.Space(5);
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Update Surrounding", GUILayout.Width(175))){

#if UNITY_4_3

#else

#endif
target.OODODOQQOD.OODDQCOQCQ(target.smoothSurDistance, 1);
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();

EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField("Cam Fly Over", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("  Position", ""), GUILayout.Width(75) );
var sp : float = target.splinePos;
target.splinePos = EditorGUILayout.Slider(target.splinePos, 0, 0.9999);
EditorGUILayout.EndHorizontal();
if(sp != target.splinePos){
}
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("  Height", ""), GUILayout.Width(75) );
sp = target.camHeight;
target.camHeight = EditorGUILayout.Slider(target.camHeight, 1, 10);
EditorGUILayout.EndHorizontal();
if(sp != target.camHeight){
}
GUI.enabled = true;
EditorGUILayout.Space();
}else if(target.OQOQOOOQQO == 4){
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();

GUILayout.Label(target.OOOCCQQQOC);
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
GUILayout.Label(" EasyRoads3D v"+target.version);
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();

GUILayout.Label(" Version Type: Free Version", GUILayout.Height(22));
if(GUILayout.Button ("i", GUILayout.Width(22))){
Application.OpenURL ("http://www.unityterraintools.com");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Help", GUILayout.Width(225))){
Application.OpenURL ("http://www.unityterraintools.com/manual.php");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.skin = origSkin;
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
//EditorGUILayout.BeginVertical("Box");
EditorGUILayout.HelpBox("\nCheck out the full version if you had like to take advantage of all the features including the built-in paramatric modeling tool\n", MessageType.None, true);
//EditorGUILayout.EndVertical();
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Purchase Full Version from website", GUILayout.Width(225))){
Application.OpenURL ("http://www.unityterraintools.com/store.php");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Purchase from the Assetstore", GUILayout.Width(225))){
//	AssetStore.Open("http://u3d.as/content/anda-soft/easy-roads3d-pro/1Ch");
Application.OpenURL ("https://www.assetstore.unity3d.com/#/content/469");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
GUILayout.Label(new GUIContent("  Newsletter Sign Up:",""), GUILayout.Width(155) );
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
GUILayout.Label(new GUIContent("  Name", ""), GUILayout.Width(75) );
target.uname = GUILayout.TextField(target.uname,  GUILayout.Width(150));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
GUILayout.Label(new GUIContent("  Email", ""), GUILayout.Width(75) );
target.email = GUILayout.TextField(target.email,  GUILayout.Width(150));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Submit", GUILayout.Width(225))){
EditorUtility.DisplayDialog("Newsletter Signup", OCQDCDCOCQ0.NewsletterSignUp(target.uname, target.email), "Ok");
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
}else{
if(target.markers != target.OOODODOQDC.childCount){
target.ODDOOCQODD();
}
EditorGUILayout.Space();

EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField(" General Info", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();

if(RoadObjectScript.objectStrings == null){
RoadObjectScript.objectStrings = new String[3];
RoadObjectScript.objectStrings[0] = "Road Object"; RoadObjectScript.objectStrings[1]="River Object";RoadObjectScript.objectStrings[2]="Procedural Mesh Object";
}
if(target.distance == "-1"){
var ar : String[]  = target.OODODOQQOD.ODODCOOQCO(-1);
target.distance = ar[0];
}
EditorGUILayout.Space();
GUILayout.Label("    Object Type: " + RoadObjectScript.objectStrings[target.objectType]);
if(target.objectType == 0) GUILayout.Label("    Total Road Distance: " + target.distance.ToString() + " km");
}
EditorGUILayout.Space();
EditorGUILayout.Space();
if (GUI.tooltip != "") GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 200, 40), GUI.tooltip);
if (GUI.changed)
{
target.OQOQOOOCQO = true;
}
return markerMenuDisplay;
}
function OODQDODQOC(fwd: Vector3, targetDir: Vector3, up: Vector3) : float {
var perp: Vector3 = Vector3.Cross(fwd, targetDir);
var dir: float = Vector3.Dot(perp, up);
if (dir > 0.0) {
return 1.0;
} else if (dir < 0.0) {
return -1.0;
} else {
return 0.0;
}
}
function OnScene(){
var cEvent : Event = Event.current;
if(target.OCCDDDDDCD != -1  && Event.current.shift) target.OOCCDOCCDC[target.OCCDDDDDCD] = true;
if(target.OOCCDOCCDC == null || target.OOCCDOCCDC.Length == 0){
target.OOCCDOCCDC = new boolean[5];
target.OOCOQDCQQQ = new boolean[5];
}
if((cEvent.shift  && cEvent.type == EventType.mouseDown) || target.OOCCDOCCDC[1])
{
var hit : RaycastHit;
var mPos : Vector2 = cEvent.mousePosition;
mPos.y = Screen.height - mPos.y - 40;
var ray : Ray = Camera.current.ScreenPointToRay(mPos);


if (col.Raycast (ray, hit, 3000))
{
if(target.OOCCDOCCDC[0]){
var scrpts : RoadObjectScript[] = FindObjectsOfType(typeof(RoadObjectScript));
if(scrpts.Length > 1){
EditorUtility.DisplayDialog("Alert", "The Free version supports only one road editor object in the OCOCCQOCDD!\n\nPlease finalize the current road object or upgrade to the full version before creating a new road object.", "Close");
return;
}
go = target.OQCCDQOOCQ(hit.point);
}
else if(target.OOCCDOCCDC[1] && cEvent.type == EventType.mouseDown && cEvent.shift){

target.OCQOOCOCQD(hit.point, true);
}
else if(target.OOCCDOCCDC[1]  && cEvent.shift) target.OCQOOCOCQD(hit.point, false);
else if(target.handleInsertFlag) target.handleInsertFlag = target.OODODOQQOD.OCOOCDDDQC();
Selection.activeGameObject = target.obj.gameObject;
}
}
if(cEvent.control && cEvent.alt && cEvent.type == EventType.mouseDown){
mPos = cEvent.mousePosition;
mPos.y = Screen.height - mPos.y - 40;
ray = Camera.current.ScreenPointToRay(mPos);
if (Physics.Raycast (Camera.current.transform.position, ray.direction, hit, 3000))
{
if(hit.collider.gameObject.GetComponent(typeof(Terrain)) != null){

var t : Terrain = hit.collider.gameObject.GetComponent(typeof(Terrain));
for(i = 0; i < OQOCOOCCQO.terrains.Length; i++){

if(t == OQOCOOCCQO.terrains[i]){
if(OQOCOOCCQO.terrains.Length > 1)OQOCOOCCQO.selectedTerrain = i + 1;
else OQOCOOCCQO.selectedTerrain = i;
OQOCOOCCQO.ODQQQCQQCC();
EditorUtility.SetDirty (target);
}
}
}
}
}
if(target.OCCDOQCCQQ != target.obj || target.obj.name != target.OQQODQQDOO){
target.OODODOQQOD.OQQOOCCDQO();
target.OCCDOQCCQQ = target.obj;
target.OQQODQQDOO = target.obj.name;
}
if(target.OCCQQCQQOQ != null){
target.OODODOQQOD.OCOOCDDDQC();

}

}
static function OQQCDOOQOQ() : boolean{

var flag : boolean = false;
var terrains : Terrain[]  = MonoBehaviour.FindObjectsOfType(typeof(Terrain));
for(terrain in terrains) {
if(!terrain.gameObject.GetComponent(EasyRoads3DTerrainID)){
var terrainscript : EasyRoads3DTerrainID = terrain.gameObject.AddComponent.<EasyRoads3DTerrainID>();
var id : String = UnityEngine.Random.Range(100000000,999999999).ToString();
terrainscript.terrainid = id;
//Debug.Log(id);
flag = true;

path = Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + id;
if( !Directory.Exists(path)){
try{
Directory.CreateDirectory( path);
}
catch(e:System.Exception)
{
Debug.Log("Could not create directory: " + path + " " + e);
}
}
if(Directory.Exists(path)){


}
}
}

OQQDCCCQDQ.terrainList.Clear();

for(terrain in terrains) {
var t:Terrains = new Terrains();
t.terrain = terrain;
if(!terrain.gameObject.GetComponent.<EasyRoads3DTerrainID>()){
terrainscript = terrain.gameObject.AddComponent.<EasyRoads3DTerrainID>();
id = UnityEngine.Random.Range(100000000,999999999).ToString();
terrainscript.terrainid = id;
t.id = id;
}else{
t.id = terrain.gameObject.GetComponent.<EasyRoads3DTerrainID>().terrainid;
}
OQQDCCCQDQ.OOCDCOQQCD(t);
}
OQOCOOCCQO.OOCDCOQQCD();
return flag;
}
static function OCDCQCQODD(target){
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object","Initializing", 0);

scripts = FindObjectsOfType(typeof(RoadObjectScript));
var rObj : List.<Transform> = new List.<Transform>();
for(script in scripts) {
if(script.transform != target.transform) rObj.Add(script.transform);
}
if(target.ODODQOQO == null){
target.ODODQOQO = target.OODODOQQOD.OOCCQODDOD();
target.ODODQOQOInt = target.OODODOQQOD.OCOCDOODOC();
}
target.OCQDOCOQDO(0.5f, true, false);


if(OQOCOOCCQO.selectedTerrain == null || target.OODODOQQOD.terrain == null)OQOCOOCCQO.OOCDCOQQCD();
target.OODODOQQOD.OCQQDOQCDO();

OQOCOOCCQO.OQDCQQCQQD(target.OODODOQQOD.terrain, Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder + "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO+"_splatMap");


OQQDODDCQC.OQOQQCDDDD(target.OODODOQQOD.terrain, Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO+"_heightmap.backup");
var hitODQCCQQQDD : List.<tPoint> = target.OODODOQQOD.ODQCOOQQDD(Vector3.zero, target.raise, target.obj, target.OOQDOOQQ, rObj, target.handleVegetation);
var changeArr : List.<Vector3> = new List.<Vector3>();
var stepsf : float = Mathf.Floor(hitODQCCQQQDD.Count / 10);
var steps : int = Mathf.RoundToInt(stepsf);


for(i = 0; i < 10;i++){
changeArr = target.OODODOQQOD.ODCCCQQQCC(hitODQCCQQQDD, i * steps, steps, changeArr);
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object","Updating Terrain", i * 10);
}

changeArr = target.OODODOQQOD.ODCCCQQQCC(hitODQCCQQQDD, 10 * steps, hitODQCCQQQDD.Count - (10 * steps), changeArr);
target.OODODOQQOD.OODQQQOCDC(changeArr, rObj);
if(target.OODODOQQOD.handleVegetation){
target.OODODOQQOD.OOOOQDQQCQ();

path = Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQDODOQDOD.OQQODQQDOO;
OOCCCOCCCC.OCOCQDQCQQ(Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder);
OOCCCOCCCC.OCOCQDQCQQ(Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain));
OQQDODDCQC.OOQQODOQCQ(target.OODODOQQOD.ODCOOODDQO.ToArray(), target.OODODOQQOD.ODOCQDCOCQ, path);
}

target.OOCQDDOQOC();

target.OODODOQQOD.OODDCODQOD(target.transform, true);
target.OODODOQQOD.OQQCQQOQDD();
EditorUtility.ClearProgressBar();

}
function OOQQOCDQCQ(target){
EditorUtility.DisplayProgressBar("Restore EasyRoads3D Object","Restoring terrain data", 0f);
target.OCQDOCOQDO(target.geoResolution, false, false);

if(target.OODODOQQOD.OCQCQCCQCQ != null && target.OODODOQQOD != null){
if(target.OODODOQQOD.editRestore && File.Exists(Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO+"_heightmap.backup")){
OQQDODDCQC.OOCOQDOQCC(target.OODODOQQOD.terrain, Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO+"_heightmap.backup");
}else if(target.OODODOQQOD.editRestore){
Debug.LogWarning("The original terrain heightmap data file was not found. If necessary you may restore the terrain data by using Undo or, if the terrain backup is up to date, through the EasyRoads3D Menu");
}
}

if(target.OODODOQQOD != null){
target.OODODOQQOD.OQODQOQOOQ();
if(target.OODODOQQOD.handleVegetation && target.OODODOQQOD.editRestore){
if(target.OODODOQQOD.ODCOOODDQO != null){
if(target.OODODOQQOD.ODCOOODDQO.Count == 0){
// get treeData from file
path = Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO;
target.OODODOQQOD.ODCOOODDQO = OQQDODDCQC.OOOCQCQDDQ(path);
}
if(target.OODODOQQOD.ODCOOODDQO != null) target.OODODOQQOD.OOCODDQDOC();

if(target.OODODOQQOD.ODOCQDCOCQ.Count == 0){
// get detailData from file

path = Directory.GetCurrentDirectory() + OOCCCOCCCC.backupFolder+ "/" + OQOCOOCCQO.OQOCQQQQQO(target.OODODOQQOD.terrain) + "/"+target.OODODOQQOD.OQQODQQDOO;
target.OODODOQQOD.ODOCQDCOCQ = OQQDODDCQC.ODODCQCCOC(path);

}
if(target.OODODOQQOD.ODOCQDCOCQ != null) target.OODODOQQOD.OCQDDDDDQC();
}
}
}
target.ODODDDOO = false;

EditorUtility.ClearProgressBar();
}
function GetExtensionPath() : String{
var extensionPath  = Path.GetDirectoryName( AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( this ) ) );

extensionPath = extensionPath.Replace("lib", "");
extensionPath = extensionPath.Replace("Editor", "");
extensionPath = extensionPath.Replace("scripts", "");

return "/" + extensionPath;
}
}
