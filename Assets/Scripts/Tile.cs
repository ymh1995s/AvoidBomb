using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // 리스너 패턴!
    public delegate void DHitTrigger(Tile tile, int indexY, int indexX);
    public event DHitTrigger HitTrigger;
    
    // Y X는 BFS용도로
    public int indexY { get; set; }
    public int indexX { get; set; }
    public GameObject Hitffect;
    private Coroutine fireCoroutine;
    public State state;

    public enum State
    {
        None,
        Fire,
        Obstacle
    }

    private void Start()
    {
        state = State.None;
    }

    public void ActivateFIre(bool directHit)
    {
        if(directHit ==true)
        {
            // TODO : 직격 데미지
            // TODO : 삭제
            // TODO : ??아닌가? 삭제 보류
        }

        if(state==State.Fire)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(DoFireEffect());
    }

    IEnumerator DoFireEffect()
    {
        //state = State.Fire;
        //Hitffect.SetActive(true);
        yield return new WaitForSeconds(3);
        Hitffect.SetActive(false);
        state = State.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            if((int)playerPos.x == transform.position.x && (int)playerPos.z == transform.position.z)
            {
                projectile.PlayerHit();
            }

            Destroy(other.gameObject);
            HitTrigger?.Invoke(this, indexY, indexX);
        }
    }
}
