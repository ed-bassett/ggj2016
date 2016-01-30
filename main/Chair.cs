using UnityEngine;
using System.Collections;

public class Chair : MonoBehaviour {
  public Grid grid;

  public Material normalMaterial, mouseOverMaterial;
  
  //[HideInInspector]
  public int posX, posY; // stores where in the array it's placed - helps for removing it when picked up

  [HideInInspector]
  public bool selected; // true when the player picks it up

  private Renderer myRenderer;
  private FloorManager floorManager;
  private ChairManager chairManager;

  
  void Start () 
  {
    floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
    chairManager = GameObject.Find("ChairManager").GetComponent<ChairManager>();
    selected = false;
    myRenderer = GetComponent<Renderer>();

    // find out where I am in the grid and save my posX and posY
    // this means chairs can be placed in the editor and they'll figure out their exact co-ordinates on their own
    float closestDistance = Mathf.Infinity;
    
    for (int y = 0; y < floorManager.height; y++)
    {
      for (int x = 0; x < floorManager.width; x++)
      {
        // only look at empty tiles
        if (floorManager.isEmpty(x, y))
        {
          Vector3 searchPosition = new Vector3(x * floorManager.spacing, 0, y * floorManager.spacing);
          float currentDistance = Vector3.Distance(transform.position, searchPosition);

          if (currentDistance < closestDistance)
          {
            closestDistance = currentDistance;
            posX = x;
            posY = y;
          }
        }
      }
    }

    // tell the floor manager there's a chair here
    floorManager.toggleChair(posX, posY, true);

    // snap the mesh to the correct location
    transform.position = new Vector3(posX * floorManager.spacing, chairManager.chairOffset, posY * floorManager.spacing);
  }
  
  void OnMouseEnter ()
  {
    if (!selected && chairManager.currentChair == null && ChairManager.chairsCanBeMoved)
      myRenderer.material = mouseOverMaterial;
  }

  void OnMouseExit ()
  {
    if (!selected)
      myRenderer.material = normalMaterial;
  }
}
