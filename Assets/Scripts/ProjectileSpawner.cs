using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject missile;

    void Start()
    {
        StartCoroutine(SpawnBomb());
    }

    IEnumerator SpawnBomb()
    {
        while (true)
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
