using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GoalDrinkCoffee : Goal {
  public GoalDrinkCoffee(Person person) {
    init(person);
  }
  override public List<UpdatableGoal> populatePrerequisites(Person person) {
    return new List<UpdatableGoal>{
//      new UpdatableGoal(new GoalWaitForOpen(person),()=>{}),
//      new UpdatableGoal(new GoalLocation(person, person.manager.grid.doorLocation + V2int.b),()=>{}),
//      new UpdatableGoal(new GoalLocation(person, person.manager.grid.doorLocation),()=>{})
    };
  }
  override protected bool selfIsComplete(Person person) {
    return person.hasDrunkCoffee;
  }
}
