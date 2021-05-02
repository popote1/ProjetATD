using UnityEngine.Events;
using Components;
using UnityEngine;

namespace Assets.Scripts.Bourg
{

    public class Mairie : Batiment
    {
        
        public override void OnDestroy()
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
                    Playgrid.GetCell(cell).IndividualMoveValue -= DragFactor;
                }
                
            }
            Debug.Log("Maire Destroy");
            GameManagerComponent.IsLose = true;
        }
    }
}