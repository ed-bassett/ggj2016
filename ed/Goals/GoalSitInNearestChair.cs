using UnityEngine;
using System;
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

  private Chair closestChair(Person person) {
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
      return _manager.ChairAtCoord()[nearestCoord];
    }
    return null;
  }

  override public List<UpdatableGoal> populatePrerequisites(Person person) {
    _chair = closestChair(person);
    GoalLocation goal = new GoalLocation(person,_chair.grid.coordForPosition(_chair.transform.position));
    Action updateSelf = ()=>{
      Chair closest = closestChair(person);
      _chair = closest;
      goal.pos = _chair.grid.coordForPosition(_chair.transform.position);
    };
    return new List<UpdatableGoal>{
      new UpdatableGoal(goal,updateSelf)
    };
  }
  override protected bool selfIsComplete(Person person) {
    return person.isSittingOn(chair);
  }
  public Chair chair {get{return _chair;}}
}
