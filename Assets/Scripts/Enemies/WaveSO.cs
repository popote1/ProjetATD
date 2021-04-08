using UnityEngine;

namespace Enemies
{
    public class WaveSO : ScriptableObject
    {
        [Header("Prefabs")] 
        public GameObject Orc;
        public GameObject Ghost;
        public GameObject Warg;
        public GameObject Troll;
        public GameObject Drake;
        
        
        [Header("Wave Settings")]
        public int NombreOrcs;
        public int NombreGhost;
        public int NombreWargs;
        public int NombreTrolls;
        public int NombreDrake;
        public float EndWavePercentage;
        public float TimeToWait;


    }
}
