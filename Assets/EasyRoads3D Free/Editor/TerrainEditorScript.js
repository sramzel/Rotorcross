@CustomEditor(EasyRoads3DTerrainID)
class TerrainEditorScript extends Editor
{
function OnSceneGUI()
{
if(Event.current.shift && RoadObjectScript.OCCDOQCCQQ != null) Selection.activeGameObject = RoadObjectScript.OCCDOQCCQQ.gameObject;
else RoadObjectScript.OCCDOQCCQQ = null;
}
}
