using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GoalSitInNearestChair : Goal {
  [SerializeField]
  private Manager _manager;
  private Chair _chair;

  public GoalSitInNearestChair(Person person, Manager manager) {
    this._manager = manager;
    init(person);
  }
  override public List<Goal> populatePrerequisites(Person person) {
    List<V2int> chairCoords = _manager.ChairAtCoord().Keys.ToList();
    List<V2int> otherPeopleCoords = _manager.PersonAtCoord().Where(v=>v.Value != person).ToList().ConvertAll(v=>v.Key);
    chairCoords.RemoveAll(c=>otherPeopleCoords.Contains(c));
    if (chairCoords.Any()) {
      V2int nearestCoord = chairCoords.First();
      float nearestDistance = Mathf.Infinity;
      chairCoords.ForEach(c=>{
        float distance = (_manager.grid.coordForPosition(person.transform.position) - c).sqrMagnitude;
        if ( distance < nearestDistance ) {
          nearestCoord = c;
          nearestDistance = distance;
        }
      });
      _chair = _manager.ChairAtCoord()[nearestCoord];
      return new List<Goal>{new GoalLocation(person,_chair.grid.coordForPosition(_chair.transform.position))};
    }
    return new List<Goal>();
  }
  override protected bool selfIsComplete(Person person) {
    return person.isSittingOn(chair);
  }
  public Chair chair {get{return _chair;}}
}
