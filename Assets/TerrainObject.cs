using UnityEngine;
using System.Collections;

public class TerrainObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Terrain terrain = GetComponent<Terrain> ();
		align (terrain, transform);
	}

	void align (Terrain terrain, Transform transform)
	{
		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild (i);
			var position = child.position;
			position.y = terrain.transform.position.y + terrain.SampleHeight (position);
			child.position = position;
			if (child.childCount > 0) {
				align(terrain, child);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
