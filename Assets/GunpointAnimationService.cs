using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class GunpointAnimationService : MonoBehaviour
{
    CancellationToken cancellationTokenOnStopApplication;
    Config _config;

    [Inject]
    public void Construct(Config config)
    {
        _config = config;
    }
    private void Start()
    {
        cancellationTokenOnStopApplication = this.GetCancellationTokenOnDestroy();
    }

    public async UniTaskVoid StandartGunPointAnimation(float animSpeedMod, GunPoint gunPoint)
    {
        float t = 0;

        while (t < 1 && !cancellationTokenOnStopApplication.IsCancellationRequested)
        {
            t += Time.deltaTime * animSpeedMod;
            t = Mathf.Clamp01(t);
            float zOffset = _config.ForwardMovementAnimationCurve.Evaluate(t);
            gunPoint.transform.localPosition = gunPoint.DeffPos + Vector3.forward * zOffset;
            await UniTask.Yield();
        }
    }

    public async UniTaskVoid GunPointWarmingUpRotateAnimation(CancellationToken shootCT, float animSpeedMod, float startWarmingUpValue, GunPoint gunPoint, Vector3 rotateDirection)
    {
        float t = startWarmingUpValue;
        while (!shootCT.IsCancellationRequested && !cancellationTokenOnStopApplication.IsCancellationRequested)
        {
            if (t < _config.WarminTime) t += Time.deltaTime;

            t = Mathf.Clamp01(t);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(t) * animSpeedMod;
            gunPoint.transform.Rotate(rotateDirection, rotateSpeed, Space.Self);

            await UniTask.Yield();
        }
    }

    public async UniTaskVoid GunPointCoolingRotateAnimation(CancellationToken coolingCT, float animSpeedMod, float startWarmingUpValue, GunPoint gunPoint, Vector3 rotateDirection)
    {
        float t = startWarmingUpValue;
        while (!coolingCT.IsCancellationRequested && !cancellationTokenOnStopApplication.IsCancellationRequested)
        {
            t -= Time.deltaTime;
            t = Mathf.Clamp01(t);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(t) * animSpeedMod;
            gunPoint.transform.Rotate(rotateDirection, rotateSpeed, Space.Self);
            if (t <= 0)
            {
                break;
            }

            await UniTask.Yield();
        }
    }
}

