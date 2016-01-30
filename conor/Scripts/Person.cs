using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour 
{
	public float speed;

	public void die (Vector3 directionOfDeath, int typeOfDeath)
	{
		// entered a hazard
		Debug.Log("ZOMG I'm dead");
	}

	// walk forward for testing
	void Update ()
	{
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.Self); 
	}
}
