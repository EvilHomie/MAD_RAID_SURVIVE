using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawGameZones : MonoBehaviour
{
    [SerializeField] Config _config;
    [SerializeField] AreaMeshFilterData _meshFilterEnemyFightArea;
    [SerializeField] AreaMeshFilterData _meshFilterBonusEnemyArea;
    [SerializeField] AreaMeshFilterData _meshFilterEnvironmentsArea;

    [SerializeField] AreaMeshFilterData _meshFilterSpawnEnemiesArea_Left;
    [SerializeField] AreaMeshFilterData _meshFilterSpawnEnemiesArea_Right;


    [SerializeField] int _enemiesPointsLineCount = 3;
    [SerializeField] int _enemiesPointsColumnCount = 3;
    [SerializeField] float _pointsRadius = 2;


    [SerializeField] AreaZone _enemyFightArea;
    [SerializeField] AreaZone _bonusEnemyArea;
    [SerializeField] AreaZone _environmentsArea;
    [SerializeField] AreaZone _spawnEnemiesArea_LeftArea;
    [SerializeField] AreaZone _spawnEnemiesArea_RightArea;

    List<Vector3> _pointsPos = new();
    Color _pointsColor = Color.yellow;

    //[Inject]
    //public void Construct(Config config)
    //{
    //    _config = config;
    //}

    private void OnEnable()
    {

    }

    void Update()
    {
        _environmentsArea = new(DrawAreaZone(_meshFilterEnvironmentsArea.corner00.position.x, _meshFilterEnvironmentsArea.corner11.position.x, _meshFilterEnvironmentsArea.corner00.position.z, _meshFilterEnvironmentsArea.corner11.position.z, _meshFilterEnvironmentsArea.areaMeshFilter));
        _enemyFightArea = new(DrawAreaZone(_meshFilterEnemyFightArea.corner00.position.x, _meshFilterEnemyFightArea.corner11.position.x, _meshFilterEnemyFightArea.corner00.position.z, _meshFilterEnemyFightArea.corner11.position.z, _meshFilterEnemyFightArea.areaMeshFilter));
        _bonusEnemyArea = new(DrawAreaZone(_meshFilterBonusEnemyArea.corner00.position.x, _meshFilterBonusEnemyArea.corner11.position.x, _meshFilterBonusEnemyArea.corner00.position.z, _meshFilterBonusEnemyArea.corner11.position.z, _meshFilterBonusEnemyArea.areaMeshFilter));

        _spawnEnemiesArea_LeftArea = new(DrawAreaZone(_meshFilterSpawnEnemiesArea_Left.corner00.position.x, _meshFilterSpawnEnemiesArea_Left.corner11.position.x, _meshFilterSpawnEnemiesArea_Left.corner00.position.z, _meshFilterSpawnEnemiesArea_Left.corner11.position.z, _meshFilterSpawnEnemiesArea_Left.areaMeshFilter));
        _spawnEnemiesArea_RightArea = new(DrawAreaZone(_meshFilterSpawnEnemiesArea_Right.corner00.position.x, _meshFilterSpawnEnemiesArea_Right.corner11.position.x, _meshFilterSpawnEnemiesArea_Right.corner00.position.z, _meshFilterSpawnEnemiesArea_Right.corner11.position.z, _meshFilterSpawnEnemiesArea_Right.areaMeshFilter));
        DrawPoints(_enemiesPointsLineCount, _enemiesPointsColumnCount, _meshFilterEnemyFightArea.corner00, _meshFilterEnemyFightArea.corner11);


        _config.EnvironmentsAreaZone = _environmentsArea;
    }

    Vector4 DrawAreaZone(float XMinPos, float XMaxPos, float ZMinPos, float ZMaxPos, MeshFilter targetMF)
    {
        Mesh mesh = new();

        Vector3[] verticles = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];


        verticles[0] = new Vector3(XMinPos, 0, ZMinPos);
        verticles[1] = new Vector3(XMinPos, 0, ZMaxPos);
        verticles[2] = new Vector3(XMaxPos, 0, ZMaxPos);
        verticles[3] = new Vector3(XMaxPos, 0, ZMinPos);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);


        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = verticles;
        mesh.uv = uv;
        mesh.triangles = triangles;

        targetMF.mesh = mesh;
        return new Vector4(XMinPos, XMaxPos, ZMinPos, ZMaxPos);
    }

    void DrawPoints(int lineCount, int columnCount, Transform corner00, Transform corner11)
    {
        float totalZoneWidth = corner11.position.x - corner00.position.x;
        float totalZoneDepth = corner11.position.z - corner00.position.z;

        Vector2 zoneSize = new(totalZoneWidth / columnCount, totalZoneDepth / lineCount);

        List<AreaBordersPos> linesBordersPositions = new();
        List<AreaBordersPos> columnBordersPositions = new();

        for (int i = 0; i < columnCount; i++)
        {
            float startPos = i * zoneSize.x + corner00.position.x;
            float endPos = startPos + zoneSize.x;
            columnBordersPositions.Add(new(startPos, endPos));
        }

        for (int i = 0; i < lineCount; i++)
        {
            float startPos = i * zoneSize.y + corner00.position.z;
            float endPos = startPos + zoneSize.y;
            linesBordersPositions.Add(new(startPos, endPos));
        }
        _pointsPos.Clear();

        for (int i = 0; i < lineCount; i++)
        {

            for (int j = 0; j < columnCount; j++)
            {
                Gizmos.color = Color.yellow;

                Vector3 pos = new(columnBordersPositions[j].Middle, 0, linesBordersPositions[i].Middle);
                _pointsPos.Add(pos);
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach (var pos in _pointsPos)
        {
            Gizmos.color = _pointsColor;
            Gizmos.DrawWireSphere(pos, _pointsRadius);
        }
    }
}

[Serializable]
public struct AreaMeshFilterData
{
    public MeshFilter areaMeshFilter;
    public Transform corner00;
    public Transform corner11;
}

struct AreaBordersPos
{
    public float StartPos;
    public float EndPos;

    public float Middle => (StartPos + EndPos) / 2;

    public AreaBordersPos(float startPos, float endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}