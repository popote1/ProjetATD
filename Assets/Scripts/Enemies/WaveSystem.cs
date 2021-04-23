using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Components;
using Enemies;
using PlaneC;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    public WaveSO WaveSo;

    [Header("First Wave Settings")]
    public int NbOrc;
    public int NbGhost;
    public int NbWarg;
    public int NbTroll;

    private float _timer = 0f;
    private PlayerManagerComponent _pm;
    private GameManagerComponent _gm;
    private List<Vector3> _spawns = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        _pm ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManagerComponent>();
        _gm ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerComponent>();


        WaveSo.NombreOrcs = NbOrc;
        WaveSo.NombreTrolls = NbTroll;
        WaveSo.NombreWargs = NbWarg;
        WaveSo.NombreGhost = NbGhost;

        _spawns = _gm.SpawnList;
        StartWave();
    }

    private void StartWave()
    {
        for (int i = 0; i < WaveSo.NombreOrcs; i++)
        {
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            GameObject enemy = Instantiate(WaveSo.EnemyPrefab, randomSpawn, Quaternion.identity);
            EnemyComponent component = enemy.GetComponent<EnemyComponent>();
            component.Enemy = WaveSo.OrcSO;
        }
        
        for (int i = 0; i < WaveSo.NombreGhost; i++)
        {
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            GameObject enemy = Instantiate(WaveSo.EnemyPrefab, randomSpawn, Quaternion.identity);
            EnemyComponent component = enemy.GetComponent<EnemyComponent>();
            component.Enemy = WaveSo.GhostSO;
        }
        
        for (int i = 0; i < WaveSo.NombreTrolls; i++)
        {
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            GameObject enemy = Instantiate(WaveSo.EnemyPrefab, randomSpawn, Quaternion.identity);
            EnemyComponent component = enemy.GetComponent<EnemyComponent>();
            component.Enemy = WaveSo.TrollSO;
        }
        
        for (int i = 0; i < WaveSo.NombreWargs; i++)
        {
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            GameObject enemy = Instantiate(WaveSo.EnemyPrefab, randomSpawn, Quaternion.identity);
            EnemyComponent component = enemy.GetComponent<EnemyComponent>();
            component.Enemy = WaveSo.WargSO;
        }

        /*
        for (int i = 0; i < WaveSo.NombreDrake; i++)
        {
            
        }*/

        EndWave();
    }

    private void EndWave()
    {
        if (_timer < WaveSo.TimeToWait)
        {
            _timer++;
        }
        else
        {
            SetNextWave();
        }
    }

    private void SetNextWave()
    {
        _timer = 0f;
        
        //Check pm to see the player's progression & add enemy to next wave
        WaveSo.NombreGhost += 1;
        WaveSo.NombreOrcs += 1;
        WaveSo.NombreTrolls += 1;
        WaveSo.NombreWargs += 1;
        WaveSo.NombreDrake += 1;
        
        StartWave();
    }
}
