using UnityEngine;
using System.Collections;

public class GoalDrinkCoffeeMB : GoalMB {
  [SerializeField]
  public GoalDrinkCoffee _goalDrinkCoffee;
  override public Goal goal {get{return _goalDrinkCoffee;}}
}
