using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "newWave" , menuName = "WaveSO")]
    public class WaveSO : ScriptableObject
    {
        [Header("Prefabs & SO")] 
        public GameObject EnemyPrefab;
        public EnemySO OrcSO;
        public EnemySO WargSO;
        public EnemySO TrollSO;
        public EnemySO GhostSO;
        public EnemySO DrakeSO;

        
        [Header("Wave Settings")]
        public int NombreOrcs;
        public int NombreGhost;
        public int NombreWargs;
        public int NombreTrolls;
        public int NombreDrake;
        public float TimeToWait;


    }
}
