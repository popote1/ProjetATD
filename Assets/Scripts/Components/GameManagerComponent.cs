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
        public PlayGrig PlayGrig;
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
                PlayGrig = new PlayGrig(Width,Height);
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        Debug.Log(" travail sur la case"+PlayGrig.Cells[x,y].Position);
                        PlayGrig.GetCell(new Vector2Int(x, y)).ConstructionTile = Instantiate(ConstructionTiles,
                            PlayGrig.GetCellCenterWorldPosByCell(new Vector2Int(x, y)), Quaternion.identity);
                        PlayGrig.GetCell(new Vector2Int(x, y)).ConstructionTile.SetActive(false);
                        PlayGrig.GetCell(new Vector2Int(x, y)).IsRoad = false;
                        PlayGrig.GetCell(new Vector2Int(x, y)).IsNonWalkable = false;
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
            foreach (Cell cell in PlayGrig.Cells) {
                cell.MoveValue =int.MaxValue;
            }
            OpenList.Add(Target);
            PlayGrig.GetCell(Target).MoveValue=0;
            while (OpenList.Count > 0){
                foreach (Vector2Int cell in OpenList) {
                    foreach (Vector2Int neibors in PlayGrig.GetNeibors(cell)) {
                        if ((neibors-cell).magnitude > 1) {
                            if (PlayGrig.GetCell(neibors).MoveValue >PlayGrig.GetCell(cell).MoveValue + 14 + PlayGrig.GetCell(neibors).IndividualMoveValue&&!PlayGrig.CheckCellIsWall(new Vector2Int(cell.x,neibors.y))&&!PlayGrig.CheckCellIsWall(new Vector2Int(neibors.x,cell.y))) {
                                PlayGrig.GetCell(neibors).MoveValue =( PlayGrig.GetCell(cell).MoveValue + 14 +PlayGrig.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                PlayGrig.GetCell(neibors).MoveVector=(new Vector2(oriantation.x,oriantation.y));
                            } 
                        }
                        else {
                            if (PlayGrig.GetCell(neibors).MoveValue >PlayGrig.GetCell(cell).MoveValue + 10 + PlayGrig.GetCell(neibors).IndividualMoveValue) {
                                PlayGrig.GetCell(neibors).MoveValue =( PlayGrig.GetCell(cell).MoveValue + 10 + PlayGrig.GetCell(neibors).IndividualMoveValue);
                                temporalToAdd.Add(neibors);
                                Vector2Int oriantation = cell - neibors;
                                PlayGrig.GetCell(neibors).MoveVector=(new Vector3(oriantation.x,oriantation.y));
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