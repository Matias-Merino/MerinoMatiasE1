using E1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBomb : Tile
{
    public override void OnMouseDown()
    {
        boardManager.ExplodeTiles(tileData);
    }
}
