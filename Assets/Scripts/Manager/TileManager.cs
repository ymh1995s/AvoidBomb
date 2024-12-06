using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Tile[,] tilesInfo; // X Y 기준으로 통일함
    public GameObject tileParent;

    //private const int xoffsetStart = -30;
    //private const int xoffsetEnd = 30;
    //private const int yoffsetStart = -15;
    //private const int yoffsetEnd = 150;
    private const int xoffsetStart = -15;
    private const int xoffsetEnd = 15;
    private const int yoffsetStart = -15;
    private const int yoffsetEnd = 20;

    // For BFS
    int[] dy = { -1, 0, 0, 1 };
    int[] dx = { 0, 1, -1, 0 };
    int[,] visited;

    private void Start()
    {
        Init();
        StartCoroutine(SetRandomObstacle());
    }   

    void Init()
    {
        // BFS 방문배열 초기화
        visited = new int[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];

        // 바닥 타일 정보 초기화
        tilesInfo = new Tile[xoffsetEnd - xoffsetStart, yoffsetEnd - yoffsetStart];
        for (int i = xoffsetStart; i < xoffsetEnd; i++)
        {
            for (int j = yoffsetStart; j < yoffsetEnd; j++)
            {
                Vector3 thisTilePos = new Vector3(i, -0.05f, j);
                int thisXoffset = i - xoffsetStart;
                int thisYoffset = j - yoffsetStart;
                GameObject thisTile = Instantiate(tilePrefab, thisTilePos, Quaternion.identity, tileParent.transform);
                tilesInfo[thisXoffset, thisYoffset] = thisTile.GetComponent<Tile>();
                tilesInfo[thisXoffset, thisYoffset].indexX = thisXoffset;
                tilesInfo[thisXoffset, thisYoffset].indexY = thisYoffset;
                tilesInfo[thisXoffset, thisYoffset].name = $"tile [{thisXoffset},{thisYoffset}]";
                tilesInfo[thisXoffset, thisYoffset].HitTrigger += OnProjectileHitHandler;
            }
        }
    }

    //void SetRandomObstacle()
    IEnumerator SetRandomObstacle()
    {
        // 1초 대기 안하면 Start 꼬여서 Aniamator를 못찾음
        yield return new WaitForSeconds(1f);
        int maxObstacleCount = UnityEngine.Random.Range(5, 10);
        Debug.Log($"{maxObstacleCount} Created..");

        for(int i = 0;i < maxObstacleCount; i++)
        {
            int randomX = UnityEngine.Random.Range(0, xoffsetEnd - xoffsetStart);
            int randomY = UnityEngine.Random.Range(0, yoffsetEnd - yoffsetStart);

            //Debug.Log($"{randomX} {randomY}");
            tilesInfo[randomX, randomY].SetObstacle();
        }
    }

    public void OnProjectileHitHandler(Tile tile, int indexX, int indexY)
    {
        // 해당 타일 화염!
        tile.ActivateFIre(true);

        // BFS 반경화염!
        //Array.Clear(visited, 0, visited.Length);
        Queue<ValueTuple<int, int>> q = new Queue<ValueTuple<int, int>>();
        Queue<ValueTuple<int, int>> optimizationQ = new Queue<ValueTuple<int, int>>();
        q.Enqueue((indexX, indexY));
        optimizationQ.Enqueue((indexX, indexY));

        while (q.Count > 0)
        {
            var current = q.Dequeue();
            int x = current.Item1;
            int y = current.Item2;

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (ny < 0 || nx < 0 || visited[nx, ny] != 0) continue;
                visited[nx, ny] = visited[x, y] + 1;
                optimizationQ.Enqueue((nx, ny));
                if (visited[nx, ny] < 2)
                {
                    q.Enqueue((nx, ny));
                    tilesInfo[nx, ny].ActivateFIre(false);
                }
            }
        }
        // 전체 초기화 대신 방문한 곳만 0으로 초기화해서 최적화, 버그 여지는 미지수
        while (optimizationQ.Count > 0)
        {
            var current = optimizationQ.Dequeue();
            visited[current.Item1, current.Item2] = 0;
        }
    }
}
