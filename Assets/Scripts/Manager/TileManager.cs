using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Tile[,] tilesInfo;
    public GameObject tileParent;

    private int xoffsetStart = -30;
    private int xoffsetEnd = 30;
    private int yoffsetStart = -15;
    private int yoffsetEnd = 150;

    // For BFS
    int[] dy = { -1, 0, 0, 1 };
    int[] dx = { 0, 1, -1, 0 };
    int[,] visited;

    private void Start()
    {
        //BFS 방문배열 초기화
        visited = new int[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];

        tilesInfo = new Tile[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];
        for (int i = xoffsetStart; i< xoffsetEnd;i++)
        {
            for (int j = yoffsetStart; j< yoffsetEnd;j++)
            {
                Vector3 thisTilePos = new Vector3(i, -0.05f, j);
                int thisXoffset = i - xoffsetStart;
                int thisYoffset = j - yoffsetStart;
                GameObject thisTile = Instantiate(tilePrefab, thisTilePos, Quaternion.identity, tileParent.transform);
                tilesInfo[thisXoffset, thisYoffset] = thisTile.GetComponent<Tile>();
                tilesInfo[thisXoffset, thisYoffset].indexY = thisXoffset;
                tilesInfo[thisXoffset, thisYoffset].indexX = thisYoffset;
                tilesInfo[thisXoffset, thisYoffset].HitTrigger += OnProjectileHitHandler;
            }
        }
    }   

    public void OnProjectileHitHandler(Tile tile, int indexY, int indexX)
    {
        // 직격!
        tile.ActivateFIre(true);

        // 반경화염!
        // This is BFS!
        //Array.Clear(visited, 0, visited.Length);
        Queue<ValueTuple<int, int>> q = new Queue<ValueTuple<int, int>>();
        Queue<ValueTuple<int, int>> optimizationQ = new Queue<ValueTuple<int, int>>();
        q.Enqueue((indexY, indexX));
        optimizationQ.Enqueue((indexY, indexX));

        while (q.Count > 0)
        {
            var current = q.Dequeue();
            int y = current.Item1;
            int x = current.Item2;

            for (int i = 0; i < 4; i++)
            {
                int ny = y + dy[i];
                int nx = x + dx[i];

                if (ny < 0 || nx < 0 || visited[ny, nx] != 0) continue;
                visited[ny, nx] = visited[y, x] + 1;
                optimizationQ.Enqueue((ny, nx));
                if (visited[ny, nx] < 2)
                {
                    q.Enqueue((ny, nx));
                    tilesInfo[ny, nx].ActivateFIre(false);
                }
            }
        }

        // 전체 배열을 0으로 초기화 하면 계산이 못따라가서
        // 방문한 곳만 0으로 초기화하면 최적화가 되지 않을까?
        while (optimizationQ.Count > 0)
        {
            var current = optimizationQ.Dequeue();
            visited[current.Item1, current.Item2] = 0;
        }
    }
}
