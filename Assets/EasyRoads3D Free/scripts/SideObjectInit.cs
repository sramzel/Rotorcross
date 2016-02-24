using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EasyRoads3D;
public class OQOCCODODC{
	/*
static public void ODCCCDQQDC(RoadObjectScript target){


OOQQDCODCQ.OQDCQQOOOO(target.transform);

List<ODODDQQO> arr = OOQQDCODCQ.ODCDCQOQQQ(false);
target.ODOCCDDQQC(arr, OOQQDCODCQ.OODDODOCCD(arr), OOQQDCODCQ.OOQODQDDCC(arr));
Transform mySideObject = OQOOCODDDQ(target);
OQQODQQOOC(target.OOQCCQOCOQ, target.transform, target.OQOODDDDDC(), target.OOQDOOQQ, target.OOQQQOQO, target.raise, target, mySideObject);



target.ODODDDOO = true;

}
	// ddddddddddddddddddddddddddddddddddddddddd
static public void OQQODQQOOC(OQQDCCCOCQ OOQCCQOCOQ, Transform obj , List<SideObjectParams> param , bool OOQDOOQQ ,  int[] activeODODDQQO , float raise, RoadObjectScript target , Transform mySideObject){
List<OCQCDCDCDD> pnts  = target.OOQCCQOCOQ.OQQOCDQQCO;
List<ODODDQQO> arr  = OOQQDCODCQ.ODCDCQOQQQ(false);
for(int i = 0; i < activeODODDQQO.Length; i++){  
ODODDQQO so = (ODODDQQO)arr[activeODODDQQO[i]];

GameObject goi  = null;

if(so.goPath != "") goi =  (GameObject)Resources.Load(so.goPath, typeof(GameObject));
GameObject ODCOCDDDDQ = null;

if(so.startPath != "") ODCOCDDDDQ =  (GameObject)Resources.Load(so.startPath, typeof(GameObject));
GameObject OCDOODODDQ = null;

if(so.endPath != "") OCDOODODDQ =  (GameObject)Resources.Load(so.endPath, typeof(GameObject));
OOQQDCODCQ.OCDOOCDDCO(so, pnts, obj, OOQCCQOCOQ, param, OOQDOOQQ, activeODODDQQO[i], raise, goi, ODCOCDDDDQ, OCDOODODDQ);
if(so.terrainTree > 0){

if(EditorUtility.DisplayDialog("Side Objects", "Side Object " + so.name + " in " + target.gameObject.name + " includes an asset part of the terrain vegetation data.\n\nWould you like to add this side object to the terrain vegetation data?", "yes","no")){
foreach(Transform child in mySideObject){
if(child.gameObject.name == so.name){

MonoBehaviour.DestroyImmediate(child.gameObject);
break;
}
}
}
}
foreach(Transform child in mySideObject)if(child.gameObject.GetComponent(typeof(sideObjectScript)) != null) MonoBehaviour.DestroyImmediate(child.gameObject.GetComponent(typeof(sideObjectScript)));
}
}

static public void OCQQDQODQO(sideObjectScript scr, int index, RoadObjectScript target, Transform go){
string n = go.gameObject.name;
Transform p = go.parent;

if(go != null){
MonoBehaviour.DestroyImmediate(go.gameObject);
}
List<ODODDQQO> arr = OOQQDCODCQ.ODCDCQOQQQ(false);
ODODDQQO so = (ODODDQQO)arr[index];

OCOCCODDCO(n, p, so, index, target);

GameObject goi  = null;

if(so.goPath != "") goi =  (GameObject)Resources.Load(so.goPath, typeof(GameObject));
GameObject ODCOCDDDDQ = null;

if(so.startPath != "") ODCOCDDDDQ =  (GameObject)Resources.Load(so.startPath, typeof(GameObject));
GameObject OCDOODODDQ = null;

if(so.endPath != "") OCDOODODDQ =  (GameObject)Resources.Load(so.endPath, typeof(GameObject));

OOQQDCODCQ.ODCQODCQCD(target.OOQCCQOCOQ, target.transform, target.OQOODDDDDC(), target.OOQDOOQQ, index, target.raise, goi, ODCOCDDDDQ, OCDOODODDQ);
arr = null;
}

static public Transform OQOOCODDDQ(RoadObjectScript target){

GameObject go  =  new GameObject("Side Objects");

go.transform.parent = target.transform;
List<ODODDQQO> arr = OOQQDCODCQ.ODCDCQOQQQ(false);
for(int i = 0; i < target.OOQQQOQO.Length; i++){  
ODODDQQO so = (ODODDQQO)arr[target.OOQQQOQO[i]];
OCOCCODDCO(so.name, go.transform, so, target.OOQQQOQO[i], target);
}
return go.transform;
}
static public void OCOCCODDCO(string objectname, Transform obj, ODODDQQO so, int index, RoadObjectScript target){



Transform rootObject = null;

foreach(Transform child1 in obj)
{
if(child1.name == objectname){
rootObject = child1;

if(so.texturePath !=""){
MeshRenderer mr  = (MeshRenderer)rootObject.transform.GetComponent(typeof(MeshRenderer));

Material mat =  (Material)Resources.Load(so.texturePath, typeof(Material));
mr.material = mat;



}
}
}
if(rootObject == null){
GameObject go  =  new GameObject(objectname);
go.name = objectname;
go.transform.parent = obj;
rootObject = go.transform;

go.AddComponent(typeof(MeshFilter));
go.AddComponent(typeof(MeshRenderer));
go.AddComponent(typeof(MeshCollider));
go.AddComponent(typeof(sideObjectScript));
sideObjectScript scr = (sideObjectScript)go.GetComponent(typeof(sideObjectScript));
if(so.texturePath !=""){
MeshRenderer mr = (MeshRenderer)go.GetComponent(typeof(MeshRenderer));

Material mat =  (Material)Resources.Load(so.texturePath, typeof(Material));
mr.material = mat;
scr.mat = mat;


}
scr.soIndex = index;
scr.soName = so.name;

scr.soAlign = int.Parse(so.align);
scr.soUVx = so.uvx;
scr.soUVy = so.uvy;
scr.m_distance = so.m_distance;
scr.objectType = so.objectType;
scr.weld = so.weld;
scr.combine = so.combine;
scr.OQOCODCQCC = so.OQOCODCQCC;
scr.m_go = so.ODOOCQQDCC;
if(so.OQQDQDDQQO != ""){
scr.OQQDQDDQQO = so.OQQDQDDQQO;

}
if(so.OQQDQDDQQO != ""){
scr.OQQCDCOOQQ = so.OQQCDCOOQQ;

}
scr.selectedRotation = so.selectedRotation;
scr.position = so.position;
scr.uvInt = so.uvType;
scr.randomObjects = so.randomObjects;
scr.childOrder = so.childOrder;
scr.sidewaysOffset = so.sidewaysOffset;
scr.density = so.density;
scr.OODQQQCDOO = target;
scr.terrainTree = so.terrainTree;
scr.xPosition = so.xPosition;
scr.yPosition = so.yPosition;
scr.uvYRound = so.uvYRound;
scr.m_collider = so.collider;
scr.m_tangents = so.tangents;

}
}
	 */
}
