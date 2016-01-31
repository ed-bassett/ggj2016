using UnityEngine;
using System.Collections;

public class GoalOrderCoffeeMB : GoalMB {
  [SerializeField]
  public GoalOrderCoffee _goalOrderCoffee;
  override public Goal goal {get{return _goalOrderCoffee;}}
}
