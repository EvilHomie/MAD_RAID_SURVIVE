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
    

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _simulateFloatingPosRepeatRange;

    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _changePosInFightZoneRepeatRange;

    [SerializeField] float _simulateFloatingPosRadius;
    [SerializeField] float _enemySpeedOutOfFightZone;
    [SerializeField] float _enemySpeedInsideFightZone;
    [SerializeField] float _bonusEnemySpeed;
    [SerializeField] float _onDieTranslationMaxSpeed;
    [SerializeField] float _onDieAccelerationSpeed;
    [SerializeField] float _destroyVehiclePartsPos;
    [SerializeField] List<Vector3> _fightZonePointsPositions;
    [SerializeField] float _explosivePartDamageToBodyPercent;
    [SerializeField] float _slowDownTimeInFightZone;
    [SerializeField] float _slowDownTimeOutOfFightZone;

    public List<Vector3> FightZonePointsPositions => _fightZonePointsPositions;    
    public Vector2 ChangePosInFightZoneRepeatRange => _changePosInFightZoneRepeatRange;
    public float SimulateFloatingPosRadius => _simulateFloatingPosRadius;
    public Vector2 SimulateFloatingPosRepeatRange => _simulateFloatingPosRepeatRange;
    public float EnemySpeedOutOfFightZone => _enemySpeedOutOfFightZone;
    public float EnemySpeedInsideFightZone => _enemySpeedInsideFightZone;
    public float BonusEnemySpeed => _bonusEnemySpeed;
    public float OnDieTranslationMaxSpeed => _onDieTranslationMaxSpeed;
    public float OnDieAccelerationSpeed => _onDieAccelerationSpeed;
    public float DestroyVehiclePartsPos => _destroyVehiclePartsPos;
    public float ExplosivePartDamageToBodyPercent => _explosivePartDamageToBodyPercent;
    public float SlowDownTimeInFightZone => _slowDownTimeInFightZone;
    public float SlowDownTimeOutOfFightZone => _slowDownTimeOutOfFightZone;

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
    [SerializeField] float _bordersOffset;


    public Vector2 SpawnFighingEnemyRepeatRange => _spawnFighingEnemyRepeatRange;
    public Vector2 SpawnBonusEnemyRepeatRange => _spawnBonusEnemyRepeatRange;

    public Vector2 LargeObjectSpawnRepeatRange => _largeObjectSpawnRepeatRange;
    public Vector2 SmallObjectSpawnRepeatRange => _smallObjectSpawnRepeatRange;

    public AreaZone BonusEnemyZone => _bonusEnemyZone;
    public AreaZone SpawnEnemiesZone_Left => _spawnEnemiesZone_Left;
    public AreaZone SpawnEnemiesZone_Right => _spawnEnemiesZone_Right;
    public int MaxFighingEnemiesCount => _maxFighingEnemiesCount;
    public float BordersOffset => _bordersOffset;

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
    [SerializeField] float _lightOnSingleShootFlickDuration;
    [SerializeField] float _lightOnContinuouslyShootingFlickDuration;
    [SerializeField] int _particlesCountOnBulletCollision;
    [SerializeField] float _continuousShootCheckHitRepiteRate;
    public AnimationCurve ForwardMovementAnimationCurve => _forwardMovementAnimationCurve;
    public AnimationCurve RotationAnimationCurve => _rotationAnimationCurve;
    public float WarmingTime => _warmingTime;
    public float MaxRotationSpeed => _maxRotationSpeed;
    public float LightOnSingleShootFlickDuration => _lightOnSingleShootFlickDuration;
    public float LightOnContinuouslyShootingFlickDuration => _lightOnContinuouslyShootingFlickDuration;
    public int ParticlesCountOnBulletCollision => _particlesCountOnBulletCollision;

    public float ContinuousShootCheckHitRepiteRate => _continuousShootCheckHitRepiteRate;
    #endregion

    #region TIMINGS
    [Space(25)]
    [Header("TIMINGS")]
    [SerializeField] float _checkReachedEndOfPathRepeatDelay;
    [SerializeField] float _checkObjectForDestroyRepeatDelay;
    [SerializeField] float _increaseEnemyTirDelay;
    [SerializeField] float _increaseEnemyPowerValueByTime;
    [SerializeField] float _increaseEnemyPowerTickRate;
    public float CheckReachedEndOfPathRepeatDelay => _checkReachedEndOfPathRepeatDelay;
    public float CheckObjectForDestroyRepeatDelay => _checkObjectForDestroyRepeatDelay;
    public float IncreaseEnemyTirDelay => _increaseEnemyTirDelay;
    public float IncreaseEnemyPowerValueByTime => _increaseEnemyPowerValueByTime;
    public float IncreaseEnemyPowerTickRate => _increaseEnemyPowerTickRate;
    #endregion

    #region ENEMYVISUAL
    [Space(25)]
    [Header("ENEMYVISUAL")]
    [SerializeField] float _wheelRotationSpeed;
    [SerializeField] float _caterpillarTextureOffsetSpeed;
    [SerializeField] float _caterpillarRotatingPartModSpeed;
    [SerializeField] float _checkZPosThreshold;
    [SerializeField] float _maxAngleForMoverPart;
    [SerializeField] float _movePartSidewaysTurnSpeed;
    [SerializeField] float _bodySidewaysTurnSpeed;
    [SerializeField] float _posToAngleMod;
    [SerializeField] float _showHitDuration;
    [ColorUsage(false, true)]
    [SerializeField] Color _criticalHPColor;
    public float WheelRotationSpeed => _wheelRotationSpeed;
    public float CaterpillarTextureOffsetSpeed => _caterpillarTextureOffsetSpeed;
    public float CaterpillarRotatingPartModSpeed => _caterpillarRotatingPartModSpeed;
    public float CheckZPosThreshold => _checkZPosThreshold;
    public float MaxAngleForMoverPart => _maxAngleForMoverPart;
    public float MovePartSidewaysTurnSpeed => _movePartSidewaysTurnSpeed;
    public float BodySidewaysTurnSpeed => _bodySidewaysTurnSpeed;
    public float PosToAngleMod => _posToAngleMod;
    public float ShowHitDuration => _showHitDuration;
    public Color CriticalHPColor => _criticalHPColor;
    #endregion

    #region DETACH
    [Space(25)]
    [Header("DETACH")]
    [SerializeField] float _detachForce;
    [SerializeField] float _maxSpeedDetachedParts;
    [SerializeField] float _onDetachedAccelerationSpeed;
    [SerializeField] float _loseMovePartChangeCenterOfMassMod;
    public float DetachForce => _detachForce;
    public float MaxSpeedDetachedParts => _maxSpeedDetachedParts;
    public float OnDetachedAccelerationSpeed => _onDetachedAccelerationSpeed;
    public float LoseMovePartChangeCenterOfMassMod => _loseMovePartChangeCenterOfMassMod;
    #endregion
    //[Space(25)]


    //[SerializeField] Layer _
}