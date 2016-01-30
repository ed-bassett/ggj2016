using UnityEngine;
using System.Collections;

public class ChairManager : MonoBehaviour 
{
	[Tooltip("Height of placed chairs above the ground")]
	public float chairOffset;
	public LayerMask floorLayerMask;
	public LayerMask chairLayerMask;
	[Tooltip("Drag the floormanager here")]
	public FloorManager floorManager;

	[Tooltip("Materials for 2 chairStates")]
	public Material chairNormal, chairPicked;

	[HideInInspector]
	public Transform currentChair;  // holds the chair being moved

	private bool pickedUpThisFrame;

	public static bool chairsCanBeMoved = true;
	// This should be set false when the level starts so the player can't move chairs during the first day.
	// Turns true only after the people have done their first day at the coffee shop.
	// Turns false again after they've opened the shop door to admit customers.

	void Start () 
	{
		currentChair = null;
		pickedUpThisFrame = false;
	}
	
	void Update () 
	{
		Ray ray;
		RaycastHit hit;
		int chairX = 0, chairY = 0; // saves position of dropped chairs to tell floorManager

		if (currentChair == null)
		{
			if (Input.GetMouseButtonDown(0) && chairsCanBeMoved) // player clicked - look for a chair
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 1000, chairLayerMask))
				{
					currentChair = hit.collider.transform;
					pickedUpThisFrame = true; // prevent dropping the chair later this frame from the same mouse click

					// change material on the chair
					hit.collider.GetComponent<Renderer>().material = chairPicked;

					hit.collider.GetComponent<Chair>().selected = true; // prevents it changing its material on mouse-over

					//tell the floorManager that this space is now empty
					floorManager.toggleChair(hit.collider.GetComponent<Chair>().posX, hit.collider.GetComponent<Chair>().posY, false);
				}
			}
		}

		if (currentChair != null) // snap the chair to the grid
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000, floorLayerMask))
			{
				// find closest empty floorTile to the ray hit location
				float closestDistance = Mathf.Infinity;
				Transform closestTile = null;

				for (int y = 0; y < floorManager.height; y++)
				{
					for (int x = 0; x < floorManager.width; x++)
					{
						// only look at empty tiles
						if (floorManager.isEmpty(x, y))
                        {
							Vector3 searchPosition = new Vector3(x * floorManager.spacing, 0, y * floorManager.spacing);
							//float currentDistance = Vector3.Distance(hit.point, floorManager.floorTiles[x, y].transform.position);
							float currentDistance = Vector3.Distance(hit.point, searchPosition);

							if (currentDistance < closestDistance)
							{
								closestDistance = currentDistance;
								closestTile = floorManager.floorTiles[x, y].transform;
								chairX = x;
								chairY = y;
							}
						}
					}
				}

				// position the chair
				currentChair.position = new Vector3(closestTile.position.x, closestTile.position.y + chairOffset, closestTile.position.z);

				// update its internal x and y storage
				currentChair.GetComponent<Chair>().posX = chairX;
				currentChair.GetComponent<Chair>().posY = chairY;
			}

			if (Input.GetMouseButtonDown(0)) // player clicked - drop the chair
			{
				if (!pickedUpThisFrame)
				{
					// change material on the chair
					currentChair.GetComponent<Renderer>().material = chairNormal;
					currentChair.GetComponent<Chair>().selected = false; // allow it to change material again
					
					// drop it
					currentChair = null;

					// tell the floor manager there's now a chair here
					floorManager.toggleChair(chairX, chairY, true);
				}
			}
		}

		pickedUpThisFrame = false; // allows dropping the chair on next click
	}
}
