using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class Config : ScriptableObject
{
    #region ENVIRONMENTS CONFIG DATA
    [Header("ENVIRONMENTS")]
    [SerializeField] float _groundMoveSpeed;
    [SerializeField] float _environmentMoveSpeed;
    [SerializeField] AreaZone _environmentsAreaZone;
    [SerializeField] float _objectsMaxTiltOnSpawn;
    [SerializeField] float _objectsMaxRotationOnSpawn;
    [SerializeField] float _largeObjectsCorectYPosByTiltMod;
    [SerializeField] float _mediumObjectsCorectYPosByTiltMod;
    [SerializeField] float _smallObjectsCorectYPosByTiltMod;

    public float GroundMoveSpeed => _groundMoveSpeed;
    public float EnvironmentMoveSpeed => _environmentMoveSpeed;
    public AreaZone EnvironmentsAreaZone { get => _environmentsAreaZone; }

    public float ObjectsMaxTiltOnSpawn => _objectsMaxTiltOnSpawn;
    public float ObjectsMaxRotationOnSpawn => _objectsMaxRotationOnSpawn;
    public float LargeObjectsCorectYPosByTiltMod => _largeObjectsCorectYPosByTiltMod;
    public float MediumObjectsCorectYPosByTiltMod => _mediumObjectsCorectYPosByTiltMod;
    public float SmallObjectsCorectYPosByTiltMod => _smallObjectsCorectYPosByTiltMod;
    #endregion

    #region ENEMIES
    [Space(25)]
    [Header("ENEMIES")]
    [SerializeField] float _checkReachedEndOfPathRepeatDelay;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _simulateFloatingPosRepeatRange;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _changePosInFightZoneRepeatRange;

    [SerializeField] float _simulateFloatingPosRadius;
    [SerializeField] float _enemySpeedOutOfFightZone;
    [SerializeField] float _enemySpeedInsideFightZone;
    [SerializeField] List<Vector3> _fightZonePointsPositions;

    public List<Vector3> FightZonePointsPositions => _fightZonePointsPositions;
    public float CheckReachedEndOfPathRepeatDelay => _checkReachedEndOfPathRepeatDelay;
    public Vector2 ChangePosInFightZoneRepeatRange => _changePosInFightZoneRepeatRange;
    public float SimulateFloatingPosRadius => _simulateFloatingPosRadius;
    public Vector2 SimulateFloatingPosRepeatRange => _simulateFloatingPosRepeatRange;
    public float EnemySpeedOutOfFightZone => _enemySpeedOutOfFightZone;
    public float EnemySpeedInsideFightZone => _enemySpeedInsideFightZone;
    #endregion

    #region SPAWNER
    [Space(25)]
    [Header("SPAWNER")]
    [SerializeField] int _maxFighingEnemiesCount;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _spawnFighingEnemyRepeatRange;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _spawnBonusEnemyRepeatRange;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _largeObjectSpawnRepeatRange;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _smallObjectSpawnRepeatRange;

    [SerializeField] AreaZone _bonusEnemyZone;
    [SerializeField] AreaZone _spawnEnemiesZone_Left;
    [SerializeField] AreaZone _spawnEnemiesZone_Right;


    public Vector2 SpawnFighingEnemyRepeatRange => _spawnFighingEnemyRepeatRange;
    public Vector2 SpawnBonusEnemyRepeatRange => _spawnBonusEnemyRepeatRange;

    public Vector2 LargeObjectSpawnRepeatRange => _largeObjectSpawnRepeatRange;
    public Vector2 SmallObjectSpawnRepeatRange => _smallObjectSpawnRepeatRange;

    public AreaZone BonusEnemyZone => _bonusEnemyZone;
    public AreaZone SpawnEnemiesZone_Left => _spawnEnemiesZone_Left;
    public AreaZone SpawnEnemiesZone_Right => _spawnEnemiesZone_Right;

    public int MaxFighingEnemiesCount => _maxFighingEnemiesCount;

    public void UpdateAreas(AreaZone environmentsZone, AreaZone bonusEnemyZone, AreaZone spawnEnemiesZone_Left, AreaZone spawnEnemiesZone_Right, List<Vector3> fightZonePointsPositions)
    {
        _environmentsAreaZone = environmentsZone;
        _bonusEnemyZone = bonusEnemyZone;
        _spawnEnemiesZone_Left = spawnEnemiesZone_Left;
        _spawnEnemiesZone_Right = spawnEnemiesZone_Right;
        _fightZonePointsPositions = new(fightZonePointsPositions);
    }
    #endregion

    #region PLAYER
    [Space(25)]
    [Header("PLAYER")]

    [SerializeField] float _PCMouseSensitivity;
    [SerializeField] float _mobileMouseSensitivity;
    [SerializeField] float _vertMaxRotationAngle;
    [SerializeField] float _horMaxRotationAngle;


    public float PCMouseSensitivity => _PCMouseSensitivity;
    public float MobileMouseSensitivity => _mobileMouseSensitivity;
    public float VertMaxRotationAngle => _vertMaxRotationAngle;
    public float HorMaxRotationAngle => _horMaxRotationAngle;
    #endregion

    #region WEAPON
    [Space(25)]
    [Header("WEAPON")]
    [SerializeField] AnimationCurve _forwardMovementAnimationCurve;
    [SerializeField] AnimationCurve _rotationAnimationCurve;
    [SerializeField] float _warmingTime;
    [SerializeField] float _maxRotationSpeed;
    [SerializeField] float _lightOnShootFlickDuration;
    public AnimationCurve ForwardMovementAnimationCurve => _forwardMovementAnimationCurve;
    public AnimationCurve RotationAnimationCurve => _rotationAnimationCurve;
    public float WarmingTime => _warmingTime;
    public float MaxRotationSpeed => _maxRotationSpeed;
    public float LightOnShootFlickDuration => _lightOnShootFlickDuration;
    #endregion
}