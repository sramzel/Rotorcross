using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;
using EasyRoads3D;

public class RoadObjectScript : MonoBehaviour {
static public string version = "";
public int objectType = 0;
public bool displayRoad = true;
public float roadWidth = 5.0f;
public float indent = 3.0f;
public float surrounding = 5.0f;
public float raise = 1.0f;
public float raiseMarkers = 0.5f;
public bool OOQDOOQQ = false;
public bool renderRoad = true;
public bool beveledRoad = false;
public bool applySplatmap = false;
public int splatmapLayer = 4;
public bool autoUpdate = true;
public float geoResolution = 5.0f;
public int roadResolution = 1;
public float tuw =  15.0f;
public int splatmapSmoothLevel;
public float opacity = 1.0f;
public int expand = 0;
public int offsetX = 0;
public int offsetY = 0;
private Material surfaceMaterial;
public float surfaceOpacity = 1.0f;
public float smoothDistance = 1.0f;
public float smoothSurDistance = 3.0f;
private bool handleInsertFlag;
public bool handleVegetation = true;
public float OCQCQQOCCQ = 2.0f;
public float ODCOOQCCCQ = 1f;
public int materialType = 0;
String[] materialStrings;
public string uname;
public string email;
private MarkerScript[] mSc;

private bool OQQOOODDCO;
private bool[] OOCCDOCCDC = null;
private bool[] OOCOQDCQQQ = null;
public string[] OQDODQDOCO;
public string[] ODODQOQO;
public int[] ODODQOQOInt;
public int OQOQOOOQQO = -1;
public int OCCDDDDDCD = -1;
static public GUISkin OOOODDDODC;
static public GUISkin OCOQCDCDCO;
public bool OQOQOOOCQO = false;
private Vector3 cPos;
private Vector3 ePos;
public bool OQQODCQODC;
static public Texture2D OOOCCQQQOC;
public int markers = 1;
public OQQDCCCQDQ OODODOQQOD;
private GameObject ODOQDQOO;
public bool OCCDOQDQQD;
public bool doTerrain;
private Transform OCCQQCQQOQ = null;
public GameObject[] OCCQQCQQOQs;
private static string OCOCCQOCDD = null;
public Transform obj;
private string OQQODQQDOO;
public static string erInit = "";
static public Transform OCCDOQCCQQ;
private RoadObjectScript OQQOQOOOOO;
public bool flyby;


private Vector3 pos;
private float fl;
private float oldfl;
private bool ODDQDDODOC;
private bool OCDQCCQDDD;
private bool ODDQDDQOCO;
public Transform OOODODOQDC;
public int OdQODQOD = 1;
public float OOQQQDOD = 0f;
public float OOQQQDODOffset = 0f;
public float OOQQQDODLength = 0f;
public bool ODODDDOO = false;
static public string[] ODOQDOQO;
static public string[] ODODOQQO; 
static public string[] ODODQOOQ;
public int ODQDOOQO = 0;
public string[] ODQQQQQO;  
public string[] ODODDQOO; 
public bool[] ODODQQOD; 
public int[] OOQQQOQO; 
public int ODOQOOQO = 0; 

public bool forceY = false;
public float yChange = 0f;
public float floorDepth = 2f;
public float waterLevel = 1.5f; 
public bool lockWaterLevel = true;
public float lastY = 0f;
public string distance = "0";
public string markerDisplayStr = "Hide Markers";
static public string[] objectStrings;
public string objectText = "Road";
public bool applyAnimation = false;
public float waveSize = 1.5f;
public float waveHeight = 0.15f;
public bool snapY = true;

private TextAnchor origAnchor;
public bool autoODODDQQO;
public Texture2D roadTexture;
public Texture2D roadMaterial;
public string[] OOQOOQCDDQ;
public string[] OOQCDQCQCQ;
public int selectedWaterMaterial;
public int selectedWaterScript;
private bool doRestore = false;
public bool doFlyOver;
public static GameObject tracer;
public Camera goCam;
public float speed = 1f;
public float offset = 0f;
public bool camInit;
public GameObject customMesh = null;
static public bool disableFreeAlerts = true;
public bool multipleTerrains;
public bool editRestore = true;
public Material roadMaterialEdit;
static public int backupLocation = 0;
public string[] backupStrings = new string[2]{"Outside Assets folder path","Inside Assets folder path"};
public Vector3[] leftVecs = new Vector3[0];
public Vector3[] rightVecs = new Vector3[0];
public bool applyTangents = false;
public bool sosBuild = false;
public float splinePos = 0;
public float camHeight = 3;
public Vector3 splinePosV3 = Vector3.zero;
public bool blendFlag; 
public float startBlendDistance = 5;
public float endBlendDistance = 5;
public bool iOS = false;
static public string extensionPath = "";
public void OOCCOODQQD(List<ODODDQQO> arr, String[] DOODQOQO, String[] OODDQOQO){

ODOOCOCCQQ(transform, arr, DOODQOQO, OODDQOQO);
}
public void ODDCDCOQQQ(MarkerScript markerScript){

OCCQQCQQOQ = markerScript.transform;



List<GameObject> tmp = new List<GameObject>();
for(int i=0;i<OCCQQCQQOQs.Length;i++){
if(OCCQQCQQOQs[i] != markerScript.gameObject)tmp.Add(OCCQQCQQOQs[i]);
}




tmp.Add(markerScript.gameObject);
OCCQQCQQOQs = tmp.ToArray();
OCCQQCQQOQ = markerScript.transform;

OODODOQQOD.OCODQOQQDQ(OCCQQCQQOQ, OCCQQCQQOQs, markerScript.OOOCCQCDQO, markerScript.OCCQCQOCQQ, OOODODOQDC, out markerScript.OCCQQCQQOQs, out markerScript.trperc, OCCQQCQQOQs);

OCCDDDDDCD = -1;
}
public void OOQCDDQCQD(MarkerScript markerScript){
if(markerScript.OCCQCQOCQQ != markerScript.ODOOQQOO || markerScript.OCCQCQOCQQ != markerScript.ODOOQQOO){
OODODOQQOD.OCODQOQQDQ(OCCQQCQQOQ, OCCQQCQQOQs, markerScript.OOOCCQCDQO, markerScript.OCCQCQOCQQ, OOODODOQDC, out markerScript.OCCQQCQQOQs, out markerScript.trperc, OCCQQCQQOQs);
markerScript.ODQDOQOO = markerScript.OOOCCQCDQO;
markerScript.ODOOQQOO = markerScript.OCCQCQOCQQ;
}
if(OQQOQOOOOO.autoUpdate) OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
}
public void ResetMaterials(MarkerScript markerScript){
if(OODODOQQOD != null)OODODOQQOD.OCODQOQQDQ(OCCQQCQQOQ, OCCQQCQQOQs, markerScript.OOOCCQCDQO, markerScript.OCCQCQOCQQ, OOODODOQDC, out markerScript.OCCQQCQQOQs, out markerScript.trperc, OCCQQCQQOQs);
}
public void OCDODQOQCC(MarkerScript markerScript){
if(markerScript.OCCQCQOCQQ != markerScript.ODOOQQOO){
OODODOQQOD.OCODQOQQDQ(OCCQQCQQOQ, OCCQQCQQOQs, markerScript.OOOCCQCDQO, markerScript.OCCQCQOCQQ, OOODODOQDC, out markerScript.OCCQQCQQOQs, out markerScript.trperc, OCCQQCQQOQs);
markerScript.ODOOQQOO = markerScript.OCCQCQOCQQ;
}
OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
}
private void OCOCDCCCDQ(string ctrl, MarkerScript markerScript){
int i = 0;
foreach(Transform tr in markerScript.OCCQQCQQOQs){
MarkerScript wsScript = (MarkerScript) tr.GetComponent<MarkerScript>();
if(ctrl == "rs") wsScript.LeftSurrounding(markerScript.rs - markerScript.ODOQQOOO, markerScript.trperc[i]);
else if(ctrl == "ls") wsScript.RightSurrounding(markerScript.ls - markerScript.DODOQQOO, markerScript.trperc[i]);
else if(ctrl == "ri") wsScript.LeftIndent(markerScript.ri - markerScript.OOQOQQOO, markerScript.trperc[i]);
else if(ctrl == "li") wsScript.RightIndent(markerScript.li - markerScript.ODODQQOO, markerScript.trperc[i]);
else if(ctrl == "rt") wsScript.LeftTilting(markerScript.rt - markerScript.ODDQODOO, markerScript.trperc[i]);
else if(ctrl == "lt") wsScript.RightTilting(markerScript.lt - markerScript.ODDOQOQQ, markerScript.trperc[i]);
else if(ctrl == "floorDepth") wsScript.FloorDepth(markerScript.floorDepth - markerScript.oldFloorDepth, markerScript.trperc[i]);
i++;
}
}
public void OCCQDQQDCD(){
if(markers > 1) OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
}
public void ODOOCOCCQQ(Transform tr, List<ODODDQQO> arr, String[] DOODQOQO, String[] OODDQOQO){
version = "2.5.7";
OOOODDDODC = (GUISkin)Resources.Load("ER3DSkin", typeof(GUISkin));


OOOCCQQQOC = (Texture2D)Resources.Load("ER3DLogo", typeof(Texture2D));
if(RoadObjectScript.objectStrings == null){
RoadObjectScript.objectStrings = new string[3];
RoadObjectScript.objectStrings[0] = "Road Object"; RoadObjectScript.objectStrings[1]="River Object";RoadObjectScript.objectStrings[2]="Procedural Mesh Object";
}
obj = tr;
OODODOQQOD = new OQQDCCCQDQ();
OQQOQOOOOO = obj.GetComponent<RoadObjectScript>();
foreach(Transform child in obj){
if(child.name == "Markers") OOODODOQDC = child;
}
RoadObjectScript[] rscrpts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
OQQDCCCQDQ.terrainList.Clear();
Terrain[] terrains = (Terrain[])FindObjectsOfType(typeof(Terrain));
foreach(Terrain terrain in terrains) {
Terrains t = new Terrains();
t.terrain = terrain;
if(!terrain.gameObject.GetComponent<EasyRoads3DTerrainID>()){
EasyRoads3DTerrainID terrainscript = (EasyRoads3DTerrainID)terrain.gameObject.AddComponent<EasyRoads3DTerrainID>();
string id = UnityEngine.Random.Range(100000000,999999999).ToString();
terrainscript.terrainid = id;
t.id = id;
}else{
t.id = terrain.gameObject.GetComponent<EasyRoads3DTerrainID>().terrainid;
}
OQQDCCCQDQ.OOCDCOQQCD(t);
}
OQOCOOCCQO.OOCDCOQQCD();
if(roadMaterialEdit == null){
roadMaterialEdit = (Material)Resources.Load("materials/roadMaterialEdit", typeof(Material));
}
if(objectType == 0 && GameObject.Find(gameObject.name + "/road") == null){
GameObject road = new GameObject("road");
road.transform.parent = transform;
}

OODODOQQOD.OCOOQCDOOD(obj, OCOCCQOCDD, OQQOQOOOOO.roadWidth, surfaceOpacity, out OQQODCQODC, out indent, applyAnimation, waveSize, waveHeight);
OODODOQQOD.ODCOOQCCCQ = ODCOOQCCCQ;
OODODOQQOD.OCQCQQOCCQ = OCQCQQOCCQ;
OODODOQQOD.OdQODQOD = OdQODQOD + 1;
OODODOQQOD.OOQQQDOD = OOQQQDOD;
OODODOQQOD.OOQQQDODOffset = OOQQQDODOffset;
OODODOQQOD.OOQQQDODLength = OOQQQDODLength;
OODODOQQOD.objectType = objectType;
OODODOQQOD.snapY = snapY;
OODODOQQOD.terrainRendered = OCCDOQDQQD;
OODODOQQOD.handleVegetation = handleVegetation;
OODODOQQOD.raise = raise;
OODODOQQOD.roadResolution = roadResolution;
OODODOQQOD.multipleTerrains = multipleTerrains;
OODODOQQOD.editRestore = editRestore;
OODODOQQOD.roadMaterialEdit = roadMaterialEdit;
OODODOQQOD.renderRoad = renderRoad;
OODODOQQOD.rscrpts = rscrpts.Length;
OODODOQQOD.blendFlag = blendFlag; 
OODODOQQOD.startBlendDistance = startBlendDistance;
OODODOQQOD.endBlendDistance = endBlendDistance;
if(backupLocation == 0)OOCCCOCCCC.backupFolder = "/EasyRoads3D";
else OOCCCOCCCC.backupFolder =  OOCCCOCCCC.extensionPath + "/Backups";

ODODQOQO = OODODOQQOD.OOCCQODDOD();
ODODQOQOInt = OODODOQQOD.OCOCDOODOC();


if(OCCDOQDQQD){




doRestore = true;
}


ODDOOCQODD();

if(arr != null || ODODQOOQ == null) OQQCOCCCQO(arr, DOODQOQO, OODDQOQO);


if(doRestore) return;
}
public void UpdateBackupFolder(){
}
public void OOCDQQDODQ(){
if(!ODODDDOO || objectType == 2){
if(OOCCDOCCDC != null){
for(int i = 0; i < OOCCDOCCDC.Length; i++){
OOCCDOCCDC[i] = false;
OOCOQDCQQQ[i] = false;
}
}
}
}

public void OQCCDQOOCQ(Vector3 pos){


if(!displayRoad){
displayRoad = true;
OODODOQQOD.OQOODQOQCQ(displayRoad, OOODODOQDC);
}
pos.y += OQQOQOOOOO.raiseMarkers;
if(forceY && ODOQDQOO != null){
float dist = Vector3.Distance(pos, ODOQDQOO.transform.position);
pos.y = ODOQDQOO.transform.position.y + (yChange * (dist / 100f));
}else if(forceY && markers == 0) lastY = pos.y;
GameObject go = null;
if(ODOQDQOO != null) go = (GameObject)Instantiate(ODOQDQOO);
else go = (GameObject)Instantiate(Resources.Load("marker", typeof(GameObject)));
Transform newnode = go.transform;
newnode.position = pos;
newnode.parent = OOODODOQDC;
markers++;
string n;
if(markers < 10) n = "Marker000" + markers.ToString();
else if (markers < 100) n = "Marker00" + markers.ToString();
else n = "Marker0" + markers.ToString();
newnode.gameObject.name = n;
MarkerScript scr = newnode.GetComponent<MarkerScript>();
scr.OQQODCQODC = false;
scr.objectScript = obj.GetComponent<RoadObjectScript>();
if(ODOQDQOO == null){
scr.waterLevel = OQQOQOOOOO.waterLevel;
scr.floorDepth = OQQOQOOOOO.floorDepth;
scr.ri = OQQOQOOOOO.indent;
scr.li = OQQOQOOOOO.indent;
scr.rs = OQQOQOOOOO.surrounding;
scr.ls = OQQOQOOOOO.surrounding;
scr.tension = 0.5f;
if(objectType == 1){

pos.y -= waterLevel;
newnode.position = pos;
}
}
if(objectType == 2){
#if UNITY_3_5
if(scr.surface != null)scr.surface.gameObject.active = false;
#else
if(scr.surface != null)scr.surface.gameObject.SetActive(false);
#endif
}
ODOQDQOO = newnode.gameObject;
if(markers > 1){
OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
if(materialType == 0){

OODODOQQOD.ODOCQCOCDQ(materialType);

}
}
}
public void OCQDOCOQDO(float geo, bool renderMode, bool camMode){
OODODOQQOD.OOQOQQDOOO.Clear();
int ii = 0;
OCQQOCQOOC k;
foreach(Transform child  in obj)
{
if(child.name == "Markers"){
foreach(Transform marker   in child) {
MarkerScript markerScript = marker.GetComponent<MarkerScript>();
markerScript.objectScript = obj.GetComponent<RoadObjectScript>();
if(!markerScript.OQQODCQODC) markerScript.OQQODCQODC = OODODOQQOD.OQDQDDOOOQ(marker);
k  = new OCQQOCQOOC();
k.position = marker.position;
k.num = OODODOQQOD.OOQOQQDOOO.Count;
k.object1 = marker;
k.object2 = markerScript.surface;
k.tension = markerScript.tension;
k.ri = markerScript.ri;
if(k.ri < 1)k.ri = 1f;
k.li =markerScript.li;
if(k.li < 1)k.li = 1f;
k.rt = markerScript.rt;
k.lt = markerScript.lt;
k.rs = markerScript.rs;
if(k.rs < 1)k.rs = 1f;
k.ODDDODDDCO = markerScript.rs;
k.ls = markerScript.ls;
if(k.ls < 1)k.ls = 1f;
k.OQOQCDDCDD = markerScript.ls;
k.renderFlag = markerScript.bridgeObject;
k.OCCCQQCCDQ = markerScript.distHeights;
k.newSegment = markerScript.newSegment;
k.tunnelFlag = markerScript.tunnelFlag;
k.floorDepth = markerScript.floorDepth;
k.waterLevel = waterLevel;
k.lockWaterLevel = markerScript.lockWaterLevel;
k.sharpCorner = markerScript.sharpCorner;
k.OOCQQQOOOC = OODODOQQOD;
markerScript.markerNum = ii;
markerScript.distance = "-1";
markerScript.OQOOCQQDOO = "-1";
OODODOQQOD.OOQOQQDOOO.Add(k);
ii++;
}
}
}
distance = "-1";

OODODOQQOD.ODCQDOCCCQ = OQQOQOOOOO.roadWidth;

OODODOQQOD.OQCDOODOOQ(geo, obj, OQQOQOOOOO.OOQDOOQQ, renderMode, camMode, objectType);
if(OODODOQQOD.leftVecs.Count > 0){
leftVecs = OODODOQQOD.leftVecs.ToArray();
rightVecs = OODODOQQOD.rightVecs.ToArray();
}
}
public void StartCam(){

OCQDOCOQDO(0.5f, false, true);

}
public void ODDOOCQODD(){
int i = 0;
foreach(Transform child  in obj)
{
if(child.name == "Markers"){
i = 1;
string n;
foreach(Transform marker in child) {
if(i < 10) n = "Marker000" + i.ToString();
else if (i < 100) n = "Marker00" + i.ToString();
else n = "Marker0" + i.ToString();
marker.name = n;
ODOQDQOO = marker.gameObject;
i++;
}
}
}
markers = i - 1;

OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
}
public List<Transform> RebuildObjs(){
RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
List<Transform> rObj = new List<Transform>();
foreach (RoadObjectScript script in scripts) {
if(script.transform != transform) rObj.Add(script.transform);
}
return rObj;
}
public void RestoreTerrain1(){

OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
if(OODODOQQOD != null) OODODOQQOD.OQODQOQOOQ();
ODODDDOO = false;
}
public void OOCQDDOQOC(){
OODODOQQOD.OOCQDDOQOC(OQQOQOOOOO.applySplatmap, OQQOQOOOOO.splatmapSmoothLevel, OQQOQOOOOO.renderRoad, OQQOQOOOOO.tuw, OQQOQOOOOO.roadResolution, OQQOQOOOOO.raise, OQQOQOOOOO.opacity, OQQOQOOOOO.expand, OQQOQOOOOO.offsetX, OQQOQOOOOO.offsetY, OQQOQOOOOO.beveledRoad, OQQOQOOOOO.splatmapLayer, OQQOQOOOOO.OdQODQOD, OOQQQDOD, OOQQQDODOffset, OOQQQDODLength);
}
public void OQQDOQDOQD(){
OODODOQQOD.OQQDOQDOQD(OQQOQOOOOO.renderRoad, OQQOQOOOOO.tuw, OQQOQOOOOO.roadResolution, OQQOQOOOOO.raise, OQQOQOOOOO.beveledRoad, OQQOQOOOOO.OdQODQOD, OOQQQDOD, OOQQQDODOffset, OOQQQDODLength);
}
public void OCQOOCOCQD(Vector3 pos, bool doInsert){


if(!displayRoad){
displayRoad = true;
OODODOQQOD.OQOODQOQCQ(displayRoad, OOODODOQDC);
}

int first = -1;
int second = -1;
float dist1 = 10000;
float dist2 = 10000;
Vector3 newpos = pos;
OCQQOCQOOC k;
OCQQOCQOOC k1 = (OCQQOCQOOC)OODODOQQOD.OOQOQQDOOO[0];
OCQQOCQOOC k2 = (OCQQOCQOOC)OODODOQQOD.OOQOQQDOOO[1];

if(doInsert){
Debug.Log("Start Insert" + doInsert);
}
OODODOQQOD.OQCOOOQOCQ(pos, out first, out second, out dist1, out dist2, out k1, out k2, out newpos, doInsert);
if(doInsert){
Debug.Log("marker 1: " + first);
Debug.Log("marker 2: " + second);
}
pos = newpos;
if(doInsert && first >= 0 && second >= 0){
if(OQQOQOOOOO.OOQDOOQQ && second == OODODOQQOD.OOQOQQDOOO.Count - 1){
OQCCDQOOCQ(pos);
}else{
k = (OCQQOCQOOC)OODODOQQOD.OOQOQQDOOO[second];
string name = k.object1.name;
string n;
int j = second + 2;
for(int i = second; i < OODODOQQOD.OOQOQQDOOO.Count - 1; i++){
k = (OCQQOCQOOC)OODODOQQOD.OOQOQQDOOO[i];
if(j < 10) n = "Marker000" + j.ToString();
else if (j < 100) n = "Marker00" + j.ToString();
else n = "Marker0" + j.ToString();
k.object1.name = n;
j++;
}
k = (OCQQOCQOOC)OODODOQQOD.OOQOQQDOOO[first];
Transform newnode = (Transform)Instantiate(k.object1.transform, pos, k.object1.rotation);
newnode.gameObject.name = name;
newnode.parent = OOODODOQDC;
#if UNITY_4_5
newnode.SetSiblingIndex(second);
#elif UNITY_4_6
newnode.SetSiblingIndex(second);
#elif UNITY_5_0
newnode.SetSiblingIndex(second);
#endif
MarkerScript scr = newnode.GetComponent<MarkerScript>();
scr.OQQODCQODC = false;
float	totalDist = dist1 + dist2;
float perc1 = dist1 / totalDist;
float paramDif = k1.ri - k2.ri;
scr.ri = k1.ri - (paramDif * perc1);
paramDif = k1.li - k2.li;
scr.li = k1.li - (paramDif * perc1);
paramDif = k1.rt - k2.rt;
scr.rt = k1.rt - (paramDif * perc1);
paramDif = k1.lt - k2.lt;
scr.lt = k1.lt - (paramDif * perc1);
paramDif = k1.rs - k2.rs;
scr.rs = k1.rs - (paramDif * perc1);
paramDif = k1.ls - k2.ls;
scr.ls = k1.ls - (paramDif * perc1);
OCQDOCOQDO(OQQOQOOOOO.geoResolution, false, false);
if(materialType == 0)OODODOQQOD.ODOCQCOCDQ(materialType);
#if UNITY_3_5
if(objectType == 2) scr.surface.gameObject.active = false;
#else
if(objectType == 2) scr.surface.gameObject.SetActive(false);
#endif
}
}
ODDOOCQODD();
}
public void OOCOOODQCQ(){

DestroyImmediate(OQQOQOOOOO.OCCQQCQQOQ.gameObject);
OCCQQCQQOQ = null;
ODDOOCQODD();
}
public void ODQCDOCCDO(){
}

public List<SideObjectParams> OCQDODOCOC(){
		return  null;
}
public void OQQQQDDCOO(){
}
public void OQQCOCCCQO(List<ODODDQQO> arr, String[] DOODQOQO, String[] OODDQOQO){
}
public void SetMultipleTerrains(bool flag){
RoadObjectScript[] scrpts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
foreach(RoadObjectScript scr in scrpts){
scr.multipleTerrains = flag;
if(scr.OODODOQQOD != null)scr.OODODOQQOD.multipleTerrains = flag;
}
}
public bool CheckWaterHeights(){
if(OQOCOOCCQO.terrain == null) return false;
bool flag = true;

float y = OQOCOOCCQO.terrain.transform.position.y;
foreach(Transform child  in obj) {
if(child.name == "Markers"){
foreach(Transform marker  in child) {

if(marker.position.y - y <= 0.1f) flag = false;
}
}
}
return flag;
}
}
