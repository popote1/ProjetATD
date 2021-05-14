using System.Collections.Generic;
using PlaneC;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace Assets.Scripts.Bourg.Achetable
{
    public class Murs : Achetables
    {
        [Header("Mesh")]
        public Mesh MeshMur;
        public Mesh MeshSecondaire;

        [Header("Material")]
        public Material MaterialPrincipal;
        public Material MaterialSecondaire;
        
        [HideInInspector]
        public List<Murs> checkedNeighbours = new List<Murs>();
        
        private List<Murs> _mursAdjacent = new List<Murs>();
        private List<Cell> _checked = new List<Cell>();
        private List<Murs> _allWalls = new List<Murs>();
        private Cell _rightCell, _leftCell, _upCell, _downCell;
        private Murs _rightWall, _leftWall, _upWall, _downWall;

        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Cell _thisCell;
        [HideInInspector]
        public Vector2Int IntPos;

        //Initialisation
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            IntPos = OccupiedCells[0];

            _thisCell = Playgrid.GetCell(IntPos);
            _thisCell.IsWall = true; // Ajout du public bool IsWall dans Cell

            _rightCell = Playgrid.GetCell(new Vector2Int(IntPos.x+1, IntPos.y));
            _leftCell = Playgrid.GetCell(new Vector2Int(IntPos.x-1, IntPos.y));
            _upCell = Playgrid.GetCell(new Vector2Int(IntPos.x, IntPos.y+1));
            _downCell = Playgrid.GetCell(new Vector2Int(IntPos.x, IntPos.y-1));

            CheckAndUpdate();
        }

        
        //Check if neighbours are walls
        private void CheckAndUpdate()
        {
            _checked.Clear();
            _allWalls.Clear();
            
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Murs"))
            {
                Murs mur = go.GetComponent<Murs>();
                if (!_allWalls.Contains(mur))
                {
                    _allWalls.Add(mur);
                }
            }
 
            foreach (Murs mur in _allWalls)
            {
                if (mur.IntPos == _rightCell.Position)
                {
                    _rightWall = mur;
                }

                if (mur.IntPos == _leftCell.Position)
                {
                    _leftWall = mur;
                }

                if (mur.IntPos == _upCell.Position)
                {
                    _upWall = mur;
                }

                if (mur.IntPos == _downCell.Position)
                {
                    _downWall = mur;
                }
            }

            if (!_checked.Contains(_rightCell))
            {
                if (_rightCell.IsWall)
                {
                    if (!_mursAdjacent.Contains(_rightWall))
                    {
                        _mursAdjacent.Add(_rightWall); 
                    }
                    _checked.Add(_rightCell);
                }
                else
                {
                    _checked.Add(_rightCell);
                }
            }
            
            if (!_checked.Contains(_leftCell))
            {
                if (_leftCell.IsWall)
                {
                    if (!_mursAdjacent.Contains(_leftWall))
                    {
                        _mursAdjacent.Add(_leftWall);
                    }
                    _checked.Add(_leftCell);
                }
                else
                {
                    _checked.Add(_leftCell);
                }
            }
            
            if (!_checked.Contains(_upCell))
            {
                if (_upCell.IsWall)
                {
                    if (!_mursAdjacent.Contains(_upWall))
                    {
                        _mursAdjacent.Add(_upWall);
                    }
                    _checked.Add(_upCell);
                }
                else
                {
                    _checked.Add(_upCell);
                }
            }
            
            if (!_checked.Contains(_downCell))
            {
                if (_downCell.IsWall)
                {
                    if (!_mursAdjacent.Contains(_downWall))
                    {
                        _mursAdjacent.Add(_downWall);
                    }
                    _checked.Add(_downCell);
                }
                else
                {
                    _checked.Add(_downCell);
                }
            }

            UpdateMesh();

        }
        

        //Change Mesh & Material
        private void UpdateMesh()
        {
            //Mur Horizontal
            if (_mursAdjacent.Contains(_rightWall) && _mursAdjacent.Contains(_leftWall))
            {
                if (_mursAdjacent.Contains(_upWall) || _mursAdjacent.Contains(_downWall))
                {
                    _meshFilter.mesh = MeshMur;
                    _meshRenderer.material = MaterialPrincipal;
                    
                }
                else
                {
                    _meshFilter.mesh = MeshSecondaire;
                    _meshRenderer.material = MaterialSecondaire;
                }
            }
            
            //Mur Vertical
            else if (_mursAdjacent.Contains(_upWall) && _mursAdjacent.Contains(_downWall))
            {
                if (_mursAdjacent.Contains(_rightWall) || _mursAdjacent.Contains(_leftWall))
                {
                    _meshFilter.mesh = MeshMur;
                    _meshRenderer.material = MaterialPrincipal;
                    
                }
                else
                {
                    transform.Rotate(0,0,90);
                    _meshFilter.mesh = MeshSecondaire;
                    _meshRenderer.material = MaterialSecondaire;
                }
            }

            //Mur Basique
            else
            {
                _meshFilter.mesh = MeshMur;
                _meshRenderer.material = MaterialPrincipal;
            }
            
            if (_mursAdjacent.Count == 0) return;

            foreach (Murs wall in _mursAdjacent)
            {
                if (!checkedNeighbours.Contains(wall))
                {
                    checkedNeighbours.Add(wall);
                    wall.CheckAndUpdate();
                }
            }
        }
        
        
        public override void OnDestroy()
        {
            _thisCell.IsWall = false;

            if (_mursAdjacent.Count != 0)
            {
                foreach (Murs wall in _mursAdjacent)
                {
                    
                    wall.checkedNeighbours.Remove(this);
                    wall._mursAdjacent.Remove(this);
                    wall.CheckAndUpdate();
                }
            }
            base.OnDestroy();
            if (SecurityValue != 0)
            {
                Vector2Int[] aura = Playgrid.GetBuildingAura(Playgrid.GetOriginalBuildingCenter(OccupiedCells.ToArray()),
                    (int) Mathf.Sqrt(OccupiedCells.Count), SecurityRange);
                foreach (Vector2Int vec in aura)
                {
                    Playgrid.GetCell(vec).SecurityValue -= SecurityValue;
                }
                
            }
            foreach (Vector2Int cell in OccupiedCells)
            {
                Playgrid.GetCell(cell).IndividualMoveValue -= IndividualMoveFactor;
            }
            Debug.Log("Remove Wall data");
        }
        

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        