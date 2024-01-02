using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public int numOfChunksX = 0;
    public int numOfChunksY = 0;
    public int chunkSizeX;
    public int chunkSizeY;
    TileGenerator chunk;
    TileGenerator[,] chunks;
    GameObject chunkParent;
    void Start()
    {
        for (int x = 0; x < numOfChunksX; x++)
        {
                for (int y = 0; y < numOfChunksY; y++)
                {
                    chunk = new TileGenerator();
                    chunk.tileMapSizeX = chunkSizeX;
                    chunk.tileMapSizeY = chunkSizeY;
                    chunks[x,y] = chunk;
                    Vector3Int chunkpos = new Vector3Int(x*chunkSizeX,y*chunkSizeY,0);
                    GameObject chunkparent = Instantiate(chunkParent,chunkpos,quaternion.identity);
                    Instantiate(chunk,chunkpos,quaternion.identity,chunkParent.transform);
                }
        }

    }
}
