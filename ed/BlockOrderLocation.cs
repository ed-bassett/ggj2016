using UnityEngine;
using System.Collections;

public class BlockOrderLocation : BlockContent {
  override public bool isNavigable() {return true;}
  override public bool isChairable() {return false;}
}
