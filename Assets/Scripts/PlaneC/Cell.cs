using UnityEngine;

namespace PlaneC
{
    public class Cell
    {
        public Vector2Int Position;
        public PlayGrig PlayGrid;
        public int IndividualMoveValue;
        public int MoveValue;
        public bool IsNonWalkable;
        public Vector2 MoveVector;
        public float DragFactor;
        //public Batiment Batiment;
        public bool IsPlayble;
        public bool IsRoad;
        public GameObject ConstructionTile;

        public Cell(Vector2Int position, PlayGrig playGrid)
        {
            Position = position;
            PlayGrid = playGrid;
            
        }
    }
}

