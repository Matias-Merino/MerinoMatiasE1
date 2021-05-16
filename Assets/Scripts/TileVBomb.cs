using E1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVBomb : Tile
{
    public override void OnMouseDown()
    {
        boardManager.ExplodeVTiles(tileData);
    }
}
