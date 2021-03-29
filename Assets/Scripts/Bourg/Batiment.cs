using System.Collections.Generic;
using Components;
using PlaneC;
using UnityEngine;

namespace Assets.Scripts.Bourg
{

    public class Batiment : MonoBehaviour
    {
	    [Header("Paramètres Batiment")]
    	public int Hp;
        public int CurrentHp;
    	public PlayGrig Playgrid;
    	public List<Vector2Int> OccupiedCells;
    	public int IndividualMoveFactor;
        public Vector2 Position;
        public PlayerManagerComponent Pm;

        private void Start()
        {
	        if(Playgrid == null) Playgrid = GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrig>();
	        if (Pm == null) Pm = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManagerComponent>();
	        Position = new Vector2(transform.position.x, transform.position.y);
        }

        private void OnDestroy()
        {
	        //Pm.Batiments.Remove(this);
        }
    }
}