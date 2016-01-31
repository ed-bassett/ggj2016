using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {
  public int backWallWidth = 5;
  public int width = 10;
  public int height = 10;
  public int wallWidth = 1;
  public int pathWidth = 2;
  public int roadWidth = 4;
  public int crossingMin = 12;
  public int crossingMax = 13;
  public int counterX = 16;
  public int counterZmin = 12;
  public int counterZmax = 16;
  public float size = 1f;
  public int randomSeed = 0;
  public V2int doorLocation = new V2int(7,5);
  public Vector3 chairOffset = Vector3.up * .5f;

  public Block blockPrefab;
  public BlockTable tablePrefab;
  public BlockWall wallPrefab;
  public BlockPath pathPrefab;
  public BlockCrossing crossingPrefab;
  public BlockRoad roadPrefab;
  public BlockDoor doorPrefab;
  public BlockWall backWallPrefab;
  public BlockCounter counterPrefab;
  public BlockBehindCounter behindCounterPrefab;

  public Transform props;

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

      gridContents = new Block[totalWidth,totalHeight];
      Random.seed = randomSeed;
      for (int x=0;x<totalWidth;x++) {
        for (int z=0;z<totalHeight;z++) {
          Block block = GameObject.Instantiate<Block>(blockPrefab);
          block.transform.parent = container;
          block.transform.localPosition = new Vector3(x*size,0,z*size);
          block.transform.localRotation = Quaternion.identity;

          if ( x < pathWidth || z < pathWidth ) { // path
            createBlockContent<BlockPath>(block, pathPrefab.gameObject);
          } else if ( x < pathWidth + roadWidth || z < pathWidth + roadWidth ) { // road
            if ( x >= crossingMin && x<= crossingMax ) {
              createBlockContent<BlockCrossing>(block, crossingPrefab.gameObject);
            } else {
              createBlockContent<BlockRoad>(block, roadPrefab.gameObject);
            }
          } else if ( x < pathWidth + roadWidth + pathWidth || z < pathWidth + roadWidth + pathWidth ) { // path
            createBlockContent<BlockPath>(block, pathPrefab.gameObject);
          } else if ( (x < pathWidth + roadWidth + pathWidth + wallWidth || z < pathWidth + roadWidth + pathWidth + wallWidth) ) { // wall
            if (new V2int(x,z) == doorLocation) {
              createBlockContent<BlockDoor>(block, doorPrefab.gameObject);
            } else {
              createBlockContent<BlockWall>(block, wallPrefab.gameObject);
            }
          } else if ( x < pathWidth + roadWidth + pathWidth + wallWidth + width && z < pathWidth + roadWidth + pathWidth + wallWidth + height) { // inside
            if ( x >= counterX ) {
              if ( x == counterX && z >= counterZmin && z < counterZmax) {
                createBlockContent<BlockCounter>(block, counterPrefab.gameObject);
              } else {
                createBlockContent<BlockBehindCounter>(block, behindCounterPrefab.gameObject);
              }
            } else {
              bool isTable = Random.Range(0,7)==1;
              if (isTable) {
                createBlockContent<BlockTable>(block, tablePrefab.gameObject);
              }
            }
          } else {
            createBlockContent<BlockWall>(block, backWallPrefab.gameObject);
          }

          gridContents[x,z] = block; 
        }
      }
      props.position = positionForCoord(new V2int(pathWidth + roadWidth + pathWidth + wallWidth, pathWidth + roadWidth + pathWidth + wallWidth));
    }
  }

  public int totalWidth {get {return backWallWidth + width + wallWidth + pathWidth + roadWidth + pathWidth;}}
  public int totalHeight {get {return backWallWidth + height + wallWidth + pathWidth + roadWidth + pathWidth;}}

  public void addChair(V2int pos, Chair chair) {
    gridContents[pos.x,pos.y].contents.Add(chair);
  }

  public Chair removeChair(V2int pos) {
    Chair chair = (Chair)gridContents[pos.x,pos.y].contents.Find(c=>c is Chair);
    if ( chair != null ) {
      gridContents[pos.x,pos.y].contents.Remove(chair);
    }
    return chair;
  }

  private void createBlockContent<T> (Block block, GameObject prefab) where T:BlockContent {
    T t = GameObject.Instantiate<T>(prefab.GetComponent<T>());
    t.transform.parent = block.transform;
    t.transform.localPosition = Vector3.zero;
    t.transform.localRotation = Quaternion.identity;
    block.contents.Add(t);
  }

  public Vector3 positionForCoord(V2int pos) {
    return new Vector3(pos.x*size,0,pos.y*size);
  }
  public V2int coordForPosition(Vector3 pos) {
    Vector3 localPos = transform.InverseTransformPoint(pos);
    return new V2int(Mathf.RoundToInt(localPos.x/size),Mathf.RoundToInt(localPos.z/size));
  }

  public bool[,] navigable() {
    bool[,] result = new bool[totalWidth,totalHeight];
    for (int x=0;x<totalWidth;x++) {
      for (int z=0;z<totalHeight;z++) {
        result[x,z] = gridContents[x,z].isNavigable();
      }
    }
    return result;
  }

  public bool isValidChairPosition(V2int pos) {
    return gridContents[pos.x,pos.y].isChairable();
  }
}
