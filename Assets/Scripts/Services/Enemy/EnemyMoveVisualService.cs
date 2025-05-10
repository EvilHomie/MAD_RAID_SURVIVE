using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyMoveVisualService : AbstractInRaidService
{
    readonly int _mainTextureOffsetValuePropertyID = Shader.PropertyToID("_TextureOffset");

    List<Wheel> _enemiesWheels;
    List<Caterpillar> _enemiesCaterpillars;
    List<Enemy> _enemies;

    [Inject]
    public void Construct()
    {
        _enemies = new();
        _enemiesWheels = new();
        _enemiesCaterpillars = new();
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
        _enemiesWheels.Clear();
        _enemiesCaterpillars.Clear();
        _enemies.Clear();

    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _enemies.Add(enemy);
        foreach (var movePart in enemy.MoveParts)
        {
            if (movePart.VehiclePartType == VehiclePartType.Wheel)
            {
                _enemiesWheels.Add(movePart as Wheel);
            }
            else if (movePart.VehiclePartType == VehiclePartType.Caterpillar)
            {
                _enemiesCaterpillars.Add(movePart as Caterpillar);
            }
        }
    }

    private void CustomUpdate()
    {
        WheelsAnimation();
        CaterpillarsAnimation();
        OnSidewaysTurnAnimation();
    }

    void WheelsAnimation()
    {
        for (int i = _enemiesWheels.Count - 1; i >= 0; i--)
        {
            if (_enemiesWheels[i] == null)
            {
                _enemiesWheels.RemoveAt(i);
                continue;
            }
            AnimateWheel(_enemiesWheels[i]);
        }
    }
    void CaterpillarsAnimation()
    {
        for (int i = _enemiesCaterpillars.Count - 1; i >= 0; i--)
        {
            if (_enemiesCaterpillars[i] == null)
            {
                _enemiesCaterpillars.RemoveAt(i);
                continue;
            }
            AnimateCaterpillar(_enemiesCaterpillars[i]);
        }
    }

    void OnSidewaysTurnAnimation()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies[i].IsDead)
            {
                _enemies.RemoveAt(i);
                continue;
            }
            SidewaysTurnAnimation(_enemies[i]);
        }
    }

    void AnimateWheel(Wheel wheel)
    {
        wheel.transform.Rotate(Vector3.right, _config.WheelRotationSpeed * Time.deltaTime, Space.Self);
    }
    void AnimateCaterpillar(Caterpillar caterpillar)
    {
        Vector3 currentTextureOffset = caterpillar.TextureOffset;
        float textureAnimationSpeed = _config.CaterpillarTextureOffsetSpeed * Time.deltaTime;
        currentTextureOffset.x += textureAnimationSpeed;
        caterpillar.TextureOffset = currentTextureOffset;

        caterpillar.Tape.material.SetVector(_mainTextureOffsetValuePropertyID, currentTextureOffset);
        foreach (Transform rotationPart in caterpillar.AdditionalRotationParts)
        {
            rotationPart.Rotate(Vector3.right, textureAnimationSpeed * _config.CaterpillarRotatingPartModSpeed, Space.Self);
        }
    }

    void SidewaysTurnAnimation(Enemy enemy)
    {
        float changePosDelta = enemy.LastZPos - enemy.transform.position.z;
        float movePartRotateStep = _config.MovePartSidewaysTurnSpeed * Time.deltaTime;
        float bodyRotateStep = _config.BodySidewaysTurnSpeed * Time.deltaTime;

        if (Mathf.Abs(changePosDelta) > _config.CheckZPosThreshold)
        {
            float rotateAngle = Mathf.Clamp(changePosDelta * _config.PosToAngleMod, -_config.MaxAngleForMoverPart, _config.MaxAngleForMoverPart);

            AnimateSidewaysTurn(enemy.VehicleBody.transform, rotateAngle, bodyRotateStep);
            foreach (var movePart in enemy.MoveParts)
            {
                if (!movePart.WithSidewaysTurnAnimation || movePart == null) continue;
                AnimateSidewaysTurn(movePart.transform, rotateAngle, movePartRotateStep);
            }
        }
        else
        {
            AnimateSidewaysTurn(enemy.VehicleBody.transform, 0, bodyRotateStep);
            foreach (var movePart in enemy.MoveParts)
            {
                if (!movePart.WithSidewaysTurnAnimation || movePart == null) continue;
                AnimateSidewaysTurn(movePart.transform, 0, movePartRotateStep);
            }
        }
        enemy.LastZPos = enemy.transform.position.z;
    }

    void AnimateSidewaysTurn(Transform movePart, float newAngle, float rotateSpeedTick)
    {
        if(rotateSpeedTick == 0) return;
        //float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, _sidewaysTargetRotation.y));
        //float smoothMod = Mathf.InverseLerp(0, maxValue, deltaAngle);

        Vector3 currentRotation = movePart.transform.eulerAngles;
        float xRotation = currentRotation.x;
        currentRotation.x = 0;

        Vector3 targetRotation = Vector3.up * (newAngle + 90);

        Quaternion newRotation = Quaternion.RotateTowards(Quaternion.Euler(currentRotation), Quaternion.Euler(targetRotation), rotateSpeedTick /** smoothMod*/);
        currentRotation = newRotation.eulerAngles;
        currentRotation.x = xRotation;

        movePart.rotation = Quaternion.Euler(currentRotation);
    }



}
