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
        public int PhysicDamagesResistance;
        public int MagicDamagesResistance;

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

        public void TakePhysicDamages(int damages)
        {
	        CurrentHp -= damages - PhysicDamagesResistance;
	        if(CurrentHp <= 0)
	        {
		        Destroy(this.gameObject);
	        }
        }
        
        public void TakeMagicDamages(int damages)
        {
	        CurrentHp -= damages - MagicDamagesResistance;
	        if(CurrentHp <= 0)
	        {
		        Destroy(this.gameObject);
	        }
        }
        
    }
}