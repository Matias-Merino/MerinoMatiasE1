using E1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHBomb : Tile
{
    public override void OnMouseDown()
    {
        boardManager.ExplodeHTiles(tileData);
    }
}
