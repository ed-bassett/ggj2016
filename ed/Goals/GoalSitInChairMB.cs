using UnityEngine;
using System.Collections;

public class GoalSitInChairMB : GoalMB {
  [SerializeField]
  public GoalSitInChair _goalSitInChair;
  override public Goal goal {get{return _goalSitInChair;}}
}
