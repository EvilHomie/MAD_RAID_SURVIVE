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
    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _largeObjectSpawnDelayRange;
    [MinMaxRangeSlider(0f, 100f)]
    [SerializeField] Vector2 _smallObjectSpawnDelayRange;
    public float GroundMoveSpeed => _groundMoveSpeed;
    public float EnvironmentMoveSpeed => _environmentMoveSpeed;
    public AreaZone EnvironmentsAreaZone { get => _environmentsAreaZone; set => _environmentsAreaZone = value; }
    public Vector2 LargeObjectSpawnDelayRange => _largeObjectSpawnDelayRange;
    public Vector2 SmallObjectSpawnDelayRange => _smallObjectSpawnDelayRange;
    public float ObjectsMaxTiltOnSpawn => _objectsMaxTiltOnSpawn;
    public float ObjectsMaxRotationOnSpawn => _objectsMaxRotationOnSpawn;
    public float LargeObjectsCorectYPosByTiltMod => _largeObjectsCorectYPosByTiltMod;
    public float MediumObjectsCorectYPosByTiltMod => _mediumObjectsCorectYPosByTiltMod;
    public float SmallObjectsCorectYPosByTiltMod => _smallObjectsCorectYPosByTiltMod;
    #endregion

    #region ENEMIES
    [Space(50)]
    [Header("ENEMIES")]
    [SerializeField] AreaZone _bonusEnemyAreaZone;
    [SerializeField] AreaZone _enemyFightAreaZone;



    public AreaZone BonusEnemyAreaZone => _bonusEnemyAreaZone;
    public AreaZone EnemyFightAreaZone => _enemyFightAreaZone;
    #endregion
}
