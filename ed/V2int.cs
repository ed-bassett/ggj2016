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

  public static V2int tl {get{return new V2int(-1,-1);}}
  public static V2int t  {get{return new V2int( 0,-1);}}
  public static V2int tr {get{return new V2int( 1,-1);}}
  public static V2int l  {get{return new V2int(-1, 0);}}
  public static V2int r  {get{return new V2int( 1, 0);}}
  public static V2int bl {get{return new V2int(-1, 1);}}
  public static V2int b  {get{return new V2int( 0, 1);}}
  public static V2int br {get{return new V2int( 1, 1);}}
  public static V2int operator +(V2int a, V2int b) {
    return new V2int(a.x+b.x,a.y+b.y);
  }
}