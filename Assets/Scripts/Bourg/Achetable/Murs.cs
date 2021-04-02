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
        
        
        private List<Murs> _mursAdjacent = new List<Murs>();
        private List<Cell> _checked = new List<Cell>();
        private List<Murs> _allWalls = new List<Murs>();
        private Cell _rightCell, _leftCell, _upCell, _downCell;
        private Murs _rightWall, _leftWall, _upWall, _downWall;

        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private Cell _thisCell;
        private Vector2Int _intPos;

        //Initialisation
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _mesh = GetComponent<Mesh>();
            _intPos = OccupiedCells[0];

            _thisCell = Playgrid.GetCell(_intPos);
            _thisCell.IsWall = true; // Ajout du public bool IsWall dans Cell

            _rightCell = Playgrid.GetCell(new Vector2Int(_intPos.x+1, _intPos.y));
            _leftCell = Playgrid.GetCell(new Vector2Int(_intPos.x-1, _intPos.y));
            _upCell = Playgrid.GetCell(new Vector2Int(_intPos.x, _intPos.y+1));
            _downCell = Playgrid.GetCell(new Vector2Int(_intPos.x, _intPos.y-1));

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
            
            if (!_checked.Contains(_rightCell))
            {
                if (_rightCell.IsWall)
                {
                    foreach (Murs wall in Enumerable.Where(_allWalls, wall => wall.Position == _rightCell.Position))
                    {
                        _rightWall = wall;
                        if (!_mursAdjacent.Contains(_rightWall))
                        {
                            _mursAdjacent.Add(_rightWall);
                        }
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
                    foreach (Murs wall in Enumerable.Where(_allWalls, wall => wall.Position == _leftCell.Position))
                    {
                        _leftWall = wall;
                        if (!_mursAdjacent.Contains(_leftWall))
                        {
                            _mursAdjacent.Add(_leftWall);
                        }
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
                    foreach (Murs wall in Enumerable.Where(_allWalls, wall => wall.Position == _upCell.Position))
                    {
                        _upWall = wall;
                        if (!_mursAdjacent.Contains(_upWall))
                        {
                            _mursAdjacent.Add(_upWall);
                        }
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
                    foreach (Murs wall in Enumerable.Where(_allWalls, wall => wall.Position == _downCell.Position))
                    {
                        _downWall = wall;
                        if (!_mursAdjacent.Contains(_downWall))
                        {
                            _mursAdjacent.Add(_downWall);
                        }
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
                if (!_mursAdjacent.Contains(_upWall) && !_mursAdjacent.Contains(_downWall))
                {
                    _mesh = MeshSecondaire;
                    _meshRenderer.material = MaterialSecondaire;
                }
                else
                {
                    _mesh = MeshMur;
                    _meshRenderer.material = MaterialPrincipal;
                }
            }
            
            //Mur Vertical
            else if (_mursAdjacent.Contains(_upWall) && _mursAdjacent.Contains(_downWall))
            {
                if (!_mursAdjacent.Contains(_rightWall) && !_mursAdjacent.Contains(_leftWall))
                {
                    transform.Rotate(0,90,0);
                    _mesh = MeshSecondaire;
                    _meshRenderer.material = MaterialSecondaire;
                }
                else
                {
                    _mesh = MeshMur;
                    _meshRenderer.material = MaterialPrincipal;
                }
            }

            //Mur Basique
            else
            {
                _mesh = MeshMur;
                _meshRenderer.material = MaterialPrincipal;
                if (_mursAdjacent.Count == 0) return;
                foreach (Murs wall in _mursAdjacent)
                {
                    wall.CheckAndUpdate();
                }
            }
        }
        
        
        private void OnDestroy()
        {
            _thisCell.IsWall = false;

            if (_mursAdjacent.Count == 0) return;
            foreach (Murs wall in _mursAdjacent)
            {
                wall.CheckAndUpdate();
            }
        }
        

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        