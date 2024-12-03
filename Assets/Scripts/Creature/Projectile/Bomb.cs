using UnityEngine;

public class Bomb : Projectile
{
    protected override void Start()
    {
        base.Start();
        damage = (int)MasterDamage.Bomb;
    }
}
