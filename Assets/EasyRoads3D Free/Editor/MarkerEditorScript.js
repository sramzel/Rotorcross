import EasyRoads3D;
@CustomEditor(MarkerScript)
@CanEditMultipleObjects
class MarkerEditorScript extends Editor
{
var oldPos : Vector3;
var pos : Vector3;
var OOOODDDODC : GUISkin;
var OCOQCDCDCO : GUISkin;
var showGui : int;
var OCDQCCQDDD : boolean;
var count:int = 0;
function OnEnable(){
if(target.objectScript == null) target.SetObjectScript();
else target.GetMarkerCount();
}
function OnInspectorGUI()
{


if(target.objectScript.OCCQQCQQOQs == null)target.objectScript.OCCQQCQQOQs = new GameObject[0];
showGui = EasyRoadsGUIMenu(false, false, target.objectScript);
if(showGui != -1 && !target.objectScript.ODODDQOO) Selection.activeGameObject =  target.transform.parent.parent.gameObject;
else if(target.objectScript.OCCQQCQQOQs.length <= 1  && !target.objectScript.ODODDDOO) ERMarkerGUI(target);
else  if(target.objectScript.OCCQQCQQOQs.length == 2 && !target.objectScript.ODODDDOO) MSMarkerGUI(target);
else if(target.objectScript.ODODDDOO)TRMarkerGUI(target);


}
function OnSceneGUI() {
if(target.objectScript.OODODOQQOD == null || target.objectScript.erInit == "") Selection.activeGameObject =  target.transform.parent.parent.gameObject;
else MarkerOnScene(target);
}
function EasyRoadsGUIMenu(flag : boolean, senderIsMain : boolean,  nRoadScript : RoadObjectScript) : int {
if((target.objectScript.OOCCDOCCDC == null || target.objectScript.OOCOQDCQQQ == null || target.objectScript.OQQOQOOOOO == null) && target.objectScript.OODODOQQOD){
target.objectScript.OOCCDOCCDC = new boolean[5];
target.objectScript.OOCOQDCQQQ = new boolean[5];
target.objectScript.OQQOQOOOOO = nRoadScript;

target.objectScript.OQDODQDOCO = target.objectScript.OODODOQQOD.ODQOOCQCQQ();
target.objectScript.ODODQOQO = target.objectScript.OODODOQQOD.OOCCQODDOD();
target.objectScript.ODODQOQOInt = target.objectScript.OODODOQQOD.OCOCDOODOC();
}else if(target.objectScript.OODODOQQOD == null) return;

if(target.objectScript.OOOODDDODC == null){
target.objectScript.OOOODDDODC = Resources.Load("ER3DSkin", GUISkin);
target.objectScript.OOOCCQQQOC = Resources.Load("ER3DLogo", Texture2D);
}
if(!flag) target.objectScript.OOCDQQDODQ();
GUI.skin = target.objectScript.OOOODDDODC;
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal ();
GUILayout.FlexibleSpace();
target.objectScript.OOCCDOCCDC[0] = GUILayout.Toggle(target.objectScript.OOCCDOCCDC[0] ,new GUIContent("", " Add road markers. "),"AddMarkers",GUILayout.Width(40), GUILayout.Height(22));
if(target.objectScript.OOCCDOCCDC[0] == true && target.objectScript.OOCOQDCQQQ[0] == false) {
target.objectScript.OOCDQQDODQ();
target.objectScript.OOCCDOCCDC[0] = true;  target.objectScript.OOCOQDCQQQ[0] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.OOCCDOCCDC[1]  = GUILayout.Toggle(target.objectScript.OOCCDOCCDC[1] ,new GUIContent("", " Insert road markers. "),"insertMarkers",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.OOCCDOCCDC[1] == true && target.objectScript.OOCOQDCQQQ[1] == false) {
target.objectScript.OOCDQQDODQ();
target.objectScript.OOCCDOCCDC[1] = true;  target.objectScript.OOCOQDCQQQ[1] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.OOCCDOCCDC[2]  = GUILayout.Toggle(target.objectScript.OOCCDOCCDC[2] ,new GUIContent("", " Process the terrain and create road geometry. "),"terrain",GUILayout.Width(40),GUILayout.Height(22));

if(target.objectScript.OOCCDOCCDC[2] == true && target.objectScript.OOCOQDCQQQ[2] == false) {
if(target.objectScript.markers < 2){
EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
target.objectScript.OOCCDOCCDC[2] = false;
}else{
target.objectScript.OOCCDOCCDC[2] = false;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;





}
}
if(target.objectScript.OOCCDOCCDC[2] == false && target.objectScript.OOCOQDCQQQ[2] == true){

target.objectScript.OOCOQDCQQQ[2] = false;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.OOCCDOCCDC[3]  = GUILayout.Toggle(target.objectScript.OOCCDOCCDC[3] ,new GUIContent("", " General settings. "),"settings",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.OOCCDOCCDC[3] == true && target.objectScript.OOCOQDCQQQ[3] == false) {
target.objectScript.OOCDQQDODQ();
target.objectScript.OOCCDOCCDC[3] = true;  target.objectScript.OOCOQDCQQQ[3] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.OOCCDOCCDC[4]  = GUILayout.Toggle(target.objectScript.OOCCDOCCDC[4] ,new GUIContent("", "Version and Purchase Info"),"info",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.OOCCDOCCDC[4] == true && target.objectScript.OOCOQDCQQQ[4] == false) {
target.objectScript.OOCDQQDODQ();
target.objectScript.OOCCDOCCDC[4] = true;  target.objectScript.OOCOQDCQQQ[4] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.skin = null;
target.objectScript.OQOQOOOQQO = -1;
for(var i : int  = 0; i < 5; i++){
if(target.objectScript.OOCCDOCCDC[i]){
target.objectScript.OQOQOOOQQO = i;
target.objectScript.OCCDDDDDCD = i;
}
}
if(target.objectScript.OQOQOOOQQO == -1) target.objectScript.OOCDQQDODQ();
var markerMenuDisplay : int = 1;
if(target.objectScript.OQOQOOOQQO == 0 || target.objectScript.OQOQOOOQQO == 1) markerMenuDisplay = 0;
else if(target.objectScript.OQOQOOOQQO == 2 || target.objectScript.OQOQOOOQQO == 3 || target.objectScript.OQOQOOOQQO == 4) markerMenuDisplay = 0;
if(target.objectScript.OCCDOQDQQD && !target.objectScript.OOCCDOCCDC[2] && !target.objectScript.ODODDQOO){
target.objectScript.OCCDOQDQQD = false;
if(target.objectScript.objectType != 2)target.objectScript.OQODQOQOOQ();
if(target.objectScript.objectType == 2 && target.objectScript.OCCDOQDQQD){
Debug.Log("restore");
target.objectScript.OODODOQQOD.ODCDQOOOOO(target.transform, true);
}
}
GUI.skin.box.alignment = TextAnchor.UpperLeft;
if(target.objectScript.OQOQOOOQQO >= 0 && target.objectScript.OQOQOOOQQO != 4){
if(target.objectScript.OQDODQDOCO == null || target.objectScript.OQDODQDOCO.Length == 0){

target.objectScript.OQDODQDOCO = target.objectScript.OODODOQQOD.ODQOOCQCQQ();
target.objectScript.ODODQOQO = target.objectScript.OODODOQQOD.OOCCQODDOD();
target.objectScript.ODODQOQOInt = target.objectScript.OODODOQQOD.OCOCDOODOC();
}
EditorGUILayout.BeginHorizontal();
GUILayout.Box(target.objectScript.OQDODQDOCO[target.objectScript.OQOQOOOQQO], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
}
return target.objectScript.OQOQOOOQQO;
}
function ERMarkerGUI( markerScript : MarkerScript){
EditorGUILayout.Space();
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField(" Marker: " + (target.markerNum + 1).ToString(), EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
EditorGUILayout.Space();
if(target.distance == "-1" && target.objectScript.OODODOQQOD != null){
var arr = target.objectScript.OODODOQQOD.ODODCOOQCO(target.markerNum);
target.distance = arr[0];
target.ODCDDOCCDD = arr[1];
target.OQOOCQQDOO = arr[2];
}
GUILayout.Label("   Total Distance to Marker: " + target.distance.ToString() + " km");
GUILayout.Label("   Segment Distance to Marker: " + target.ODCDDOCCDD.ToString() + " km");
GUILayout.Label("   Marker Distance: " + target.OQOOCQQDOO.ToString() + " m");
EditorGUILayout.Space();
EditorGUILayout.BeginVertical("Box");
EditorGUILayout.LabelField(" Marker Settings", EditorStyles.boldLabel);
EditorGUILayout.EndVertical();
var oldss : boolean = markerScript.OOOCCQCDQO;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Soft Selection", "When selected, the settings of other road markers within the selected distance will change according their distance to this marker."),  GUILayout.Width(105));
GUI.SetNextControlName ("OOOCCQCDQO");
markerScript.OOOCCQCDQO = EditorGUILayout.Toggle (markerScript.OOOCCQCDQO);
EditorGUILayout.EndHorizontal();
if(markerScript.OOOCCQCDQO){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("         Distance", "The soft selection distance within other markers should change too."),  GUILayout.Width(105));
markerScript.OCCQCQOCQQ = EditorGUILayout.Slider(markerScript.OCCQCQOCQQ, 0, 500);
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
}
if(oldss != markerScript.OCCQCQOCQQ) target.objectScript.ResetMaterials(markerScript);
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Left Indent", "The distance from the left side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
GUI.SetNextControlName ("ri");
oldfl = markerScript.ri;
markerScript.ri = EditorGUILayout.Slider(markerScript.ri, target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.ri){
target.objectScript.OCOCDCCCDQ("ri", markerScript);
markerScript.OOQOQQOO = markerScript.ri;
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Indent", "The distance from the right side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
oldfl = markerScript.li;
markerScript.li =  EditorGUILayout.Slider(markerScript.li, target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.li){
target.objectScript.OCOCDCCCDQ("li", markerScript);
markerScript.ODODQQOO = markerScript.li;
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Left Surrounding", "This represents the distance over which the terrain will be gradually leveled on the left side of the road to the original terrain height"),  GUILayout.Width(105));
oldfl = markerScript.rs;
GUI.SetNextControlName ("rs");
markerScript.rs = EditorGUILayout.Slider(markerScript.rs,  target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.rs){
target.objectScript.OCOCDCCCDQ("rs", markerScript);
markerScript.ODOQQOOO = markerScript.rs;
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Surrounding", "This represents the distance over which the terrain will be gradually leveled on the right side of the road to the original terrain height"),  GUILayout.Width(105));
oldfl = markerScript.ls;
markerScript.ls = EditorGUILayout.Slider(markerScript.ls,  target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.ls){
target.objectScript.OCOCDCCCDQ("ls", markerScript);
markerScript.DODOQQOO = markerScript.ls;
}
if(target.objectScript.objectType == 0){
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
oldfl = markerScript.rt;
GUILayout.Label(new GUIContent("    Left Tilting", "Use this setting to tilt the road on the left side (m)."),  GUILayout.Width(105));
markerScript.qt = EditorGUILayout.Slider(markerScript.qt, 0, target.objectScript.roadWidth * 0.5f);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.rt){
target.objectScript.OCOCDCCCDQ("rt", markerScript);
markerScript.ODDQODOO = markerScript.rt;
}
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Tilting", "Use this setting to tilt the road on the right side (cm)."),  GUILayout.Width(105));
oldfl = markerScript.lt;
markerScript.lt = EditorGUILayout.Slider(markerScript.lt, 0, target.objectScript.roadWidth * 0.5f);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.lt){
target.objectScript.OCOCDCCCDQ("lt", markerScript);
markerScript.ODDOQOQQ = markerScript.lt;
}
GUI.enabled = true;
if(target.markerInt < 2){
GUILayout.Label(new GUIContent("    Bridge Objects are disabled for the first two markers!", ""));
}else{
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Bridge Object", "When selected this road segment will be treated as a bridge segment."),  GUILayout.Width(105));
GUI.SetNextControlName ("bridgeObject");
markerScript.bridgeObject = EditorGUILayout.Toggle (markerScript.bridgeObject);
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
if(markerScript.bridgeObject){
GUILayout.Label(new GUIContent("      Distribute Heights", "When selected the terrain, the terrain will be gradually leveled between the height of this road segment and the current terrain height of the inner bridge segment."),  GUILayout.Width(135));
GUI.SetNextControlName ("distHeights");
markerScript.distHeights = EditorGUILayout.Toggle (markerScript.distHeights);
}
EditorGUILayout.EndHorizontal();
}
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Tunnel Object", "When selected this road segment will be treated as a tunnel segment."),  GUILayout.Width(105));
GUI.SetNextControlName ("bridgeObject");
markerScript.tunnelFlag = EditorGUILayout.Toggle (markerScript.tunnelFlag);
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
}else{
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Floor Depth", "Use this setting to change the floor depth for this marker."),  GUILayout.Width(105));
oldfl = markerScript.floorDepth;
markerScript.floorDepth = EditorGUILayout.Slider(markerScript.floorDepth, 0, 50);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.floorDepth){
target.objectScript.OCOCDCCCDQ("floorDepth", markerScript);
markerScript.oldFloorDepth = markerScript.floorDepth;
}
}
EditorGUILayout.Space();
GUI.enabled = false;
if(target.objectScript.objectType == 0){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Start New LOD Segment", "Use this to split the road or river object in segments to use in LOD system."),  GUILayout.Width(170));
markerScript.newSegment = EditorGUILayout.Toggle (markerScript.newSegment);
EditorGUILayout.EndHorizontal();
}
GUI.enabled = true;
EditorGUILayout.Space();
if(!markerScript.autoUpdate){
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Refresh Surface", GUILayout.Width(225))){
target.objectScript.OCCQDQQDCD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
}
if (GUI.changed && !target.objectScript.ODDQDDODOC){
target.objectScript.ODDQDDODOC = true;
}else if(target.objectScript.ODDQDDODOC){
target.objectScript.OOQCDDQCQD(markerScript);
target.objectScript.ODDQDDODOC = false;
SceneView.lastActiveSceneView.Repaint();
}
oldfl = markerScript.rs;
}
function MSMarkerGUI( markerScript : MarkerScript){
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XYZ", "Click to distribute the x, y and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OODODOQQOD.OQQOCODODQ(target.objectScript.OCCQQCQQOQs, 0);
target.objectScript.OCCQDQQDCD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XZ", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OODODOQQOD.OQQOCODODQ(target.objectScript.OCCQQCQQOQs, 1);
target.objectScript.OCCQDQQDCD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XZ  Snap Y", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers and snap the y value to the terrain height at the new position."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OODODOQQOD.OQQOCODODQ(target.objectScript.OCCQQCQQOQs, 2);
target.objectScript.OCCQDQQDCD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Average Heights ", "Click to distribute the heights all markers in between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OODODOQQOD.OQQOCODODQ(target.objectScript.OCCQQCQQOQs, 3);
target.objectScript.OCCQDQQDCD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.Space();
}
function TRMarkerGUI(markerScript : MarkerScript){
EditorGUILayout.Space();
}
function MarkerOnScene(markerScript : MarkerScript){
var cEvent : Event = Event.current;

if(!target.objectScript.ODODDDOO || target.objectScript.objectType == 2){
if(cEvent.shift && (target.objectScript.OCCDDDDDCD == 0 || target.objectScript.OCCDDDDDCD == 1)) {
Selection.activeGameObject =  markerScript.transform.parent.parent.gameObject;
}else if(cEvent.shift && target.objectScript.OCCQQCQQOQ != target.transform){
target.objectScript.ODDCDCOQQQ(markerScript);
Selection.objects = target.objectScript.OCCQQCQQOQs;
}else if(target.objectScript.OCCQQCQQOQ != target.transform && count == 0){
if(!target.InSelected()){
target.objectScript.OCCQQCQQOQs = new GameObject[0];
target.objectScript.ODDCDCOQQQ(markerScript);
Selection.objects = target.objectScript.OCCQQCQQOQs;


count++;
}

}else{

if(cEvent.control && !cEvent.alt)target.snapMarker = true;
else target.snapMarker = false;

pos = markerScript.oldPos;
if(pos  != oldPos && !markerScript.changed){
oldPos = pos;
if(!cEvent.shift){
target.objectScript.OCDODQOQCC(markerScript);
}
}
}
if(cEvent.shift && markerScript.changed){
OCDQCCQDDD = true;
}
markerScript.changed = false;
if(!cEvent.shift && OCDQCCQDDD){
target.objectScript.OCDODQOQCC(markerScript);
OCDQCCQDDD = false;
}
}

}
}
