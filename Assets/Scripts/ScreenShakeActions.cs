using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        float shakeShakeIntensity = 2f;
        ScreenShake.Instance.Shake(shakeShakeIntensity);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        float grenadeShakeIntensity = 5f;
        ScreenShake.Instance.Shake(grenadeShakeIntensity);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
