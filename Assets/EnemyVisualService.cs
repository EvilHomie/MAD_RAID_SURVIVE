using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyVisualService : AbstractInRaidService
{
    [SerializeField] float mod;


    List<Enemy> _enemies;

    [Inject]
    public void Construct()
    {
        _enemies = new();
    }

    protected override void OnStartRaid()
    {
        _eventBus.OnSpawnEnemy += OnEnemySpawned;
        _gameFlowService.CustomUpdate += CustomUpdate;
    }

    protected override void OnStopRaid()
    {
        _eventBus.OnSpawnEnemy -= OnEnemySpawned;
        _gameFlowService.CustomUpdate -= CustomUpdate;
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    private void CustomUpdate()
    {
        MoveAnimation();
    }

    void MoveAnimation()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies[i] == null)
            {
                _enemies.RemoveAt(i);
                continue;
            }

            foreach (var movePart in _enemies[i].MoveParts)
            {
                //if (_enemies[i].MoverPartsType == VehiclePartType.Wheel)
                //{
                //    movePart.MoveRotateAnimationTick(_config.WheelRotationSpeed * Time.deltaTime);
                //}
                //else if (_enemies[i].MoverPartsType == VehiclePartType.Caterpillar)
                //{
                //    movePart.MoveRotateAnimationTick(_config.CaterpillarTextureOffsetSpeed * Time.deltaTime, _config.CaterpillarRotatingPartModSpeed);
                //}
            }
            RotateAnimation(_enemies[i]);
        }
    }
    void RotateAnimation(Enemy enemy)
    {
        float changePosDelta = enemy._lastZPos - enemy.transform.position.z;

        float rotateAngle = Mathf.Clamp(changePosDelta * mod, -_config.MaxAngleForMoverPart, _config.MaxAngleForMoverPart);
        if (Mathf.Abs(changePosDelta) > _config.CheckZPosThreshold)
        {
            foreach (var movePart in enemy.MoveParts)
            {
                movePart.SidewaysTurnAnimationTick(rotateAngle, _config.SidewaysTurnSpeed * Time.deltaTime);
            }
        }
        else
        {
            foreach (var movePart in enemy.MoveParts)
            {
                movePart.SidewaysTurnAnimationTick(0, _config.SidewaysTurnSpeed * Time.deltaTime);
            }
        }

        enemy._lastZPos = enemy.transform.position.z;
    }

}
