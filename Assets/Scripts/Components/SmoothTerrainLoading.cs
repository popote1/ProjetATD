using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SmoothTerrainLoading : MonoBehaviour
{
    public int width;
    public int height;
    public int Subdivise;
    public int cellSize = 1;
    public Gradient Gradient;
    public MeshFilter InputMeshFilter;
    public AnimationCurve Interpolationtest;
    public float LoadingProgress =0;

    public bool InUpdate;



    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangle;
    private Color[] colors;
    public float _depthMax;
    public float _depthMin;
    private GameObject[,] cells;

    void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    // Update is called once per frame
    void Update()
    {

        if (InUpdate)
        {
            GenerateSmoothMesh();
        }
    }




    [ContextMenu("GenerateSmoothMesh")]
    public void GenerateSmoothMesh()
    {
        StartCoroutine(GenerateMesh());
        
    }

    IEnumerator GenerateMesh()
    {

        _vertices = new Vector3[((height + 1) * Subdivise) * ((width + 1) * Subdivise)];
        for (int i = 0, y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
               
                Vector3 newVec = InputMeshFilter.mesh.vertices[i] + new Vector3(-0.2f, -0.2f);
                if (x == 0) newVec = new Vector3(0, newVec.y, newVec.z);
                if (y == 0) newVec = new Vector3(newVec.x, 0, newVec.z);
                _vertices[GetV0(x, y, width)] = new Vector3(newVec.x, newVec.y,
                    Mathf.Lerp(newVec.z, CalculatInterpolatedepth(i, 0), 0.4f));


                newVec = InputMeshFilter.mesh.vertices[i] + new Vector3(0.2f, -0.2f);
                if (x == width) newVec = new Vector3(width, newVec.y, newVec.z);
                if (y == 0) newVec = new Vector3(newVec.x, 0, newVec.z);
                _vertices[GetV1(x, y, width)] = new Vector3(newVec.x, newVec.y,
                    Mathf.Lerp(newVec.z, CalculatInterpolatedepth(i, 1), 0.4f));


                newVec = InputMeshFilter.mesh.vertices[i] + new Vector3(-0.2f, 0.2f);
                if (x == 0) newVec = new Vector3(0, newVec.y, newVec.z);
                if (y == height) newVec = new Vector3(newVec.x, height, newVec.z);
                _vertices[GetV2(x, y, width)] = new Vector3(newVec.x, newVec.y,
                    Mathf.Lerp(newVec.z, CalculatInterpolatedepth(i, 2), 0.4f));


                newVec = InputMeshFilter.mesh.vertices[i] + new Vector3(0.2f, 0.2f);
                if (x == width) newVec = new Vector3(width, newVec.y, newVec.z);
                if (y == height) newVec = new Vector3(newVec.x, height, newVec.z);
                _vertices[GetV3(x, y, width)] = new Vector3(newVec.x, newVec.y,
                    Mathf.Lerp(newVec.z, CalculatInterpolatedepth(i, 3), 0.4f));

                i++;

            }
            Debug.Log((float)y/height*100+"%");
            LoadingProgress = (float)y / height;
            yield return new WaitForSeconds(0.001f);
        }

        width = width * Subdivise + 1;
        height = height * Subdivise + 1;

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
                //Debug.Log(depth);
                colors[i] = Gradient.Evaluate(depth);
                i++;
            }
        }
        UpdateMesh();
    }

    public void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangle;
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
    }

    private float CalculatInterpolatedepth(int originalVetice, int subVertice)
    {
        int x, y, tempX, tempY = 0;
        tempX = x = originalVetice - ((width + 1) * (Mathf.FloorToInt((float) originalVetice / (width + 1))));
        tempY = y = Mathf.FloorToInt((float) originalVetice / (width + 1));
        switch (subVertice)
        {
            case 0:
                tempX--;
                tempY--;
                break;
            case 1:
                tempX++;
                tempY--;
                break;
            case 2:
                tempX--;
                tempY++;
                break;
            case 3:
                tempX++;
                tempY++;
                break;
        }

        tempX = Mathf.Clamp(tempX, 0, width);
        tempY = Mathf.Clamp(tempY, 0, height);

       // Debug.Log("traitement de la subVertice" + subVertice + " depui les coordoner" + x + "," + y + " vers " + tempX +
       //           "," + tempY);
        return (InputMeshFilter.mesh.vertices[originalVetice].z
                + InputMeshFilter.mesh.vertices[tempY * (width + 1) + x].z
                + InputMeshFilter.mesh.vertices[y * (width + 1) + tempX].z
                + InputMeshFilter.mesh.vertices[tempY * (width + 1) + tempX].z
            ) / 4;

    }


    public int GetX(int vertice, int width)
    {
        return vertice - width * Mathf.FloorToInt((float) vertice / width);
    }

    public int GetY(int vertice, int width)
    {
        return Mathf.FloorToInt((float) vertice / width);
    }

    private int GetV0(int x, int y, int width)
    {
        return y * (width + 1) * Subdivise * Subdivise + x * Subdivise;
    }

    private int GetV1(int x, int y, int width)
    {
        return GetV0(x, y, width) + 1;
    }

    private int GetV2(int x, int y, int width)
    {
        return y * (width + 1) * Subdivise * Subdivise + x * Subdivise + (width + 1) * Subdivise;
    }

    private int GetV3(int x, int y, int width)
    {
        return GetV2(x, y, width) + 1;
    }
}