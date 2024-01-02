using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Unity.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileGenerator : MonoBehaviour
{
    public List<Tile> tiles;
    public int tileMapSizeX;
    public int tileMapSizeY;
    public int startXpos;
    public int startYpos;
    public Transform playerTransform;
    public bool placeCenter = false;
    public Cell[,] Grid;
    public Cell cell;
    public GameObject parent;
    private int lowestEntropyVal;
    private int iterations;
    
    void Start()
    {
        iterations = 0;
        GenerateTiles();
        
        //parent = this.gameObject;

        if(placeCenter == true){
            PlacePlayerInCenter();
        }
    }

    void GenerateTiles()
    {
        Grid = new Cell[tileMapSizeX, tileMapSizeY];

        for (int x = startXpos; x < tileMapSizeX; x++)
        {
            for (int y = startYpos; y < tileMapSizeY; y++)
            {
                Vector3Int vector3 = new Vector3Int(x,y,0);
                cell = Instantiate(cell);
                cell.name = "Cell" + x + "," + y;
                cell.transform.parent = parent.transform;
                cell.transform.position = vector3;
                cell.UpdateCell(false,tiles);
                Grid[x,y] = cell;
            }
        }

        Vector3Int startCollapsePos = new Vector3Int(Random.Range(0,tileMapSizeX),Random.Range(0,tileMapSizeY),0);
        Cell startCell = Grid[startCollapsePos.x,startCollapsePos.y];
        Collapse(tiles,startCell);
        startCell.isCollapsed = true;
        UpdateNeighbours(startCell);
        List<Cell> lowestEntropyList = CreateLowEntropyList();

        bool finished = false;
        while(finished == false){
            if(lowestEntropyList.Count > 0){
                Cell currentCell = lowestEntropyList[0];
                Collapse(currentCell.tiles,currentCell);
                UpdateNeighbours(currentCell);
                finished = IsFinished();
            }else{
                finished = true;
            }
            lowestEntropyList = CreateLowEntropyList();
        }
    }

    public void UpdateNeighbours(Cell cell){
        Cell upNeighbour = null;
        Cell downNeighbour = null;
        Cell leftNeighbour = null;
        Cell rightNeighbour = null;
        if(cell.transform.position.y < tileMapSizeY-1){
            //Debug.Log("tried "+cell.transform.position.y+" +1" );
            upNeighbour = Grid[Mathf.RoundToInt(cell.transform.position.x),Mathf.RoundToInt(cell.transform.position.y+1)];
        }
        if(cell.transform.position.y > startYpos){
            //Debug.Log("tried "+cell.transform.position.y+" -1" );
            downNeighbour = Grid[Mathf.RoundToInt(cell.transform.position.x),Mathf.RoundToInt(cell.transform.position.y-1)];
        }
        if(cell.transform.position.x > startXpos){
            //Debug.Log("tried "+cell.transform.position.x+" -1" );
            leftNeighbour = Grid[Mathf.RoundToInt(cell.transform.position.x-1),Mathf.RoundToInt(cell.transform.position.y)];
        }
        if(cell.transform.position.x < tileMapSizeX-1){
            //Debug.Log("tried "+cell.transform.position.x+" +1" );
            rightNeighbour = Grid[Mathf.RoundToInt(cell.transform.position.x+1),Mathf.RoundToInt(cell.transform.position.y)];
        }
        Tile collapsedCellTile = cell.currentSprite;

        List<Cell> neighbours = new()
        {
            upNeighbour,
            downNeighbour,
            leftNeighbour,
            rightNeighbour
        };
        foreach (Cell neighbour in neighbours)
        {
            if(neighbour != null){
                Vector3 vector = neighbour.transform.position-cell.transform.position;
                Vector3Int neighbourVector = new Vector3Int(Mathf.FloorToInt(vector.x),Mathf.FloorToInt(vector.y),Mathf.FloorToInt(vector.z));
                if(neighbour.isCollapsed == false){
                    List<Tile> deleteTiles = new();
                    foreach (Tile neighbourCellTile in neighbour.tiles)
                    {
                        if(collapsedCellTile.IsCompatible(neighbourCellTile,neighbourVector)==false){
                            deleteTiles.Add(neighbourCellTile);
                        }
                    }
                    foreach (Tile item in deleteTiles)
                    {
                        neighbour.tiles.Remove(item);
                    }
                }
                else{
                    if(collapsedCellTile.IsCompatible(neighbour.currentSprite,neighbourVector)==false){
                        SpriteRenderer cellSpriteRenderer = cell.GetComponent<SpriteRenderer>();
                        //SpriteRenderer neighbourSpriteRenderer = neighbour.GetComponent<SpriteRenderer>();
                        cellSpriteRenderer.sprite = null;
                        cell.tiles.Remove(neighbour.currentSprite);
                        Collapse(cell.tiles,cell);
                    }
                    
                }
            }
        }
    }

    public List<Cell> CreateLowEntropyList(){
        List<Cell> lowestEntropy = new List<Cell>();
        lowestEntropyVal = 1000;
        foreach (Cell cell in Grid)
        {
            if((cell.tiles.Count < lowestEntropyVal)&&(cell.isCollapsed == false)){
                lowestEntropyVal = cell.tiles.Count;
            }
        }

        foreach (Cell cell in Grid)
        {
            if((cell.tiles.Count == lowestEntropyVal)&&(cell.isCollapsed == false)){
                lowestEntropy.Add(cell);
            }
        }

        return lowestEntropy;
    }

    
    public void Collapse(List<Tile> options, Cell cell){
        if(options.Count > 0){
            Tile choice = options[Random.Range(0,options.Count)];
            SpriteRenderer cellSpriteRenderer = cell.GetComponent<SpriteRenderer>();
            SpriteRenderer tileSpriteRenderer = choice.GetComponent<SpriteRenderer>();
            cellSpriteRenderer.sprite = tileSpriteRenderer.sprite;
            cell.currentSprite = choice;
        }else{
            cell.currentSprite = tiles[1];
            Debug.Log($"Options = 0 {cell.name}");
        }
            cell.isCollapsed = true;
    }

    void PlacePlayerInCenter(){
        Vector3 center = new Vector3(tileMapSizeX/2,tileMapSizeY/2,0);
        playerTransform.position = center;
    }

    public bool IsFinished(){
        foreach (Cell cell in Grid)
        {
            if(cell.isCollapsed == false){
                return false;
            }
        }
        return true;
    }
}
