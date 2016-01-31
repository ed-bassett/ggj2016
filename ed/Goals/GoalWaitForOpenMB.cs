using UnityEngine;
using System.Collections;

public class GoalWaitForOpenMB : GoalMB {
  [SerializeField]
  public GoalWaitForOpen _goalWaitForOpen;
  override public Goal goal {get{return _goalWaitForOpen;}}
}
