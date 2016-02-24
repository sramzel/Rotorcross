using UnityEngine;
using System.Collections;

public class SurfaceScript : MonoBehaviour {
	void Start () {
		Material mat;
		if(transform.parent.GetComponent<MarkerScript>().objectScript.materialType == 0) mat = (Material)MonoBehaviour.Instantiate(Resources.Load("surfaceMaterial", typeof(Material)));
		else mat = (Material)MonoBehaviour.Instantiate(Resources.Load("surfaceAlphaMaterial", typeof(Material)));
		Color c = mat.color;
		c.a = transform.parent.GetComponent<MarkerScript>().objectScript.surfaceOpacity;
		gameObject.GetComponent<MeshRenderer>().sharedMaterial = mat;
	}	
}
