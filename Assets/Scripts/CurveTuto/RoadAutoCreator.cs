using System.Collections;
using System.Collections.Generic;
using Components;
using CurveTuto;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class RoadAutoCreator : MonoBehaviour
{
    public PathCreator PathCreator;
    public TerrainGenerator TerrainGenerator;
    [Header("Road new Points")]
    public Vector2 NewCoordonate = Vector2.zero;
    [Header("Road Points")]
    public List<Vector2>RoadPos;
    
    [Header("Cell Analizer parameters")]
    public float Spacing = 0.1f;
    public float Resolution = 1f;
    public float roadWight = 0.5f;

    [Header("Procedural Road Parameters")] 
    public int TargetTopTerrain;
    public int MaxSegmant;
    public Vector2Int StartRoadPos;
    public Vector2Int MinOffset;
    public Vector2Int MaxOffset;
    public int Seed;
    
    private Path _path;
    private List<GameObject> _debuganalizer = new List<GameObject>();
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu(("MakeRoade"))]
    public void MakeRoad()
    {
        PathCreator.CreatePath();
        _path = PathCreator.Path;
        _path.AutoSetControlPoints = true;

        for (int i = 0; i < RoadPos.Count; i++) {
            if (i<2)_path.MovePoints(i*3 ,RoadPos[i]); 
            else _path.AddSegment(RoadPos[i]);
        }
    }
    [ContextMenu("place CallAnalizer")]
    public void placeCellAnalizer()
    {
        foreach (GameObject gameObject in _debuganalizer)Destroy(gameObject ,0.1f);
        _debuganalizer.Clear();
        Vector2[] points = FindObjectOfType<PathCreator>().Path.CalculateEvenlySpacePoints(Spacing, Resolution);

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < points.Length - 1|| _path.IsClosed) forward += points[(i + 1)%points.Length] - points[i];
            if (i > 0|| _path.IsClosed) forward += points[i] - points[(i - 1+points.Length)%points.Length];
            forward.Normalize();
            
            Vector2 left = new Vector2( -forward.y , forward.x);
            
            GameObject m = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m.transform.position = points[i] ;
            m.transform.localScale = Vector3.one * Spacing * 0.5f;
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = points[i] + left * roadWight * 0.5f;
            g.transform.localScale = Vector3.one * Spacing * 0.5f;
            GameObject d = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            d.transform.position = points[i] - left * roadWight * 0.5f;
            d.transform.localScale = Vector3.one * Spacing * 0.5f;
           
            _debuganalizer.Add(m);
            _debuganalizer.Add(g);
            _debuganalizer.Add(d);
        }
        
    }
    [ContextMenu("Add New Segment")]
    public void AddNewSegmanet()
    {
        _path.AddSegment(NewCoordonate);
        RoadPos.Add(NewCoordonate);
    }
    [ContextMenu("Generate Procedural Road")]
    public void GeneratProceduralRoad()
    {
        PathCreator.CreatePath();
        _path = PathCreator.Path;
        _path.AutoSetControlPoints = false;
        
        _path.MovePoints(0 ,StartRoadPos);
        _path.MovePoints(1 , StartRoadPos+Vector2Int.up*2);
        System.Random random = new System.Random(Seed);
        System.Random subRandom = new System.Random(random.Next());
        Vector2 newPos =_path[0] +new Vector2Int(random.Next(MinOffset.x, MaxOffset.x), random.Next(MinOffset.y, MaxOffset.y));
        _path.MovePoints(3, newPos);
        _path.MovePoints(2 , newPos-Vector2Int.up*-2);
        

        int i=0;
        while (i<MaxSegmant&&_path[_path.NumPoints-1].y<TargetTopTerrain)
        {
            Vector2Int addedValue = new Vector2Int(random.Next(MinOffset.x, MaxOffset.x), random.Next(MinOffset.y, MaxOffset.y));
            newPos = _path[_path.NumPoints-1]+addedValue;
            if (newPos.x < TerrainGenerator.LimiteBorders) newPos = _path[_path.NumPoints - 1]+ new Vector2(addedValue.x*-1,addedValue.y);
            if (newPos.x > TerrainGenerator.width-TerrainGenerator.LimiteBorders)newPos = _path[_path.NumPoints - 1]+ new Vector2(addedValue.x*-1,addedValue.y);
            
            _path.AddSegment(newPos);
            _path.MovePoints(_path.NumPoints-2, newPos-Vector2.up*2);
            _path.MovePoints(_path.NumPoints - 3,_path[_path.NumPoints - 4]-Vector2Int.up*-2);
            i++;
        }
    }

    [ContextMenu("place CallAnalizerOnLastSegment")]
    public bool placeCellAnalizerOnLastSegment()
    {
        foreach (GameObject gameObject in _debuganalizer)Destroy(gameObject ,0.1f);
        _debuganalizer.Clear();
        Vector2[] points = FindObjectOfType<PathCreator>().Path.CalculateEvenlySpacePointsOnSegment(Spacing,_path.NumSegments-1, Resolution);

        bool isInWater = false;
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < points.Length - 1|| _path.IsClosed) forward += points[(i + 1)%points.Length] - points[i];
            if (i > 0|| _path.IsClosed) forward += points[i] - points[(i - 1+points.Length)%points.Length];
            forward.Normalize();
            
            Vector2 left = new Vector2( -forward.y , forward.x);
            
            GameObject m = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m.transform.position = points[i] ;
            m.transform.localScale = Vector3.one * Spacing * 0.5f;
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = points[i] + left * roadWight * 0.5f;
            g.transform.localScale = Vector3.one * Spacing * 0.5f;
            GameObject d = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            d.transform.position = points[i] - left * roadWight * 0.5f;
            d.transform.localScale = Vector3.one * Spacing * 0.5f;
           
            _debuganalizer.Add(m);
            _debuganalizer.Add(g);
            _debuganalizer.Add(d);
            foreach (GameObject go in _debuganalizer)
            {
                if (TerrainGenerator.playgrid
                    .GetCell(TerrainGenerator.playgrid.GetCellGridPosByWorld(go.transform.position)).IsNonWalkable)
                {
                    go.GetComponent<MeshRenderer>().material.color  =Color.red;
                }
                else
                {
                    go.GetComponent<MeshRenderer>().material.color  =Color.blue;
                }
            }
            
        }
        for (int i = 0; i<_debuganalizer.Count ;i+=3)
        {
            if (TerrainGenerator.playgrid
                .GetCell(TerrainGenerator.playgrid.GetCellGridPosByWorld(_debuganalizer[i].transform.position))
                .IsNonWalkable)
            {
                _path.MovePoints(_path.NumPoints-1,new Vector2(Mathf.RoundToInt(_debuganalizer[i].transform.position.x),Mathf.FloorToInt(_debuganalizer[i].transform.position.y)));
                _path.MovePoints(_path.NumPoints-2, _path[_path.NumPoints-1]-Vector2.up*2);
                isInWater = true;
                break;
            }
            if (TerrainGenerator.playgrid
                .GetCell(TerrainGenerator.playgrid.GetCellGridPosByWorld(_debuganalizer[i+1].transform.position))
                .IsNonWalkable)
            {
                _path.MovePoints(_path.NumPoints-1,new Vector2(Mathf.FloorToInt(_debuganalizer[i+1].transform.position.x)+1,Mathf.FloorToInt(_debuganalizer[i+1].transform.position.y)));
                _path.MovePoints(_path.NumPoints-2, _path[_path.NumPoints-1]-Vector2.up*2);
                isInWater = true;
                break;
                
            }
            if (TerrainGenerator.playgrid
                .GetCell(TerrainGenerator.playgrid.GetCellGridPosByWorld(_debuganalizer[i+2].transform.position))
                .IsNonWalkable)
            {
                _path.MovePoints(_path.NumPoints-1,new Vector2(Mathf.FloorToInt(_debuganalizer[i+2].transform.position.x),Mathf.FloorToInt(_debuganalizer[i+2].transform.position.y)));
                _path.MovePoints(_path.NumPoints-2, _path[_path.NumPoints-1]-Vector2.up*2);
                isInWater = true;
                break;
            }
        }
        return isInWater;
    }
    
}
