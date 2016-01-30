using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public void open ()
	{
		transform.Rotate(0, 90, 0);
	}	

	public void close ()
	{
		transform.Rotate(0, -90, 0);
	}

	/*
	void Update ()
	{
		//test
		if (Input.GetMouseButtonDown(0))
		{
			open();
		}
		if (Input.GetMouseButtonDown(1))
		{
			close();
		}
	}
	*/
}
