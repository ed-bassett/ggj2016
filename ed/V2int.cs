using UnityEngine;
using System.Collections;

[System.Serializable]
public class V2int {
  [SerializeField]
  private int _x;
  [SerializeField]
  private int _y;
  public V2int(int x, int y) {
    this._x = x;
    this._y = y;
  }
  public int x {get{return _x;}}
  public int y {get{return _y;}}
  public float sqrMagnitude {get{return _x * _x + _y * _y;}}

  public static V2int tl {get{return new V2int(-1,-1);}}
  public static V2int t  {get{return new V2int( 0,-1);}}
  public static V2int tr {get{return new V2int( 1,-1);}}
  public static V2int r  {get{return new V2int( 1, 0);}}
  public static V2int br {get{return new V2int( 1, 1);}}
  public static V2int b  {get{return new V2int( 0, 1);}}
  public static V2int bl {get{return new V2int(-1, 1);}}
  public static V2int l  {get{return new V2int(-1, 0);}}
  public static V2int operator +(V2int a, V2int b) {
    return new V2int(a.x+b.x,a.y+b.y);
  }
  public static V2int operator -(V2int a, V2int b) {
    return new V2int(a.x-b.x,a.y-b.y);
  }

  // overrides and overloads for value equality
  public override bool Equals(System.Object obj) {
    if (obj == null) {
      return false;
    }
    V2int p = obj as V2int;
    if ((System.Object)p == null) {
      return false;
    }
    return (x == p.x) && (y == p.y);
  }
  public bool Equals(V2int p)
  {
    if ((object)p == null) {
        return false;
    }
    return (_x == p.x) && (_y == p.y);
  }
  public override int GetHashCode() {
    return x * 100000 * y;
  }
  public static bool operator ==(V2int a, V2int b) {
    return a.Equals(b);
  }
  public static bool operator !=(V2int a, V2int b) {
    return !(a == b);
  }
}