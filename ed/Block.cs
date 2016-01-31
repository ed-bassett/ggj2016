using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Block : MonoBehaviour {
  public List<BlockContent> contents = new List<BlockContent>();
  public bool isNavigable() {
    return (!contents.Any()) || contents.TrueForAll(c=>c.isNavigable());
  }
  public bool isChairable() {
    return (!contents.Any()) || contents.TrueForAll(c=>c.isChairable());
  }
}
