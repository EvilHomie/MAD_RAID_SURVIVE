using Pathfinding;
using System.Collections;
using UnityEngine;

public class TESTAGENT : MonoBehaviour
{
    [SerializeField] int _posIndex;
    [SerializeField] DrawGameZones _drawGameZones;
    Vector3 _pos;
    IAstarAI _ai;

    float _t = 0;
    [SerializeField] float _tMax = 2;
    [SerializeField] float _radius;
    public void Start()
    {
        _ai = GetComponent<IAstarAI>();
        _pos = _drawGameZones.PointsPos[_posIndex];
        _ai.destination = _pos;
        StartCoroutine(ChangePos());
    }

    private void Update()
    {
        if (_ai.reachedEndOfPath)
        {

        }
        else
        {

        }
    }

    IEnumerator ChangePos()
    {
        while (true) 
        {
            yield return new WaitForSeconds(Random.Range(0, _tMax));

            Vector3 newOffset = Random.insideUnitSphere * _radius;
            newOffset.y = 0;
            Vector3 newPos = _pos + newOffset;
            _ai.destination = newPos;
        }
    }
}
