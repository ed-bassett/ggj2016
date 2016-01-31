using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour 
{
	enum HazardType { permanent, temporary, moving };
	
	[SerializeField]
	private HazardType type;

	[Tooltip("When the hazard becomes active")]
	public float startDelay;

	[Tooltip("Drag a destination transform here for moving hazards")]
	public Transform destination;
	
	[Tooltip("Killed players are pushed towards this")]
	public Transform deathDirection;

	[Tooltip("Category of death animation")]
	public int deathType;

	[Tooltip("Speed of moving hazards")]
	public float speed;

	private bool	moving = false;	// used by moving hazards
	private Vector3 startPosition;  // used by moving hazards
	private float	startTime;      // used by moving hazards


	public void init ()
	{
		// turn off the red trigger visual
		GetComponent<Renderer>().enabled = false;

		if (type == HazardType.temporary)
		{
			GetComponent<Collider>().enabled = false;
			StartCoroutine("enableHazard", startDelay);
		}
		else if (type == HazardType.moving)
		{
			GetComponent<Collider>().enabled = false;
			StartCoroutine("enableHazard", startDelay);
			StartCoroutine("moveHazard", startDelay);
			startPosition = transform.position;
			// unlink the destination so it doesn't move with the hazard
			destination.SetParent(null);
		}
	}

	IEnumerator enableHazard (float delay)
	{
		yield return new WaitForSeconds(delay);
		GetComponent<Collider>().enabled = true;

		// play the hazard's animation
		if (type == HazardType.temporary)
		{
			// the door is the only temporary hazard at the moment
			Transform doorPivot = transform.FindChild("DoorPivot");
			doorPivot.Rotate(0, 170, 0);
		}
	}

	IEnumerator moveHazard(float delay)
	{
		yield return new WaitForSeconds(delay);
		moving = true;
		startTime = Time.time;
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag("Person"))
		{
			Vector3 deathVector = deathDirection.position - transform.position;
			other.GetComponent<Person>().die(deathVector, deathType);
		}
	}

	void Update ()
	{
		if (moving)
		{
			transform.position = Vector3.Lerp(startPosition, destination.position, (Time.time - startTime) * speed);
		}
	}
}
