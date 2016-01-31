using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Goal {
  [System.NonSerialized]
  protected List<Goal> prerequisites;
  [System.NonSerialized]
  protected List<UpdatableGoal> inferedPrerequisites;

  public Transform prerequisiteContainer = null;
  private bool _hasBeenCompleted = false;
  public bool hasBeenCompleted {get{return _hasBeenCompleted;}}

  public Goal nextGoal(Person person) {
    if (hasBeenCompleted) {
      return null;
    }
    List<Goal> remaining = prerequisites.Concat(inferedPrerequisites.ConvertAll(ug=>ug.goal)).SkipWhile(p=>p.isComplete(person)).ToList();
    if (remaining.Any()) {
      return remaining.First().nextGoal(person);
    }
    if (!isComplete(person)) {
      return this;
    }
    return null;
  }

  protected bool isComplete(Person person) {
    if ( !hasBeenCompleted  ) {
      _hasBeenCompleted = (prerequisites.Concat(inferedPrerequisites.ConvertAll(ug=>ug.goal)).SkipWhile(p=>p.isComplete(person)).Any() == false) && selfIsComplete(person);
    }
    return hasBeenCompleted;
  }
  protected virtual bool selfIsComplete(Person person) {return false;}
  public void init(Person person) {
    if ( prerequisiteContainer != null ) {
      prerequisites = prerequisiteContainer.GetComponentsInChildren<GoalMB>().ToList().ConvertAll(gmb=>gmb.goal);
      prerequisites.ForEach(p=>p.init(person));
    } else {
      prerequisites = new List<Goal>();
    }
    inferedPrerequisites = populatePrerequisites(person);
    updatePrerequisites(person);
  }
  public void updatePrerequisites(Person person) {
//    inferedPrerequisites = populatePrerequisites(person);
    inferedPrerequisites.ForEach(ug=>ug.update());
  }
  public virtual List<UpdatableGoal> populatePrerequisites(Person person) {return new List<UpdatableGoal>();}
}

public class UpdatableGoal {
  private Goal _goal;
  private Action _update = ()=>{};
  public UpdatableGoal(Goal goal, Action update) {
    this._goal = goal;
    this._update = update;
  }
  public Goal goal {get{return _goal;}}
  public Action update {get{return _update;}}
}