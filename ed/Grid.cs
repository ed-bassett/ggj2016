using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {
  public int width = 10;
  public int height = 10;
  public float size = 1f;
  public int randomSeed = 0;

  public Block blockPrefab;
  public BlockTable tablePrefab;

  public bool built = false;

  [SerializeField]
  private Block[,] gridContents;

	void Start () {
	  built=false;
    init();
	}
	void Update () {
    init();
	}
  void init() {
    if (built==false) {
      built = true;

      Transform container = transform.FindChild("blockContainer");
      if (container != null) {
        GameObject.DestroyImmediate(container.gameObject);
      }
      GameObject containerGo = new GameObject();
      containerGo.name = "blockContainer";
      container = containerGo.transform;
      container.parent = transform;
      container.transform.localPosition = Vector3.zero;
      container.transform.localRotation = Quaternion.identity;

      gridContents = new Block[width,height];
      Random.seed = randomSeed;
      for (int x=0;x<width;x++) {
        for (int z=0;z<height;z++) {
          Block block = GameObject.Instantiate<Block>(blockPrefab);
          block.transform.parent = container;
          block.transform.localPosition = new Vector3(x*size,0,z*size);
          block.transform.localRotation = Quaternion.identity;

          bool isTable = Random.Range(0,4)==1;

          if (isTable) {
            BlockTable table = GameObject.Instantiate<BlockTable>(tablePrefab);
            table.transform.parent = block.transform;
            table.transform.localPosition = Vector3.zero;
            table.transform.localRotation = Quaternion.identity;
            block.contents.Add(table);
          }

          gridContents[x,z] = block; 
        }
      }
    }
  }

  public Vector3 positionForCoord(V2int pos) {
    return new Vector3(pos.x*size,0,pos.y*size);
  }
  public V2int coordForPosition(Vector3 pos) {
    Vector3 localPos = transform.InverseTransformPoint(pos);
    return new V2int(Mathf.RoundToInt(localPos.x/size),Mathf.RoundToInt(localPos.z/size));
  }

  public bool[,] navigable() {
    bool[,] result = new bool[width,height];
    for (int x=0;x<width;x++) {
      for (int z=0;z<height;z++) {
        result[x,z] = gridContents[x,z].isNavigable();
        Debug.Log("X: " + x + ", Z: " +z + ", " +result[x,z]);
      }
    }
    return result;
  }
}
