using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyVisualServiceOnMove : AbstractInRaidService
{
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
        _enemies.Clear();
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

            //foreach (var movePart in _enemies[i].MoveParts)
            //{
            //    if (_enemies[i].MoverPartsType == VehiclePartType.Wheel)
            //    {
            //        movePart.MoveRotateAnimationTick(_config.WheelRotationSpeed * Time.deltaTime);
            //    }
            //    else if (_enemies[i].MoverPartsType == VehiclePartType.Caterpillar)
            //    {
            //        movePart.MoveRotateAnimationTick(_config.CaterpillarTextureOffsetSpeed * Time.deltaTime, _config.CaterpillarRotatingPartModSpeed);
            //    }
            //}
            SideWaysTurnAnimation(_enemies[i]);
        }
    }
    void SideWaysTurnAnimation(Enemy enemy)
    {
        float changePosDelta = enemy._lastZPos - enemy.transform.position.z;

        float rotateAngle = Mathf.Clamp(changePosDelta * _config.PosToAngleMod, -_config.MaxAngleForMoverPart, _config.MaxAngleForMoverPart);
        float movePartRotateStep = _config.MovePartSidewaysTurnSpeed * Time.deltaTime;
        float bodyRotateStep = _config.BodySidewaysTurnSpeed * Time.deltaTime;

        if (Mathf.Abs(changePosDelta) > _config.CheckZPosThreshold)
        {
            //enemy.VehicleBody.SidewaysTurnAnimationTick(rotateAngle, bodyRotateStep);
            foreach (var movePart in enemy.MoveParts)
            {
                movePart.SidewaysTurnAnimationTick(rotateAngle, movePartRotateStep);
            }
        }
        else
        {
            //enemy.VehicleBody.SidewaysTurnAnimationTick(0, bodyRotateStep);
            foreach (var movePart in enemy.MoveParts)
            {
                movePart.SidewaysTurnAnimationTick(0, movePartRotateStep);
            }
        }

        enemy._lastZPos = enemy.transform.position.z;
    }

}
