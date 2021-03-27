using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Unity.Mathematics;

namespace PlaneC
{
    public class PlayGrig
    {
        public Cell[,] Cells;
        public int Width;
        public int Height;

        public PlayGrig(int width, int height)
        {
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
            if (pos.x > -1 && pos.x < Width && pos.y > -1 && pos.y < Height) return true;
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
            return new Vector3(pos.x ,  pos.y ) +Vector3.one/2;
        }
        
    }
}