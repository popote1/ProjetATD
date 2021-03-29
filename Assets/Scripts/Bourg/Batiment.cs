using System;
using System.IO;
using System.Collections.Generic;
using Scripts.PlainC;
using Scripts.Components;
using UnityEngine;

namespace Bourg
{

    public class Batiment : MonoBehaviour
    {
	    [Header("Paramètres Batiment")]
    	public int Hp;
        public int CurrentHp;
    	public PlayGrid Playgrid;
    	public List<Vector2Int> OccupiedCells;
    	public int IndividualMoveFactor;
        public Vector2 Position;
        public PlayerManager Pm;

        private void Start()
        {
	        if(Playgrid == null) Playgrid = GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrid>();
	        if (Pm == null) Pm = GameObject.FindGameObjectWithTag("Manager");
	        Position = new Vector2(transform.position.x, transform.position.y);
        }

        private void OnDestroy()
        {
	        Pm.Batiments.Remove(this);
        }
    }
}