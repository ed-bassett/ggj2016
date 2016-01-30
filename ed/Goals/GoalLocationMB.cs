using UnityEngine;
using System.Collections;

public class GoalLocationMB : GoalMB {
  [SerializeField]
  public GoalLocation _goalLocation;
  override public Goal goal {get{return _goalLocation;}}
}
