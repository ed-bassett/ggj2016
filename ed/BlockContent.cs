using UnityEngine;
using System.Collections;

public abstract class BlockContent : MonoBehaviour {
  public abstract bool isNavigable();
  public abstract bool isChairable();
}
