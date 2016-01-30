using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GoalSitInChair : Goal {
  [SerializeField]
  private Chair _chair;

  public GoalSitInChair(Person person, Chair chair) {
    this._chair = chair;
    init(person);
  }
  override public List<Goal> populatePrerequisites(Person person) {
    return new List<Goal>{new GoalLocation(person, _chair.grid.coordForPosition(_chair.transform.position))};
  }
  override protected bool selfIsComplete(Person person) {
    return person.isSittingOn(chair);
	}
  public Chair chair {get{return _chair;}}
}
