using UnityEngine;

public class Missile : Projectile
{
    protected override void Start()
    {
        base.Start();
        damage = (int)MasterDamage.Missile;
    }
}
