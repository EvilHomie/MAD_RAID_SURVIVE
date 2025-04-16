using Pathfinding;
using UnityEngine;

public class BonusEnemyMovement : EnemyMovement
{
    public BonusEnemyMovement(IAstarAI astarAI, Vector3 endPos, Enemy enemy, Config config) : base(astarAI, endPos, enemy, config)
    {
    }
}
