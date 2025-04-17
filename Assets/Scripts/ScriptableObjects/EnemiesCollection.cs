using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesCollection", menuName = "Scriptable Objects/EnemiesCollection")]
public class EnemiesCollection : ScriptableObject
{
    [SerializeField] FightingEnemy[] _fightingEnemies;
    [SerializeField] BonusEnemy _bonusEnemy;
    public FightingEnemy[] FightingEnemies => _fightingEnemies;
    public BonusEnemy BonusEnemy => _bonusEnemy;
}
