using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject missile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnBomb());
    }

    IEnumerator SpawnBomb()
    {
        while (true) // 무한 루프
        {
            // 프리팹을 소환
            Instantiate(bomb, transform.position, Quaternion.identity);

            // 1초 기다리기
            yield return new WaitForSeconds(1f);
        }
    }
}
