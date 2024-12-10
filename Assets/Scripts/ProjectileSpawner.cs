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
            float waitTime = Random.Range(2.0f, 2.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
