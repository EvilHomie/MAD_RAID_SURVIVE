using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MachineGun : WarmingUpWeapon
{
    //async UniTaskVoid MachineGunWeaponShootingTask(CancellationToken shootCT, MachineGun weapon)
    //{
    //    weapon.OnChangeWarmingState(true);
    //    for (int i = 0; i < weapon.GunPointsCount; i++) _gunpointAnimationService.GunPointWarmingUpRotateAnimation(shootCT, weapon.fireRate, weapon.WarmingValue, weapon.GunPoinForNextShoot()).Forget();

    //    while (true)
    //    {
    //        if (!shootCT.IsCancellationRequested)
    //        {
    //            if (weapon.IsWarmed)
    //            {
    //                await UniTask.Yield();
    //                continue;
    //            }

    //            weapon.ChangeWarmingValue(-Time.deltaTime * weapon.fireRate);
    //            await UniTask.Yield();
    //        }
    //        else
    //        {
    //            if (weapon.IsCooling == false)
    //            {
    //                for (int i = 0; i < weapon.GunPointsCount; i++) _gunpointAnimationService.GunPointCoolingRotateAnimation(shootCT, weapon.fireRate, weapon.WarmingValue, weapon.GunPoinForNextShoot()).Forget();
    //                weapon.OnChangeWarmingState(false);
    //            }

    //            if (weapon._warmingValue <= 0)
    //            {
    //                break;
    //            }

    //            weapon._warmingValue -= Time.deltaTime * weapon.fireRate;
    //            weapon._warmingValue = Mathf.Clamp01(weapon._warmingValue);

    //            await UniTask.Yield();
    //        }
    //    }

    //}
}
