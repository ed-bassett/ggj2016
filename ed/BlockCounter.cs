using UnityEngine;
using System.Collections;

public class BlockCounter : BlockContent {
  override public bool isNavigable() {return false;}
  override public bool isChairable() {return false;}
}
