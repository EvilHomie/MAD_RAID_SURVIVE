using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Threading;
using System;
using UnityEngine;

public abstract class EnemyMovement
{
    IAstarAI _astarAI;
    Vector3 _endPos;
    Enemy _enemy;
    Config _config;
    CancellationTokenSource _ctsOnDie;
    CancellationToken _onDestroyMonoBesh;

    protected EnemyMovement(IAstarAI astarAI, Vector3 endPos, Enemy enemy, Config config)
    {
        _astarAI = astarAI;
        _endPos = endPos;
        _enemy = enemy;
        _config = config;
        if (_ctsOnDie != null)
        {
            _ctsOnDie.Dispose();
        }
        _ctsOnDie = new CancellationTokenSource();

        _enemy.onDie += OnDie;
        _enemy.onDestroyMonoBeh += OnDestroyMonoBeh;
    }

    private void OnDestroyMonoBeh()
    {
        _enemy.onDestroyMonoBeh -= OnDestroyMonoBeh;
    }

    private void OnDie()
    {
        _enemy.onDie -= OnDie;
        _ctsOnDie.Cancel();
        _ctsOnDie.Dispose();
    }

    public void StartMove()
    {
        _astarAI.destination = _endPos;



        CheckReachedEndOfPath(_ctsOnDie.Token).Forget();
    }


    async UniTaskVoid CheckReachedEndOfPath(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.CheckReachedEndOfPath), cancellationToken: ct);
            if (_astarAI.reachedEndOfPath)
            {

            }
            else
            {

            }
        }
    }
}
