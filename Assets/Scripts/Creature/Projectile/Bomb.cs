using UnityEngine;

public class Bomb : Projectile
{
    public GameObject explosionSound;

    protected override void Start()
    {
        base.Start();
        damage = (int)MasterDamage.Bomb;
    }

    private void OnDisable()
    {
        Invoke(nameof(InstantiateWithDelay), 0f);
    }

    private void InstantiateWithDelay()
    {
        GameObject explosionInstance = Instantiate(explosionSound, transform.position, transform.rotation);
        Destroy(explosionInstance, 2f);
    }
}
