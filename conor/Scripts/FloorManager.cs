using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {

	public GameObject	floorTile;		// prefab
	public int			width, height;	// size of room
	public float		spacing;        // space between floor tiles
	public Material		floorTileEmpty;
	public Material		floorTileOccuppied;

	public GameObject[,] floorTiles;
	public bool[,] tileStates;

	// Use this for initialization
	void Awake ()
	{
		floorTiles = new GameObject[width, height];
		tileStates = new bool[width, height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				GameObject newTile = Instantiate(floorTile, new Vector3(x * spacing, 0, y * spacing), Quaternion.identity) as GameObject;
				floorTiles[x, y] = newTile;

				// randomly assign a table or empty tile
				tileStates[x, y] = Random.Range(0, 2) == 0 ? false : true; // true = occuppied
				if (tileStates[x, y])
					floorTiles[x, y].GetComponent<Renderer>().material = floorTileOccuppied;
				else
					floorTiles[x, y].GetComponent<Renderer>().material = floorTileEmpty;
			}
		}
	}
	
	public bool isEmpty (int x, int y)
	{
		return (!tileStates[x, y]);
	}


	/// <summary>
	/// Place or pick up a chair
	/// </summary>
	/// <param name="x">Location x</param>
	/// <param name="y">Location y</param>
	/// <param name="placed">True for placing, false for removing</param>
	public void toggleChair (int x, int y, bool placed)
	{
		tileStates[x, y] = placed;
		if (placed)
			floorTiles[x, y].GetComponent<Renderer>().material = floorTileOccuppied;
		else
			floorTiles[x, y].GetComponent<Renderer>().material = floorTileEmpty;
	}
}
