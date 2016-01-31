using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GoalLocation : Goal {
  [SerializeField]
  public V2int _pos;
  public V2int pos {
    get{return _pos;}
    set{_pos=value;}
  }
  public GoalLocation(Person person, V2int pos) {
    this._pos = pos;
    init(person);
  }
  override protected bool selfIsComplete(Person person) {
    return person.isAtLocation(pos);
  }
}
