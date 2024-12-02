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
        while (true) // ���� ����
        {
            // �������� ��ȯ
            Instantiate(bomb, transform.position, Quaternion.identity);

            // 1�� ��ٸ���
            yield return new WaitForSeconds(1f);
        }
    }
}
