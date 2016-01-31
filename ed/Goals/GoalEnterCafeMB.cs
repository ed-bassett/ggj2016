using UnityEngine;
using System.Collections;

public class GoalEnterCafeMB : GoalMB {
  [SerializeField]
  public GoalEnterCafe _goalEnterCafe;
  override public Goal goal {get{return _goalEnterCafe;}}
}
