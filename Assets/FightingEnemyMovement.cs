using Pathfinding;
using UnityEngine;

public class FightingEnemyMovement : EnemyMovement
{
    public FightingEnemyMovement(IAstarAI astarAI, Vector3 endPos, Enemy enemy, Config config) : base(astarAI, endPos, enemy, config)
    {
    }
}
