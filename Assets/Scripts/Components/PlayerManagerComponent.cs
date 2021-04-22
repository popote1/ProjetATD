﻿using System.Collections.Generic;
using UnityEngine;
using PlaneC;
using Assets.Scripts.Bourg.Achetable;
using Assets.Scripts.Bourg;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Serialization;

namespace Components
{
    public class PlayerManagerComponent : MonoBehaviour
    {
        public GameManagerComponent GameManagerComponent;

        public static int Gold;
        public static List<Batiment> Batiments = new List<Batiment>();

        public InputStat InputState;
        public List<Achetables> SelectedBuildings = new List<Achetables>();
        [Header("Camera Seting")] 
        public float2 CameraYRange;
        public Camera Camera;
        public float CameraSencibility = 1;
        

        [Header("Ennemis ")] 
        //public EnemyComponent EnnemieComponent;
        [Header("Building")] 
        public int BuildIndex;
        public List<Achetables> AchetablesList;


        [Header("Cursor Configue")] public GameObject PrefabCursor;
        [Range(0, 100)] public float CursorSmoothFactor;
        [Header("UI")] 
        public bool CursorOnUI;
        public Scrollbar ScrollbarCamera;
        public Button BPDestroy;
        public TMP_Text TxtGoldText;
        public CanvasGroup PanelFondInsufisant;
        


        private PlayGrid _playGrid;
        private GameObject _cursor;
        private Vector3 _cursorTaget;
        private Vector2Int _selsectdCell;
        private bool _press;
        private List<Vector2Int> _preselectedCell = new List<Vector2Int>();
        private Vector3 _lastCursorPos;
        
        public enum InputStat
        {
            none, Building , AddEnnemis, Selecting
        }

        private void Start()
        {
            
            _cursor = Instantiate(PrefabCursor, Vector3.zero, quaternion.identity);
        }

        private void Update()
        {
            Debug.Log("Gold = "+Gold);
            if (!CursorOnUI)
            {
                if (InputState == InputStat.Building) Building();
                if (InputState == InputStat.AddEnnemis) AddEnnemis();
                if (InputState == InputStat.none) DragCamera();
            }

            if (_playGrid != null)
            {
                //_playGrid = GameManagerComponent.PlayGrid;
                RaycastHit hit;
                if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (_playGrid.CheckIfInGrid(_playGrid.GetCellGridPosByWorld(hit.point)))
                    {
                        _selsectdCell = _playGrid.GetCellGridPosByWorld(hit.point);
                        _cursorTaget = _playGrid.GetCellCenterWorldPosByCell(_selsectdCell) + new Vector3(0, 0, -0.5f);
                    }
                    if (_cursor != null)
                        _cursor.transform.position = Vector3.Lerp(_cursor.transform.position, _cursorTaget, 
                            Time.deltaTime * CursorSmoothFactor);
                }
            }
            TxtGoldText.text = "Gold : " + Gold;
            PanelFondInsufisant.alpha = Mathf.Lerp( PanelFondInsufisant.alpha,0f, 0.01f);
            
            if (SelectedBuildings.Count==1) BPDestroy.gameObject.SetActive(true);
            else BPDestroy.gameObject.SetActive(false);
        }

        public void SetOnCursorBool(bool value)
        {
            CursorOnUI = value;
        }

        public void SetPlayGrid(PlayGrid playGrid)
        {
            _playGrid = playGrid;
        }

        [ContextMenu("Build")]
        public void SelecteBuilding()
        {
            InputState = InputStat.Building;
        }

        private void AddEnnemis()
        {
           /* if (Input.GetButton("Fire1"))
            {
                EnemyComponent en =(EnnemieComponent, _cursor, Quaternion.identity);
                en.PlayGrid = _playGrid;
            }*/
        }

        public void ScrollCamera(Scrollbar ctx)
        {
            Camera.transform.position = new Vector3(Camera.transform.position.x,
                Mathf.Lerp(CameraYRange.x, CameraYRange.y, ctx.value), Camera.transform.position.z);
        }

        private void DragCamera()
        {
            float yMove;
            if (Input.GetButton("Fire1")) {
                if (_lastCursorPos != Vector3.zero) {
                    yMove = _lastCursorPos.y - Input.mousePosition.y;
                    Camera.transform.position = new Vector3( Camera.transform.position.x
                        ,Mathf.Clamp(Camera.transform.position.y+yMove * CameraSencibility,CameraYRange.x,CameraYRange.y)
                        , Camera.transform.position.z);
                    ScrollbarCamera.value =
                        Mathf.InverseLerp(CameraYRange.x, CameraYRange.y, Camera.transform.position.y);
                }
                _lastCursorPos = Input.mousePosition;
            }
            else
            {
                _lastCursorPos = Vector3.zero;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (_playGrid.GetCell(_selsectdCell).Batiment != null)
                {
                    if (_playGrid.GetCell(_selsectdCell).Batiment is Achetables)
                    {
                        foreach (Achetables building in SelectedBuildings)building.OnDeselect();
                        SelectedBuildings.Clear();
                        Achetables bat = (Achetables)_playGrid.GetCell(_selsectdCell).Batiment;
                        SelectedBuildings.Add(bat);
                        bat.OnSelect();
                        
                    }
                }
            }
        }

        public void StartBuilding(int buildingIndex)
        {
            BuildIndex = buildingIndex;
            if (Gold > AchetablesList[buildingIndex].Prix) InputState = InputStat.Building;
            else PanelFondInsufisant.alpha = 1;

        }

        public void DestroySelectedBuilding()
        {
            Destroy(SelectedBuildings[0].gameObject);
            SelectedBuildings.Clear();
            CursorOnUI = false;
        }

        private void Building()
        {
            if (Input.GetButton("Fire1"))BuildingShowTiles();
            if (Input.GetButtonUp("Fire1")) BuildPutNewBuilding();
        }

        private void BuildingShowTiles()
        {
            Debug.Log("Batiment avec " + AchetablesList[BuildIndex].CellNeeded + " case");

            List<Vector2Int> newCells = new List<Vector2Int>();
            switch (AchetablesList[BuildIndex].CellNeeded)
            {
                case 1:
                    newCells =GetBuildingVec1by1(_selsectdCell);
                    Debug.Log("Batiment avec 1 case");
                    break;
                case 2:
                    newCells =GetBuildingVec2by2(_selsectdCell);
                    Debug.Log("Batiment avec 2 case");
                    break;
                case 3:
                    newCells =GetBuildingVec3by3(_selsectdCell);
                    Debug.Log("Batiment avec 3 case");
                    break;
            }

                
            foreach (Vector2Int cell in _preselectedCell) 
                if (!newCells.Contains(cell)) 
                    _playGrid.GetCell(cell).ConstructionTile.SetActive(false);
            _preselectedCell.Clear();
            foreach (Vector2Int cell in newCells)
            {
                _preselectedCell.Add(cell);
                if (_playGrid.GetCell(cell).Batiment == null)
                    _playGrid.GetCell(cell).ConstructionTile.SetActive(true);
            }
        }

        private void BuildPutNewBuilding()
        {
            int buildingCellReady=0;
            Vector3 buildingPos = Vector3.zero;
            foreach (Vector2Int cell in _preselectedCell) {
                if (_playGrid.GetCell(cell).Batiment == null) {
                    buildingCellReady++;
                    buildingPos += _playGrid.GetCellCenterWorldPosByCell(cell);
                }
            }
            buildingPos = buildingPos / _preselectedCell.Count;
            if (buildingCellReady == AchetablesList[BuildIndex].CellNeeded * AchetablesList[BuildIndex].CellNeeded) {
                Achetables achetable=Instantiate(AchetablesList[BuildIndex], buildingPos, quaternion.identity);
                foreach (Vector2Int cell in _preselectedCell) {
                    _playGrid.GetCell(cell).Batiment = achetable;
                    _playGrid.GetCell(cell).IndividualMoveValue = achetable.IndividualMoveFactor;
                }
                achetable.OccupiedCells = _preselectedCell;
                achetable.Position = buildingPos;
                achetable.Playgrid = _playGrid;
                Gold -= achetable.Prix;
                if (achetable.SecurityValue != 0) {
                    foreach (Vector2Int cell in _playGrid.GetBuildingAura(_selsectdCell, achetable.CellNeeded, achetable.SecurityRange)) {
                        _playGrid.GetCell(cell).SecurityValue += achetable.SecurityValue;
                    }
                }
            }
            foreach (Vector2Int cell in _preselectedCell)
            {
                _playGrid.GetCell(cell).ConstructionTile.SetActive(false);
            }
            
            

            InputState = InputStat.none;
        }
       

        private List<Vector2Int> GetBuildingVec1by1(Vector2Int origin)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            Vector2Int cell = origin;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            return cells;
        }
        
        private List<Vector2Int> GetBuildingVec2by2(Vector2Int origin)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            Vector2Int cell = origin;
            if (_playGrid.CheckIfInGrid(cell)) if (_playGrid.GetCell(cell).ConstructionTile) cells.Add(cell);
            cell += Vector2Int.left;
            if (_playGrid.CheckIfInGrid(cell)) if (_playGrid.GetCell(cell).ConstructionTile)cells.Add(cell);
            cell += Vector2Int.up;
            if (_playGrid.CheckIfInGrid(cell)) if (_playGrid.GetCell(cell).ConstructionTile)cells.Add(cell);
            cell += Vector2Int.right;
            if (_playGrid.CheckIfInGrid(cell)) if (_playGrid.GetCell(cell).ConstructionTile)cells.Add(cell);
            return cells;
        }

        private List<Vector2Int> GetBuildingVec3by3(Vector2Int origin)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            Vector2Int cell = origin;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.left;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.up;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.right;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.right;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.down;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.down;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.left;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            cell += Vector2Int.left;
            if (_playGrid.CheckIfInGrid(cell)) cells.Add(cell);
            return cells;
        }
       /* public bool BuildPutNewBuilding(Vector2Int pos , Batiment bat)
        {
            List<Vector2Int> neigbors = new List<Vector2Int>();
            Vector3 newPos = Vector3.zero;
            if (bat.CellNeeded <= 1) neigbors = GetBuildingVec1by1(pos);
            else if (bat.CellNeeded == 2) neigbors = GetBuildingVec2by2(pos);
            else if (bat.CellNeeded == 3) neigbors = GetBuildingVec3by3(pos);
            else return false;
            foreach (Vector2Int vec in neigbors)
            {
                if (_playGrid.GetCell(vec).Batiment != null) return false;
                else newPos += _playGrid.GetCellCenterWorldPosByCell(vec);
            }
            newPos = newPos / neigbors.Count;
            Batiment batiment = Instantiate(bat, newPos, Quaternion.identity);
            batiment.OccupiedCells = neigbors;
            batiment.Playgrid = _playGrid;
            if (batiment.SecurityValue != 0) {
                foreach (Vector2Int cell in _playGrid.GetBuildingAura(pos, bat.CellNeeded, bat.SecurityRange)) {
                    _playGrid.GetCell(cell).SecurityValue += bat.SecurityValue;
                }
            }
            Batiments.Add(batiment);
            foreach (Vector2Int vec in neigbors) _playGrid.GetCell(vec).Batiment = batiment;
            return true;
        }*/
       public bool BuildPutNewBuilding(Vector2Int pos, Batiment bat, bool isHome = false)
       {
           List<Vector2Int> neigbors = new List<Vector2Int>();
           Vector3 newPos = Vector3.zero;
           if (bat.CellNeeded <= 1) neigbors = GetBuildingVec1by1(pos);
           else if (bat.CellNeeded == 2) neigbors = GetBuildingVec2by2(pos);
           else if (bat.CellNeeded == 3) neigbors = GetBuildingVec3by3(pos);
           else return false;
           if (neigbors.Count!=bat.CellNeeded*bat.CellNeeded)return false;
           foreach (Vector2Int vec in neigbors) {
               if (_playGrid.GetCell(vec).Batiment != null) {
                   return false;
               }
               else {
                   if (isHome&&_playGrid.GetCell(vec).IsRoad)return false;
                   newPos += _playGrid.GetCellCenterWorldPosByCell(vec);
               }
           }
           newPos = newPos / neigbors.Count;
           Batiment batiment = Instantiate(bat, newPos, Quaternion.identity);
           batiment.OccupiedCells = neigbors;
           batiment.Playgrid = _playGrid;
           if (batiment.SecurityValue != 0) {
               foreach (Vector2Int cell in _playGrid.GetBuildingAura(pos, bat.CellNeeded, bat.SecurityRange)) {
                   _playGrid.GetCell(cell).SecurityValue += bat.SecurityValue;
               }
           }
           Batiments.Add(batiment);
           foreach (Vector2Int vec in neigbors) _playGrid.GetCell(vec).Batiment = batiment;
           return true;
       }
    }
    
    
}