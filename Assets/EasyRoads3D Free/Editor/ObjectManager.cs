using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using EasyRoads3D;
using EasyRoads3DEditor;
public class ObjectManager : EditorWindow
{
public string GetExtensionPath(){
string extensionPath  = Path.GetDirectoryName( AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( this ) ) );

extensionPath = extensionPath.Replace("lib", "");
extensionPath = extensionPath.Replace("Editor", "");
extensionPath = extensionPath.Replace("scripts", "");

return "/" + extensionPath;
}
}
