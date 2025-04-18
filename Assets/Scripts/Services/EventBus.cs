using System;
using UnityEngine;

public class EventBus
{
    public Action OnStartRaid;
    public Action OnStopRaid;
    public Action<Enemy> OnSpawnEnemy;
    public Action<Enemy> OnEnemyDie;
    public Action<Transform> OnSpawnEnvironmentObject;
    public Action<int> OnChangeEnemiesTir;

    public Action OnPauseFight;
    public Action OnResumeFight;
}
