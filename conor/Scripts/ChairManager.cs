using UnityEngine;
using System.Collections;

public class ChairManager : MonoBehaviour 
{
	[Tooltip("Height of placed chairs above the ground")]
	public float chairOffset;
	public LayerMask floorLayerMask;
	public LayerMask chairLayerMask;
	[Tooltip("Drag the floormanager here")]
//	public FloorManager floorManager;
  public Grid grid;

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
//					floorManager.toggleChair(hit.collider.GetComponent<Chair>().posX, hit.collider.GetComponent<Chair>().posY, false);
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
        V2int closestPos = null;

				for (int z = 0; z < grid.totalHeight; z++) {
					for (int x = 0; x < grid.totalWidth; x++) {
						// only look at empty tiles
            V2int pos = new V2int(x,z);
            if ( grid.isValidChairPosition(pos) ) {
              V2int searchPosition = new V2int(x,z);
							float currentDistance = (grid.coordForPosition(hit.point) - searchPosition).sqrMagnitude;

							if (currentDistance < closestDistance) {
								closestDistance = currentDistance;
								closestPos = pos;
							}
						}
					}
				}

				// position the chair
				currentChair.position = grid.positionForCoord(closestPos) + grid.chairOffset;

				// update its internal x and y storage
        currentChair.GetComponent<Chair>().pos = closestPos;
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
//					floorManager.toggleChair(chairX, chairY, true);
				}
			}
		}

		pickedUpThisFrame = false; // allows dropping the chair on next click
	}
}
