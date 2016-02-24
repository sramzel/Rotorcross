using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Spinning : MonoBehaviour {
	public Vector3 _angularMomentum;
    
	// __ prefix = order 2 tensor. _ = order 1. no underscore = scalar.
	// CONSIDER JUST USING VARIABLE NAME
	private Matrix4x4 __inertiaTensor; 
	
	void Start() {
		// This creates the diagonalised inertia tensor matrix from the Vector3 by the physics engine.
		// Note the physics engine is using the colliders of the object and it's children, and
		// the mass of the parent object to calculate an approximate inertia tensor.
		__inertiaTensor = Matrix4x4.Scale (GetComponent<Rigidbody>().inertiaTensor);
	}
	
	void Update () {if (Input.GetMouseButtonDown(0)) print ("Clicked");}  
	
	void FixedUpdate() {
		CalculateRotation ();
	}

	void CalculateRotation ()
	{
		// Rotation matrix from world axis, to current object axis.
		Matrix4x4 __rotationMatrix = Matrix4x4.TRS (Vector3.zero, transform.rotation, Vector3.one);
		// Transform inertia tensor from global to local (_L' = R _I R^-1 _w').
		Matrix4x4 __intertiaTensorLocal = __rotationMatrix * __inertiaTensor * __rotationMatrix.inverse;
		// Calculate angular velocity by multiplying inverse (_w = _L _I^-1).
		Vector3 _angularVelocity = __intertiaTensorLocal.inverse * _angularMomentum;
		Vector3 _axis = _angularVelocity.normalized;
		float speed = _angularVelocity.magnitude;
		float degreesThisFrame = Time.deltaTime * Mathf.Rad2Deg;
		transform.RotateAround (transform.position, _axis, speed * degreesThisFrame);
	}
}


/* You could do this, but it's not as clear. We can do the Vector 3 * matrix so why not!
	Matrix4x4 __angularMomentum = Matrix4x4.Scale (_angularMomentum);
	Matrix4x4 __angularVelocity = __angularMomentum * __intertiaTensorLocal.inverse;
	Vector3 _angularVelocity = ((Vector3)__angularVelocity.GetRow(2));
*/