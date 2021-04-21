using System;
using UnityEngine;
using PlaneC;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;

namespace Components
{
    public class GameManagerComponent:MonoBehaviour
    {
        [Header("Procedural Seed")] 
        public string Seed = " popote le boss";
        [Header ("Grid Info")]
        public PlayGrid PlayGrid;
        public int Width;
        public int Height;
        public bool SetDefaultGrid;
        public GameObject ConstructionTiles;

        [Header("FlowField Infos")] 
        public Vector2Int Target;
        private bool _IsReadyToCalculateFlowFlield;

        [Header("Home Building")] 
        public float HomeBuildingTimer;
        
        [Header("Linked Components")] 
        public PlayerManagerComponent PlayManagerComponent;
        public TerrainGenerator TerrainGenerator;
        public SmoothTerrain SmoothTerrain;

        private float _homeTimer;


        private void Start()
        {
            if (SetDefaultGrid)
            {
                PlayGrid = new PlayGrid(Width,Height);
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile = Instantiate(ConstructionTiles,
                            PlayGrid.GetCellCenterWorldPosByCell(new Vector2Int(x, y)), Quaternion.identity);
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile.SetActive(false);
                        PlayGrid.GetCell(new Vector2Int(x, y)).IsRoad = false;
                        PlayGrid.GetCell(new Vector2Int(x, y)).IsNonWalkable = false;
                    }
                }
            }
        }

        public void SetPlayGrid(PlayGrid playgrid, int width, int height)
        {
            Width = width;
            Height = height; 
            PlayGrid = playgrid;
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (!PlayGrid.GetCell(new Vector2Int(x, y)).IsNonWalkable) {
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile = Instantiate(ConstructionTiles,
                            PlayGrid.GetCellCenterWorldPosByCell(new Vector2Int(x, y)), Quaternion.identity, transform);
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile.SetActive(false);
                    }
                }
            }
            PlayManagerComponent.SetPlayGrid(playgrid);
        }

        [ContextMenu("Set Map")]
        public void SetTerrain()
        {
            System.Random random = new System.Random(Seed.ToUpper().GetHashCode());
            TerrainGenerator.GroundOffSet = new Vector2(random.Next(-10000,10000), random.Next(-10000,10000));
            TerrainGenerator.TreeOffSet= new Vector2(random.Next(-10000,10000), random.Next(-10000,10000));
            TerrainGenerator.TreeModifier1OffSet= new Vector2(random.Next(-10000,10000), random.Next(-10000,10000));
            foreach (RoadAutoCreator road in TerrainGenerator.Roads) road.Seed = random.Next();
            TerrainGenerator.SetMap();
            TerrainGenerator.SpawnBuilding();
            SmoothTerrain.height = TerrainGenerator.height;
            SmoothTerrain.width = SmoothTerrain.width;
            SmoothTerrain.InputMeshFilter = TerrainGenerator.GetComponent<MeshFilter>();
            TerrainGenerator.GetComponent<MeshRenderer>().enabled = false;
            SmoothTerrain.GenerateSmoothMesh();
            
        }
        [ContextMenu("GetHashCode")]
        public void GetHashCode()
        {
            Debug.Log(Seed.ToUpper().GetHashCode());
        }


        [ContextMenu("Calculate FlowField")]
        public void CalculateFlowField() {
            StartCoroutine(CalculateFlowFieldCorutine());
            _IsReadyToCalculateFlowFlield = false;
        }
        IEnumerator CalculateFlowFieldCorutine()
        {
            Debug.Log("Cacule le flowfield");
            List<Vector2Int> OpenList = new List<Vector2Int>();
            List<Vector2Int> temporalToAdd = new List<Vector2Int>();
            foreach (Cell cell in PlayGrid.Cells) {
                cell.MoveValue =int.MaxValue;
            }
            OpenList.Add(Target);
            PlayGrid.GetCell(Target).MoveValue=0;
            while (OpenList.Count > 0){
                foreach (Vector2Int cell in OpenList) {
                    foreach (Vector2Int neibors in PlayGrid.GetNeibors(cell)) {
                        if ((neibors-cell).magnitude > 1) {
                            if (PlayGrid.GetCell(neibors).MoveValue >PlayGrid.GetCell(cell).MoveValue + 14 + PlayGrid.GetCell(neibors).IndividualMoveValue&&!PlayGrid.CheckCellIsWall(new Vector2Int(cell.x,neibors.y))&&!PlayGrid.CheckCellIsWall(new Vector2Int(neibors.x,cell.y))) {
                                PlayGrid.GetCell(neibors).MoveValue =( PlayGrid.GetCell(cell).MoveValue + 14 +PlayGrid.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                PlayGrid.GetCell(neibors).MoveVector=(new Vector2(oriantation.x,oriantation.y));
                            } 
                        }
                        else {
                            if (PlayGrid.GetCell(neibors).MoveValue >PlayGrid.GetCell(cell).MoveValue + 10 + PlayGrid.GetCell(neibors).IndividualMoveValue) {
                                PlayGrid.GetCell(neibors).MoveValue =( PlayGrid.GetCell(cell).MoveValue + 10 + PlayGrid.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                PlayGrid.GetCell(neibors).MoveVector=(new Vector3(oriantation.x,oriantation.y));
                            } 
                        }
                    }
                    
                }
                OpenList.Clear();
                OpenList.AddRange(temporalToAdd);
                temporalToAdd.Clear();
                yield return new WaitForSeconds(0.01f);
            }
            _IsReadyToCalculateFlowFlield = true;
            Debug.Log("FlowField ClaculeTerminer");
            yield return null;
        }


        public void HomeTimer()
        {
            _homeTimer += Time.deltaTime;
            if (_homeTimer > HomeBuildingTimer)
            {
                _homeTimer = 0;
                buildHome();
            }
        }

        public void buildHome()
        {
            bool homeBuild =false;
            List<Cell> buildingCell = new List<Cell>();
            int mexSecurityFactor = Int32.MaxValue;
            int selectedValue=0;
            do
            {
                foreach (Cell cell in PlayGrid.Cells) {
                    if (cell.IsPlayble && cell.Batiment != null&&cell.SecurityValue < mexSecurityFactor) {
                        if (cell.SecurityValue > selectedValue) {
                            buildingCell.Clear();
                            selectedValue = cell.SecurityValue; 
                            buildingCell.Add(cell);
                        }
                        else if (cell.SecurityValue == selectedValue) {
                            buildingCell.Add(cell);
                        }
                    }
                }
                
            } while (homeBuild == false);
            
            

        }
    }
}