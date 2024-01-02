using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell : MonoBehaviour
{
    public bool isCollapsed;
    public Tilemap map;
    public List<Tile> tiles;
    public Tile currentSprite = null;
    public void UpdateCell(bool state, List<Tile> tileOptions){
        isCollapsed = state;
        tiles = tileOptions.ToList();

    }

    public void recreateTiles(List<Tile> tileOptions){
        tiles = tileOptions.ToList();
    }
}
