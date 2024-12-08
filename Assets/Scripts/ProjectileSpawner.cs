using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject missile; // TODO

    void Start()
    {
        StartCoroutine(SpawnBomb());
    }

    IEnumerator SpawnBomb()
    {
        while (true)
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
