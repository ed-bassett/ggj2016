﻿using UnityEngine;
using System.Collections;

public class BlockTable : BlockContent {
  override public bool isNavigable() {return false;}
  override public bool isChairable() {return false;}
}
