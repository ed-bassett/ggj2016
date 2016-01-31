using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GoalWaitForOpen : Goal {
  public GoalWaitForOpen(Person person) {
    init(person);
  }
  override protected bool selfIsComplete(Person person) {
    return person.manager.isOpen;
  }
}
