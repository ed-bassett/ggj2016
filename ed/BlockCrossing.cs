using UnityEngine;
using System.Collections;

public class BlockCrossing : BlockContent {
  override public bool isNavigable() {return true;}
  override public bool isChairable() {return false;}
}
