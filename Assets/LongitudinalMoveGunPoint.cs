using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class LongitudinalMoveGunPoint : AbstractGunPoint
{
    protected Vector3 _deffPos;

    public override void Init(Config config, CancellationToken onDestroyCTS)
    {
        base.Init(config, onDestroyCTS);
        _deffPos = transform.localPosition;
    }

    public override void OnStartShoot(CancellationToken shootCT, float fireRate, float startAnimValue)
    {
        MoveAnimation(fireRate).Forget();
    }


    public async UniTaskVoid MoveAnimation(float animSpeedMod)
    {
        float t = 0;
        while (t < 1 && !_onDestroyCTS.IsCancellationRequested)
        {
            t += Time.deltaTime * animSpeedMod;
            t = Mathf.Clamp01(t);
            float zOffset = _config.ForwardMovementAnimationCurve.Evaluate(t);
            transform.localPosition = _deffPos + Vector3.forward * zOffset;
            await UniTask.Yield();
        }
    }

   
}
