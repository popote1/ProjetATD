using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Unity.Mathematics;

namespace PlaneC
{
    public class PlayGrid
    {
        public Cell[,] Cells;
        public int Width;
        public int Height;

        public PlayGrid(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width,height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Cells[x,y] = new Cell(new Vector2Int(x,y),this);
                }
            }
        }

        /// <summary>
        /// Return true if the given Grid Coordinate is in the grid.
        /// </summary>
        /// <param name="Grid Coorditane"></param>
        /// <returns>bool</returns>
        public bool CheckIfInGrid(Vector2Int pos) {
            if (pos.x >=0 && pos.x < Width && pos.y >=0 && pos.y < Height) return true;
            return false;
        }
        /// <summary>
        /// Return the cell at the given coordinate, if the coordinate isn't in the grid return null.
        /// </summary>
        /// <param name="Grid Coorditane"></param>
        /// <returns>Cell</returns>
        public Cell GetCell(Vector2Int pos) {
            if (CheckIfInGrid(pos)) return Cells[pos.x, pos.y];
            return null;
        }
        /// <summary>
        /// Return the cell at the given coordinate, if the coordinate isn't in the grid return null.
        /// </summary>
        /// <param name="Grid Coorditane"></param>
        /// <returns>Cell</returns>
        public Cell GetCell(int x, int y) {
            if (CheckIfInGrid(new Vector2Int(x,y))) return Cells[x,y];
            return null;
        }
        /// <summary>
        /// Return the Grid Coordinate at the given World Coordinate.
        /// </summary>
        /// <param name="World Coordinate"></param>
        /// <returns>Vector2Int</returns>
        public Vector2Int GetCellGridPosByWorld(Vector3 pos) {
            return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
        }
        /// <summary>
        /// Return the World Coordinate of the bottom left of the Cell given by the Grid Coordinate.
        /// </summary>
        /// <param name="Grid Coordinate"></param>
        /// <returns>Vector3</returns>
        public Vector3 GetCellWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x,  pos.y );
        }
        /// <summary>
        /// Return the WorldCoordinate of the center of the Cell given by the grid Coordinate.
        /// </summary>
        /// <param name="Grid Coordinate"></param>
        /// <returns>Vector3</returns>
        public Vector3 GetCellCenterWorldPosByCell(Vector2Int pos) {
            return new Vector3(pos.x ,  pos.y ) +new Vector3(0.5f,0.5f);
        }
        /// <summary>
        /// Return the list Grid  Coordinate of cell that are in the grid and are neibers to the given Grid Coordinate. 
        /// </summary>
        /// <param name="GridCoordinate"></param>
        /// <returns>List<Vector2Int></returns>
        public List<Vector2Int> GetNeibors(Vector2Int pos)
        {
            List<Vector2Int> neibors = new List<Vector2Int>();
            if(CheckIfInGrid(pos+new Vector2Int(1,0)))neibors.Add(pos+new Vector2Int(1,0));
            if(CheckIfInGrid(pos+new Vector2Int(1,1)))neibors.Add(pos+new Vector2Int(1,1));
            if(CheckIfInGrid(pos+new Vector2Int(0,1)))neibors.Add(pos+new Vector2Int(0,1));
            if(CheckIfInGrid(pos+new Vector2Int(-1,1)))neibors.Add(pos+new Vector2Int(-1,1));
            if(CheckIfInGrid(pos+new Vector2Int(-1,0)))neibors.Add(pos+new Vector2Int(-1,0));
            if(CheckIfInGrid(pos+new Vector2Int(-1,-1)))neibors.Add(pos+new Vector2Int(-1,-1));
            if(CheckIfInGrid(pos+new Vector2Int(0,-1)))neibors.Add(pos+new Vector2Int(0,-1));
            if(CheckIfInGrid(pos+new Vector2Int(1,-1)))neibors.Add(pos+new Vector2Int(1,-1));
            return neibors;
        }
        
        /// <summary>
        /// Return false if the Given Grid Coordinate is on the grid and the cell is Walkable.
        /// </summary>
        /// <param name="Grid Coordinate"></param>
        /// <returns>bool</returns>
        public bool CheckCellIsWall(Vector2Int pos)
        {
            if (CheckIfInGrid(pos))  return GetCell(pos).IsNonWalkable;
            return true;
        }

        public Vector2Int[] GetBuildingAura(Vector2Int center, int buildingSize, int AuraSize)
        {
            int startmod=0;
            int endmod = 0;
            if (AuraSize % 2 == 1) {
                startmod = Mathf.FloorToInt(AuraSize / 2f);
                endmod = Mathf.FloorToInt(AuraSize / 2f);
            }
            else {
                startmod = Mathf.FloorToInt(AuraSize / 2f)-1;
                endmod = Mathf.FloorToInt(AuraSize / 2f);
            }
            List<Vector2Int> auraSize = new List<Vector2Int>();
            Vector2Int startpos = center - new Vector2Int(AuraSize, AuraSize)-new Vector2Int(startmod, startmod);
            for (int x = startpos.x; x < center.x+AuraSize+endmod; x++) {
                for (int y = startpos.y; y < center.y+AuraSize+endmod; y++) {
                    if (CheckIfInGrid(new Vector2Int(x, y))) auraSize.Add(new Vector2Int(x,y));
                }
            }
            return auraSize.ToArray();
        }
        
    }
}