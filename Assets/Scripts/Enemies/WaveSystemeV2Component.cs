using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;
using Enemies;
using Random = UnityEngine.Random;

public class WaveSystemeV2Component : MonoBehaviour
{
   public List<WaveSO> WaveSo;


    [Header("Wave Info")] 
    public int WaveIndex=0;
    public float WaveTimer;
    public List<EnemyComponent> EnnemisAlive;
    public bool IsPlaying;


    [Header("Wave Spawning parameters")]
    public int NbOrc;
    public int NbGhost;
    public int NbWarg;
    public int NbTroll;
    public int SpawnBashSize=1;
    public float TimeBetweenSapwnBash=0.1f;
    public EnemyComponent EnemyPrefab;
    public  GameManagerComponent gameManagerComponent;

    private float _timer = 0f;
    //private PlayerManagerComponent _pm;
    
    private List<Vector3> _spawns = new List<Vector3>();
    private bool _isSpawning;

    // Start is called before the first frame update
    void Start()
    {
       // _pm ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManagerComponent>();
        gameManagerComponent ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerComponent>();

/*
        WaveSo[0].NombreOrcs = NbOrc;
        WaveSo[0].NombreTrolls = NbTroll;
        WaveSo[0].NombreWargs = NbWarg;
        WaveSo[0].NombreGhost = NbGhost;
*/
        _spawns = gameManagerComponent.EnnemisSpawnZones;
        IsPlaying = true;
    }
[ContextMenu("StartSpawn")]
    public void GenrateWave()
    {
        if (!_isSpawning)
        {
            NbGhost = WaveSo[WaveIndex].NombreGhost;
            NbOrc = WaveSo[WaveIndex].NombreOrcs;
            NbTroll = WaveSo[WaveIndex].NombreTrolls;
            NbWarg = WaveSo[WaveIndex].NombreWargs;
            _isSpawning = true;
            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        
        int spawnInThisBash = 0;
        while (NbGhost + NbOrc + NbTroll + NbWarg > 0)
        {
            EnemyComponent mob = new EnemyComponent();
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            if (NbGhost > 0) {
                mob= Instantiate(EnemyPrefab, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].GhostSO;
                NbGhost--;
            }
            else if (NbOrc > 0)
            {
                mob= Instantiate(EnemyPrefab, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].OrcSO;
                NbOrc--;
            }else if (NbTroll > 0)
            {
                mob =  Instantiate(EnemyPrefab, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].TrollSO;
                NbTroll--;
            }else if (NbWarg > 0)
            {
                mob =  Instantiate(EnemyPrefab, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].WargSO;
                NbWarg--;
            }
            EnnemisAlive.Add(mob);

            SpawnBashSize++;
            if (spawnInThisBash > SpawnBashSize) {
                spawnInThisBash = 0;
                yield return new  WaitForSeconds(TimeBetweenSapwnBash);
            }
        }
        _isSpawning = false;
        yield return null;
    }

    private void Update()
    {
        if (IsPlaying) {
            WaveTimer += Time.deltaTime;
            if (WaveTimer > WaveSo[WaveIndex].TimeToWait) {
                WaveIndex++;
                if (WaveIndex <= WaveSo.Count - 1) {
                    WaveTimer = 0;
                    GenrateWave();
                }
            }
            else {
                IsPlaying = false;
            }
        }
    }
}
