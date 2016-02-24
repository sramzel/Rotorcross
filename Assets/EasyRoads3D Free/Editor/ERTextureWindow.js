import System.IO;
class ERTextureWindow extends EditorWindow {
public static var instance : ERTextureWindow ;
private var roadTexture : Texture2D;
private var scrollPosition : Vector2;
private var selectedItem : int = 0;
private var shaders1 : String[];
private var shaders2 : String[];
private var shaders3 : String[];
private var roadTextures : Texture2D[];
private var texturePaths : String[];
private var shaderOptions : int[];
private var selInt : int[];
private var dtextures : List.<String>  = new List.<String>();
private var btextures : List.<String>  = new List.<String>();
private var stextures : List.<String>  = new List.<String>();
private var ext : String[] = [".PSD", ".TIFF",".JPG",".TGA",".PNG",".GIF",".BMP",".IFF",".PICT"];

static public var window : ERTextureWindow;
static private var roadscript : RoadObjectScript;
public static var  test : int;
private var assetPath;
static function Init (scr : RoadObjectScript) {

roadscript = scr;
window = EditorWindow.GetWindow (ERTextureWindow);

}
public function ERTextureWindow() {

instance = this;
titleContent = new GUIContent("Road Materials");




}
function OnDestroy(){
instance = null;
}
function OnGUI () {
if(roadTextures == null)GetFiles() ;
if(shaders1 == null){
shaders1 = new String[1];
shaders1[0] = "Diffuse";
shaders2 = new String[2];
shaders2[0] = "Diffuse";shaders2[1] = "Bumpmap Diffuse";
shaders3 = new String[3];
shaders3[0] = "Diffuse";shaders3[1] = "Bumpmap Diffuse";shaders3[2] = "Bumped Specular";
}
var r : Rect = this.position;

var cols : int = Mathf.Floor(r.width / 150f);


scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
GUILayout.Space(10);
var k : int = 0;
var l : int = 0;
for(i = 0; i < 5; i++){
EditorGUILayout.BeginHorizontal();
GUILayout.Space(10);
for(j= 0; j < cols; j++){
if(GUILayout.Button (roadTextures[k], GUILayout.Width(125), GUILayout.Height(125))){
SetMaterial(roadTextures[k], selInt[k], texturePaths[k], k);
}
GUILayout.Space(25);
k++;
if(k >= roadTextures.length) break;
}
EditorGUILayout.EndHorizontal();
GUILayout.Space(1);
EditorGUILayout.BeginHorizontal();
GUILayout.Space(10);
for(j= 0; j < cols; j++){
var oldItem : int = selInt[l];
if(shaderOptions[l] == 1) selInt[l]= EditorGUILayout.Popup (selInt[l], shaders1, EditorStyles.toolbarPopup ,   GUILayout.Width(125) );
else if(shaderOptions[l] == 2) selInt[l]= EditorGUILayout.Popup (selInt[l], shaders2, EditorStyles.toolbarPopup ,   GUILayout.Width(125) );
else if(shaderOptions[l] == 3) selInt[l]= EditorGUILayout.Popup (selInt[l], shaders3, EditorStyles.toolbarPopup ,   GUILayout.Width(125) );
if(oldItem != selInt[l] && selectedItem == l) SetMaterial(roadTextures[l], selInt[l], texturePaths[l], l);
GUILayout.Space(29);
l++;
if(l >= roadTextures.length) break;
}
EditorGUILayout.EndHorizontal();
GUILayout.Space(10);
if(k >= roadTextures.length) break;
}
EditorGUILayout.EndScrollView();
}
function GetFiles() {
var textures : List.<String>  = new List.<String>();

var dir : DirectoryInfo= new DirectoryInfo(Directory.GetCurrentDirectory() + OOCCCOCCCC.extensionPath + "Textures/Road Textures/");

assetPath = OOCCCOCCCC.extensionPath.Substring(1, OOCCCOCCCC.extensionPath.Length - 1);

var extStrings : String[] = new String[1];
extStrings[0] = "*.*";

for(var ext : String in extStrings){
for(var f : FileInfo  in dir.GetFiles(ext)) {
var name : String  = f. Name;
if(InArray(f.Extension)){
textures.Add(name);

}
}
}

dtextures.Clear();
btextures.Clear();
stextures.Clear();
for(file in textures){
if(file.IndexOf("_d.") >= 0 ) dtextures.Add(file);
else if(file.IndexOf("_b.") >= 0 ) btextures.Add(file);
else if(file.IndexOf("_s.") >= 0 ) stextures.Add(file);
else dtextures.Add(file);
}
roadTextures = new Texture2D[dtextures.Count];
texturePaths = new String[dtextures.Count];
shaderOptions = new int[dtextures.Count];
selInt = new int[dtextures.Count];
var i : int = 0;
for(file in dtextures){

//		Debug.Log("/Assets/EasyRoads3D/Textures/Road Textures/" + file);
roadTextures[i] = AssetDatabase.LoadAssetAtPath(assetPath + "Textures/Road Textures/" + file, typeof(Texture2D));

var arr = file.Split("_"[0]);
var extr : String = "";
if(arr.length > 2){
for(l = 0; l < arr.length - 1;l++) extr += arr[l] + "_";
extr = extr.Substring(0, extr.length - 1);
}else{
extr = arr[0];
}

var option : int = 1;
for(fb in btextures){

if(fb.IndexOf(extr ) >= 0){
option++;
option++; 
for(fs in stextures){
if(fs.IndexOf(extr ) >= 0){

break;
}
}
break;
}
}
shaderOptions[i] = option;
texturePaths[i] = file;
i++;
}
}
function InArray(file){
var flag = false;
for(i = 0; i < ext.Length; i++){
if(ext[i] == file.ToUpper()){
flag = true;
break;
}
}
return flag;
}
function SetMaterial(tex, shader, file, item){
//		Debug.Log(shader +" "+file);
selectedItem = item;
var arr = file.Split("_"[0]);
var extr : String = "";
if(arr.length > 2){
for(l = 0; l < arr.length - 1;l++) extr += arr[l] + "_";
extr = extr.Substring(0, extr.length - 1);
}else{
extr = arr[0];
}
mat = Resources.Load("roadMaterial", typeof(Material));
matEdit = Resources.Load("roadMaterialEdit", typeof(Material));
if(shader == 0) mat.shader = Shader.Find ("EasyRoads3D/Diffuse");
else if(shader == 1){
mat.shader = Shader.Find ("EasyRoads3D/Bumped Diffuse");
var btex : String = "";
for(fb in btextures){
if(fb.IndexOf(extr ) >= 0){
btex = fb;
break;
}
}
mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath(assetPath + "Textures/Road Textures/" + btex, typeof(Texture2D)));
}else if(shader == 2){
mat.shader = Shader.Find ("EasyRoads3D/Bumped Specular");
btex = "";
for(fb in btextures){
if(fb.IndexOf(extr ) >= 0){
btex = fb;
break;
}
}
}
roadscript.roadTexture = tex;
mat.mainTexture = tex;
matEdit.mainTexture = tex;
var road : GameObject = roadscript.OODODOQQOD.road;
if(road != null){
if(road.transform.childCount > 0){
for(child in road.transform){
child.gameObject.renderer.material = mat;
}
}else{
road.GetComponent.<Renderer>().material = mat;
}
}
}
}
