using System.Collections.Generic;
using UnityEngine;
using PlaneC;
using Bourg;
using Bourg.Achetable;
using Unity.Mathematics;

namespace Components
{
    public class PlayerManagerComponent : MonoBehaviour
    {
        public GameManagerComponent GameManagerComponent;

        public static int Gold;
        public static List<Batiment> Batiments;

        public InputStat InputState;
        [Header("Building")] 
        public int BuildIndex;
        public List<Achetables> AchetablesList;


        [Header("Cursor Configue")] public GameObject PrefabCursor;
        [Range(0, 100)] public float CursorSmoothFactor;
        public Camera Camera;


        private PlayGrid _playGrid;
        private GameObject _cursor;
        private Vector3 _cursorTaget;
        private Vector2Int _selsectdCell;
        private bool _press;
        
        public enum InputStat
        {
            none, Building 
        }

        private void Start()
        {
            
            _cursor = Instantiate(PrefabCursor, Vector3.zero, quaternion.identity);
        }

        private void Update()
        {
            if (InputState == InputStat.Building) Building();
            
                
            
            
            
            if(_playGrid== null)_playGrid = GameManagerComponent.PlayGrid;
            RaycastHit hit;
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (_playGrid.CheckIfInGrid(_playGrid.GetCellGridPosByWorld(hit.point))) {
                    _selsectdCell = _playGrid.GetCellGridPosByWorld(hit.point);
                    _cursorTaget = _playGrid.GetCellCenterWorldPosByCell(_selsectdCell)+new Vector3(0,0,-0.5f);
                }
                if (_cursor != null)
                    _cursor.transform.position = Vector3.Lerp(_cursor.transform.position, _cursorTaget,
                        Time.deltaTime * CursorSmoothFactor);
            }
        }

        [ContextMenu("Build")]

        public void SelecteBuilding()
        {
            InputState = InputStat.Building;
        }

        private void Building()
        {
            
        }
        
        private List<Vector2Int> GetBuildingVec2by2(Vector2Int origin)
        {
            List<Vector2Int> cells = new List<Vector2Int>();
            Vector2Int cell = origin;
            if (_playGrid.CheckIfInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.left;
            if (_playGrid.CheckIfInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.up;
            if (_playGrid.CheckIfInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            cell += Vector2Int.right;
            if (_playGrid.CheckIfInGrid(cell)) if(OperateBuildingCell(cell))cells.Add(cell);
            return cells;
        }
        private bool OperateBuildingCell(Vector2Int cell) {
            if (_playGrid.GetCell(cell).BuildingCell != null && Grid.GetCell(cell).IndividualMoveValue <= 20) {
                if (!_preselectedCell.Contains(cell)) {
                    _preselectedCell.Add(cell);
                    _playGrid.GetCell(cell).BuildingCell.SetActive(true);
                }
                return true;
            }
            return false;
        }

        
        
    }
}