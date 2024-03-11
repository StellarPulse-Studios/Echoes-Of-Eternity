// --------------------------------------
// This script is totally optional. It is an example of how you can use the
// destructible versions of the objects as demonstrated in my tutorial.
// Watch the tutorial over at http://youtube.com/brackeys/.
// --------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public float force = 10;
	public GameObject destroyedVersion;	// Reference to the shattered version of the object

	// If the player clicks on the object
	void OnMouseDown ()
	{
		if (destroyedVersion == null) return;
		// Spawn a shattered object
		Transform obj =  Instantiate(destroyedVersion, transform.position, transform.rotation).transform;
		foreach(Transform t in obj)
		{
            Rigidbody body = t.GetComponent<Rigidbody>();
            if (body != null) body.AddForce(Vector3.one * force, ForceMode.Impulse);
        }
		
		// Remove the current object
		Destroy(gameObject);
	}

}
