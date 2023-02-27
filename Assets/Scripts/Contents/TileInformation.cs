using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInformation : TileBase
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public bool IsWall()
    {

        return true;
    }
}
