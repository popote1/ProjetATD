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
    	public PlayGrid Playgrid;
        public int CellNeeded;
    	public List<Vector2Int> OccupiedCells;
    	public int IndividualMoveFactor;
        public Vector2 Position;

        private void Start()
        {
	        Playgrid ??= GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrid>();
	        Position = new Vector2(transform.position.x, transform.position.y);
	        CurrentHp = Hp;
        }

        private void OnDestroy()
        {
	        PlayerManagerComponent.Batiments.Remove(this);
        }

        public void TakeDamage(int damages)
        {
	        CurrentHp -= damages;
	        if(CurrentHp <= 0)
	        {
		        Destroy(this.gameObject);
	        }
        }
        
    }
}