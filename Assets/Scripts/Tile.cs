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
    public ParticleSystem fireEffect;

    // 하위 컴포넌트
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

        if(state==State.Fire)
        {
            StopCoroutine(coFire);
        }
        coFire = StartCoroutine(CoFireEffect());
    }

    IEnumerator CoFireEffect()
    {
        state = State.Fire;
        fireEffect.Play();
        yield return new WaitForSeconds(3);
        fireEffect.Stop();
        state = State.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);

            // 화염 이벤트
            if (state == State.Obstacle) return;
            else
            {
                HitTrigger?.Invoke(this, indexX, indexY);
            }           
        }
    }

    public void SetObstacle()
    {
        Obstacle.SetActive(true);
        state = State.Obstacle;
        animator.Play("Create");
    }
}
