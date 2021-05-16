using UnityEngine;
using Assets.Scripts.Bourg;

namespace PlaneC
{
    public class Cell
    {
        public Vector2Int Position;
        public PlayGrid PlayGrid;
        

       
        
        public bool IsNonWalkable;
        public Vector2 MoveVector;
        public float DragFactor;
        public Batiment Batiment;
        public bool IsPlayble;
        public bool IsRoad;
        public bool IsWall;
        public GameObject ConstructionTile;
        public int SecurityValue;
        
        private int _moveValue;
        private int _individualMoveValue;
        public int MoveValue
        {
            get => _moveValue;
            set {
                if (value < 0) _moveValue = 0;
                else _moveValue = value;
            }
        }

        public int IndividualMoveValue
        {
            get => _individualMoveValue;
            set {
                if (value < 0) _individualMoveValue = 0;
                else _individualMoveValue = value;
            }
        }
        

        public Cell(Vector2Int position, PlayGrid playGrid)
        {
            Position = position;
            PlayGrid = playGrid;
            
        }
    }
}

