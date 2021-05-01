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

        public int SecurityRange;
        public int SecurityValue;

        private void Start()
        {
	        Playgrid ??= GameObject.FindGameObjectWithTag("PlayGrid").GetComponent<PlayGrid>();
	        Position = new Vector2(transform.position.x, transform.position.y);
	        CurrentHp = Hp;
        }

        public virtual void OnDestroy()
        {
	        PlayerManagerComponent.Batiments.Remove(this);
	        if (SecurityValue != 0)
	        {
		       Vector2Int[] aura = Playgrid.GetBuildingAura(Playgrid.GetOriginalBuildingCenter(OccupiedCells.ToArray()),
			        (int) Mathf.Sqrt(OccupiedCells.Count), SecurityRange);
		       foreach (Vector2Int vec in aura)
		       {
			       Playgrid.GetCell(vec).SecurityValue -= SecurityValue;
		       }
		       foreach (Vector2Int cell in OccupiedCells)
		       {
			       Playgrid.GetCell(cell).IndividualMoveValue -= IndividualMoveFactor;
		       }
		       Debug.Log("Remove building data");
	        }
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