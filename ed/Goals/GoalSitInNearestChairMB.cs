using UnityEngine;
using System.Collections;

public class GoalSitInNearestChairMB : GoalMB {
  [SerializeField]
  public GoalSitInNearestChair _goalSitInNearestChair;
  override public Goal goal {get{return _goalSitInNearestChair;}}
}
