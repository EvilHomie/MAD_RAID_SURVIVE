using System;
using UnityEngine;

public class EventBus
{
    public Action OnStartRaid;
    public Action OnStopRaid;



    public Action<Enemy> OnSpawnEnemy;
    public Action<Enemy> OnEnemyDie;
    public Action<VehiclePart> OnVehiclePartDie;
    public Action<float> OnChangeEnemiesPower;
    public Action<int> OnChangeEnemiesTir;




    public Action<EnvironmentObject> OnSpawnEnvironmentObject;

    public Action OnPauseFight;
    public Action OnResumeFight;

    public Action<AbstractWeapon> OnPlayerChangeWeapon;
}
