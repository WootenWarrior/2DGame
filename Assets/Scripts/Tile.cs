using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool done;
    public List<Tile> upTiles;
    public List<Tile> downTiles;
    public List<Tile> leftTiles;
    public List<Tile> rightTiles;

    public bool IsCompatible(Tile nextTile, Vector3Int direction){
        bool contains = false;

        if(direction == new Vector3Int(1,0,0)){
            contains = rightTiles.Contains(nextTile);
        }else if(direction == new Vector3Int(0,1,0)){
            contains = upTiles.Contains(nextTile);
        }else if(direction == new Vector3Int(-1,0,0)){
            contains = leftTiles.Contains(nextTile);
        }else if(direction == new Vector3Int(0,-1,0)){
            contains = downTiles.Contains(nextTile);
        }

        return contains;
    }
}
