
using System;
using UnityEngine;
using PlaneC;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

namespace Components
{
    public class TerrainGenerator : MonoBehaviour
    {
        public int width;
        public int height;
        public int cellSize = 1;
        [Range(0, 1)] public float Flatlevel = 0.5f;
        public bool Inupdate;
        [Range(0, 1)] public float Zome = 0.5f;
        public Vector2 GroundOffSet = new Vector2(1, 1);
        public Gradient Gradient;
        public Vector2Int MinGRoundPos;
        public Vector2Int MaxGRoundPos;
        public PlayGrid playgrid;
        [Header("Rouds paramettres")] 
        public List<Vector2Int> roudsStartPos;
        [Range(0, 100)] public float ChanceDeDevier=25;
        public GameObject PrefabPont;
        public GameObject PrefabDebugSprite;
        public int LimiteBorders = 2;
        private List<Vector2Int> _roudCells = new List<Vector2Int>();

        private Mesh _mesh;
        private Vector3[] _vertices;
        private int[] _triangle;
        private Color[] colors;
        private float _depthMax;
        private float _depthMin;
       
        
        
        void Start()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        private void Update()
        {
            if (Inupdate)MakeNewMesh();
        }

        [ContextMenu("CreateMesh")]
        public void MakeNewMesh()
        {
            playgrid = new PlayGrid(width,height);
            GenerateMesh();
            UpdateMesh();
        }

        public void GenerateMesh()
        {
            _depthMax = Mathf.NegativeInfinity;
            _depthMin = Mathf.Infinity;
            _vertices = new Vector3[(height + 1) * (width + 1)];
            for (int i = 0, y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    float z;
                    if (x > MinGRoundPos.x & x < MaxGRoundPos.x && y > MinGRoundPos.y && y < MaxGRoundPos.y)
                    {
                        z = 0;
                    }
                    else
                    {
                        z = Mathf.PerlinNoise(GroundOffSet.x + x * Zome, GroundOffSet.y + y * Zome);
                        if (z < Flatlevel) z = 0;
                        else z = 2;
                    }


                    if (z > _depthMax) _depthMax = z;
                    if (z < _depthMin) _depthMin = z;


                    _vertices[i] = new Vector3(x * cellSize, y * cellSize, z);
                    i++;
                }
            }

            _triangle = new int[width * height * 6];
            int vert = 0;
            int tris = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _triangle[tris + 0] = vert + 0;
                    _triangle[tris + 1] = vert + width + 1;
                    _triangle[tris + 2] = vert + 1;
                    _triangle[tris + 3] = vert + 1;
                    _triangle[tris + 4] = vert + width + 1;
                    _triangle[tris + 5] = vert + width + 2;
                    vert++;
                    tris += 6;
                }

                vert++;
            }

            colors = new Color[_vertices.Length];
            for (int i = 0, y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float depth = Mathf.InverseLerp(_depthMin, _depthMax, _vertices[i].z);
                    colors[i] = Gradient.Evaluate(depth);
                    i++;
                }
            }
        }
        public void UpdateMesh()
        {
            _mesh.Clear();

            _mesh.vertices = _vertices;
            _mesh.triangles = _triangle;
            _mesh.colors = colors;
            _mesh.RecalculateNormals();
        }
        [ContextMenu("Generate playable Terrain")]
        public void GeneratePlaybleMap()
        {
            int vert = 0;
            int tris = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (_vertices[vert + 0].z ==0 && _vertices[vert + 1].z == 0 && _vertices[vert + width + 1].z == 0&& _vertices[vert + width + 2].z == 0) {
                        playgrid.GetCell(new Vector2Int(x, y)).IsNonWalkable = false;
                    }
                    else {
                        playgrid.GetCell(new Vector2Int(x, y)).IsNonWalkable = true;
                    }
                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }
        
        [ContextMenu("Generate Rouds")]
        public void GeneratRouds()
        {
            bool[,] RoadMap = new bool[width, height];
            foreach (Vector2Int startPo in roudsStartPos)
            {
                int x = startPo.x;
                int y = startPo.y;
                while (y < height-1) {
                    if (Random.Range(0, 100) < ChanceDeDevier) {
                        if (Random.Range(0, 100) < 50) {
                            x++;
                            if (x > width - LimiteBorders) x -= 2;
                        }
                        else {
                            x--;
                            if (x < LimiteBorders) x += 2;
                        }
                    }
                    else {
                        y++;
                    }


                    if (!playgrid.Cells[x, y].IsNonWalkable) {
                        playgrid.Cells[x, y].IsRoad = true;
                    }
                    else {
                        playgrid.Cells[x, y].IsRoad = false;
                        Instantiate(PrefabPont, new Vector3(x+0.5f, y+0.5f, 0.5f), Quaternion.identity,transform);
                    }
                    Instantiate(PrefabDebugSprite, new Vector3(x+0.5f, y+0.5f, 0f), Quaternion.identity,transform);
                    
                }
            }
        }
    }
}