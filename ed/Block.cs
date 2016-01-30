using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Block : MonoBehaviour {
  public List<BlockContent> contents = new List<BlockContent>();
	void Start () {
	}
	void Update () {
	}

  public bool isNavigable() {
    Debug.Log("Contents: " + contents.Any());
    bool isNav = (!contents.Any()) || contents.TrueForAll(c=>c.isNavigable());
    Debug.Log("isNav: " + isNav);
    return (!contents.Any()) || contents.TrueForAll(c=>c.isNavigable());
  }
}
