﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Manager : MonoBehaviour {
  public List<Person> people;
  public List<Chair> chairs;
  public Grid grid;
  public Dictionary<V2int,Person> PersonAtCoord() {
    return people.ConvertAll(p=>new{coord=grid.coordForPosition(p.transform.position), person=p}).ToDictionary(v=>v.coord,v=>v.person);
  }
  public Dictionary<V2int,Chair> ChairAtCoord() {
    return chairs.ConvertAll(c=>new{coord=grid.coordForPosition(c.transform.position), chair=c}).ToDictionary(v=>v.coord,v=>v.chair);
  }
}