using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[,] tiles;
    public Tile[,] tilesInfo;
    public GameObject tileParent;

    private int xoffsetStart = -30;
    private int xoffsetEnd = 30;
    private int yoffsetStart = -15;
    private int yoffsetEnd = 150;


    private void Start()
    {
        //BFS 방문배열 초기화
        visited = new int[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];

        //tiles = new GameObject[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];
        tilesInfo = new Tile[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];
        for (int i = xoffsetStart; i< xoffsetEnd;i++)
        {
            for (int j = yoffsetStart; j< yoffsetEnd;j++)
            {
                Vector3 thisTilePos = new Vector3(i, -0.05f, j);
                GameObject thisTile = Instantiate(tilePrefab, thisTilePos, Quaternion.identity, tileParent.transform);
                //tiles[i - xoffsetStart, j - yoffsetStart] = thisTile;
                tilesInfo[i - xoffsetStart, j - yoffsetStart] = thisTile.GetComponent<Tile>();
                tilesInfo[i - xoffsetStart, j - yoffsetStart].indexY = i;
                tilesInfo[i - xoffsetStart, j - yoffsetStart].indexX = j;
                tilesInfo[i - xoffsetStart, j - yoffsetStart].HitTrigger += OnProjectileHitHandler;
            }
        }
    }


    int[] dy = { -1, 0, 0, 1 };
    int[] dx = { 0, 1, -1, 0 };
    int[,] visited;

    public void OnProjectileHitHandler(Tile tile, int indexY, int indexX)
    {
        print($"{indexY} {indexX} Triggered");
        tile.ActivateFIre();

        // This is BFS!
        Array.Clear(visited, 0, visited.Length);
        Queue<ValueTuple<int, int>> q = new Queue<ValueTuple<int, int>>();
        q.Enqueue((indexY, indexX));

        while(q.Count > 0)
        {
            var current = q.Dequeue();
            int y = current.Item1;
            int x = current.Item2;
            for (int i = 0; i < 4; i++)
            {
                int ny = y + dy[i];
                int nx = x + dx[i];
                if (ny<0|| nx<0||visited[ny, nx] != 0) continue;
                visited[ny, nx] = visited[y, x] + 1;
                if (visited[ny, nx] <= 2)
                {
                    q.Enqueue((ny, nx));
                    tilesInfo[ny, nx].ActivateFIre();
                }
            }
        }
    }
}
