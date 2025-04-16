using System;

public class EventBus
{
    public Action OnStartRaid;
    public Action OnStopRaid;
    public Action<Enemy> OnEnemySpawned;
    public Action<Enemy> OnEnemyDie;
}
