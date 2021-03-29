using System;
using System.IO;
using System.Collections.Generic;
using Scripts.Main;
using UnityEngine;

namespace Bourg
{

    public class Batiment : MonoBehaviour
    {
	    [Header("Paramètres Batiment")]
    	public int Hp;
        public int CurrentHp;
    	private PlayGridV2 Playgrid;
    	public List<Vector2Int> OccupiedCells;
    	public int IndividualMoveFactor;
        public Vector2 Position;

        private void Start()
        {
	        Playgrid = GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGridV2>();
	        Position = new Vector2(transform.position.x, transform.position.y);
        }

        private void OnDestroy()
        {
	        GameManager.Batiments.Remove(this);
        }
    }
}