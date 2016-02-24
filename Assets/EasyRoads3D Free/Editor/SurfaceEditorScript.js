
@CustomEditor(SurfaceScript)
class SurfaceEditorScript extends Editor
{
	function OnSceneGUI()
	{
		if(Event.current.control) Selection.activeGameObject = target.transform.parent.parent.parent.gameObject;
		else Selection.activeGameObject = target.transform.parent.gameObject;
	}
}

