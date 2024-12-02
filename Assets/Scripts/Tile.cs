using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void DHitTrigger(Tile tile, int indexY, int indexX);
    public event DHitTrigger HitTrigger;
    
    // Y X�� Ȯ�ο�����, ���� ������ ����
    public int indexY { get; set; }
    public int indexX { get; set; }
    public GameObject Hitffect;

    public void ActivateFIre()
    {
        StartCoroutine(DoFireEffect());
    }

    IEnumerator DoFireEffect()
    {
        Hitffect.SetActive(true);
        yield return new WaitForSeconds(3);
        Hitffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            HitTrigger?.Invoke(this, indexY, indexX);
        }
    }
}
