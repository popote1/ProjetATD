using System;
using UnityEngine;
using PlaneC;
using System.Collections.Generic;
using System.Collections;
namespace Components
{
    public class GameManagerComponent:MonoBehaviour
    {
        [Header ("Grid Info")]
        public PlayGrid PlayGrid;
        public int Width;
        public int Height;
        public bool SetDefaultGrid;
        public GameObject ConstructionTiles;

        [Header("FlowField Infos")] 
        public Vector2Int Target;
        private bool _IsReadyToCalculateFlowFlield;
        
        [Header("Linked Components")] 
        public PlayerManagerComponent PlayManagerComponent;


        private void Start()
        {
            if (SetDefaultGrid)
            {
                PlayGrid = new PlayGrid(Width,Height);
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        Debug.Log(" travail sur la case"+PlayGrid.Cells[x,y].Position);
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile = Instantiate(ConstructionTiles,
                            PlayGrid.GetCellCenterWorldPosByCell(new Vector2Int(x, y)), Quaternion.identity);
                        PlayGrid.GetCell(new Vector2Int(x, y)).ConstructionTile.SetActive(false);
                        PlayGrid.GetCell(new Vector2Int(x, y)).IsRoad = false;
                        PlayGrid.GetCell(new Vector2Int(x, y)).IsNonWalkable = false;
                    }
                }
            }
        }

        private void Update()
        {
           
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
            yield return null;
        }
    }
}