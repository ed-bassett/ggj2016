using UnityEngine;
using System.Collections;

public class Chair : BlockContent {
  public Grid grid;

  public Material normalMaterial, mouseOverMaterial;
  
  //[HideInInspector]
  public V2int pos; // stores where in the array it's placed - helps for removing it when picked up

  [HideInInspector]
  public bool selected; // true when the player picks it up

  private Renderer myRenderer;
  private ChairManager chairManager;

  
  void Start () {
    chairManager = GameObject.Find("ChairManager").GetComponent<ChairManager>();
    selected = false;
    myRenderer = GetComponent<Renderer>();

    // find out where I am in the grid and save my pos
    // this means chairs can be placed in the editor and they'll figure out their exact co-ordinates on their own
    float closestDistance = Mathf.Infinity;
    
    for (int z = 0; z < grid.totalHeight; z++) {
      for (int x = 0; x < grid.totalWidth; x++) {
        // only look at empty tiles
        if (grid.isValidChairPosition(new V2int(x, z)) ) {
          V2int searchPosition = new V2int(x,z);
          float currentDistance = (grid.coordForPosition(transform.position) - searchPosition).sqrMagnitude;

          if (currentDistance < closestDistance) {
            closestDistance = currentDistance;
            pos = new V2int(x,z);
          }
        }
      }
    }

    // tell the floor manager there's a chair here
    grid.addChair(pos, this);

    // snap the mesh to the correct location
    transform.position = grid.positionForCoord(pos) + grid.chairOffset;
  }

  override public bool isNavigable() {return true;}
  override public bool isChairable() {return false;}
  
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
