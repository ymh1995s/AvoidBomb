using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // 리스너 패턴!
    public delegate void DHitTrigger(Tile tile, int indexY, int indexX);
    public event DHitTrigger HitTrigger;

    // 하위 객체
    public GameObject Ground;
    public GameObject Obstacle;
    public GameObject Hitffect;

    //
    public Animator animator;
    
    // Y X는 BFS용도로
    public int indexY { get; set; }
    public int indexX { get; set; }
    private Coroutine coFire;
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

        animator = GetComponent<Animator>();
    }

    public void ActivateFIre(bool directHit)
    {
        if (state == State.Obstacle)
            return;

        if(directHit ==true)
        {
            // TODO : 직격 데미지
            // TODO : 삭제
            // TODO : ??아닌가? 삭제 보류
        }

        if(state==State.Fire)
        {
            StopCoroutine(coFire);
        }
        coFire = StartCoroutine(CoFireEffect());
    }

    IEnumerator CoFireEffect()
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
            Destroy(other.gameObject);

            Projectile projectile = other.GetComponent<Projectile>();
            Vector3 playerPos = GameManager.Instance.player.transform.position;
            // TODO 플레이어 히트는 플레이어에서 처리
            if((int)playerPos.x == transform.position.x && (int)playerPos.z == transform.position.z)
            {
                projectile.PlayerHit();
            }


            if (state == State.Obstacle) return;
            else
            {
                HitTrigger?.Invoke(this, indexX, indexY);
            }           
        }
    }

    public void SetObstacle()
    {
        state = State.Obstacle;
        animator.Play("Create");
    }

}
