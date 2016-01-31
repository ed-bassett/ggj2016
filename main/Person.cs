using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person : MonoBehaviour {
  public Manager manager;
  public Grid grid;
  public Transform goalsContainer;
  public Transform goalIndicator = null;
  protected List<Goal> goals;
  float placedOrderAt = Mathf.Infinity;
  public bool hasOrdered = false;
  float startedDrinkingCoffeeAt = Mathf.Infinity;
  public bool hasDrunkCoffee = false;

	public Material[] heads, bodies, legs;
	public Renderer myHead, myBody, myLegs;

	// Use this for initialization
	void Start () {
    goals = goalsContainer.GetComponentsInChildren<GoalMB>().ToList().ConvertAll(gmb=>gmb.goal).ToList();
    goals.ForEach(g=>g.init(this));

		Debug.Log("Starting person script");

		// randomize appearance
		int randomHead = Random.Range(0, heads.Length);
		myHead.material = heads[randomHead];

		int randomBody = Random.Range(0, bodies.Length);
		myBody.material = bodies[randomBody];

		int randomLegs = Random.Range(0, legs.Length);
		myLegs.material = legs[randomLegs];

	}

	
	
  private float pathLastUpdated = 0;
  private List<V2int> path = null;

  private Chair _currentChair = null;

	// Update is called once per frame
	void Update () {
    goals.ForEach(g=>g.updatePrerequisites(this));
    List<Goal> pendingGoals = goals.SkipWhile(g=>g.nextGoal(this) == null).ToList();
    if (pendingGoals.Any()) {
      Goal nextGoal = pendingGoals.First().nextGoal(this);

      Debug.Log(nextGoal);
      switch(nextGoal.GetType().ToString()) {
        case "GoalLocation":
          GoalLocation goalLocation = nextGoal as GoalLocation;

          if (goalIndicator != null) {
            goalIndicator.position = grid.positionForCoord(goalLocation.pos);
          }

          if (Time.time > pathLastUpdated + .5f) {
            PathFinder pathFinder = new PathFinder(manager,grid.navigable(),grid.coordForPosition(transform.position),goalLocation.pos);
            path = pathFinder.shortest();
            pathLastUpdated = Time.time;
          }
          if (path != null) {
            Vector3 direction = (grid.positionForCoord(path.First()) - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(direction,Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation,newRotation,Time.deltaTime * 4f);

            transform.position = transform.position + direction * 3 * Time.deltaTime;
          }
        break;
        case "GoalSitInChair":
          GoalSitInChair goalSitInChair = nextGoal as GoalSitInChair;
          _currentChair = goalSitInChair.chair;
        break;
        case "GoalOrderCoffee":
          if ( Time.time < placedOrderAt ) {
            placedOrderAt = Time.time;
          }
          if ( Time.time > placedOrderAt + 3f) {
            hasOrdered = true;
          }
        break;
        case "GoalDrinkCoffee":
          if ( Time.time < startedDrinkingCoffeeAt ) {
            startedDrinkingCoffeeAt = Time.time;
          }
          if ( Time.time > startedDrinkingCoffeeAt + 5f) {
            hasDrunkCoffee = true;
          }
        break;
        case "GoalSitInNearestChair":
          GoalSitInNearestChair goalSitInNearestChair = nextGoal as GoalSitInNearestChair;
          _currentChair = goalSitInNearestChair.chair;
        break;
      }
    }
	}

  public void die (Vector3 directionOfDeath, int typeOfDeath) {
    // entered a hazard
    Debug.Log("ZOMG I'm dead");
  }

  public bool isAtLocation(V2int pos) {
    Vector3 delta = transform.position - grid.positionForCoord(pos);
    return (new Vector2(delta.x,delta.z)).sqrMagnitude < .001f;
  }
  public bool isSittingOn(Chair chair) {
    return (_currentChair == chair);
  }
}

public class PathFinder {
  private Manager _manager;
  private bool[,] _grid;
  private V2int _start;
  private V2int _target;
  private HashSet<V2int> _tested = new HashSet<V2int>();

  public PathFinder(Manager manager, bool[,] grid, V2int start, V2int target) {
    this._manager = manager;
    this._grid = grid;
    this._start = start;
    this._target = target;
  }

  public List<V2int> shortest() {
    List<List<V2int>> paths = untestedSurrounding(_start).ConvertAll(s=>new List<V2int>{s});
    List<V2int> pathToTarget = paths.Find(p=>p.Last()==_target);
    if (pathToTarget != null) {
      return pathToTarget;
    }
    while (paths.Count > 0) {
      List<List<V2int>> newPaths = new List<List<V2int>>();
      for (int i=0;i<paths.Count;i++) {
        List<V2int> path = paths[i];
        List<List<V2int>> newPathsforPath = untestedSurrounding(path.Last()).ConvertAll(point=>{
          List<V2int> newPath = new List<V2int>(path);
          newPath.Add(point);
          _tested.Add(point);
          return newPath;
        });
        
        pathToTarget = newPathsforPath.Find(p=>p.Last()==_target);
        if (pathToTarget != null) {
          return pathToTarget;
        }

        newPaths.AddRange(newPathsforPath);
      };
      paths = newPaths;
    }
    return null;
  }

  private bool isValid (V2int point) {
    bool valid = point.x >= 0 && point.x < _grid.GetLength(0) && point.y >= 0 && point.y < _grid.GetLength(1) && _grid[point.x,point.y] && !_manager.PersonAtCoord().Keys.ToList().Contains(point);
    return valid;
  }

  private List<V2int> untestedSurrounding(V2int p) {
    List<V2int> surrounding = new List<V2int> {p+V2int.t,p+V2int.r,p+V2int.b,p+V2int.l};
    return surrounding.FindAll(s=>isValid(s) && !_tested.Contains(s)).ToList();
  }
}

