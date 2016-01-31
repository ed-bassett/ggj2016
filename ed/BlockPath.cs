using UnityEngine;
using System.Collections;

public class BlockPath : BlockContent {
  override public bool isNavigable() {return true;}
  override public bool isChairable() {return false;}
}
