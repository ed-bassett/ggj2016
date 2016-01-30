using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Goal {
  [System.NonSerialized]
  protected List<Goal> prerequisites;
  [System.NonSerialized]
  protected List<Goal> inferedPrerequisites;

  public Transform prerequisiteContainer = null;
  private bool _hasBeenCompleted = false;
  public bool hasBeenCompleted {get{return _hasBeenCompleted;}}

  public Goal nextGoal(Person person) {
    if (hasBeenCompleted) {
      return null;
    }
    List<Goal> remaining = prerequisites.Concat(inferedPrerequisites).SkipWhile(p=>p.isComplete(person)).ToList();
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
      _hasBeenCompleted = (prerequisites.Concat(inferedPrerequisites).SkipWhile(p=>p.isComplete(person)).Any() == false) && selfIsComplete(person);
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
    updatePrerequisites(person);
  }
  public void updatePrerequisites(Person person) {
    inferedPrerequisites = populatePrerequisites(person);
  }
  public virtual List<Goal> populatePrerequisites(Person person) {return new List<Goal>();}
}
