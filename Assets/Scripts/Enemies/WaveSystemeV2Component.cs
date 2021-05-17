using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;
using Enemies;
using UnityEngine.Rendering.UI;
using Random = UnityEngine.Random;

public class WaveSystemeV2Component : MonoBehaviour
{
   public List<WaveSO> WaveSo = new List<WaveSO>();


    [Header("Wave Info")] 
    public int WaveIndex=0;
    public float WaveTimer;
    public List<EnemyComponent> EnnemisAlive;
    public bool IsPlaying = false;
    public bool IsPreWave= true;
    public float PrewaveTimer=10;
    public float DrakeAnimSpeed = 5f;


    [Header("Wave Spawning parameters")]
    public int NbOrc;
    public int NbGhost;
    public int NbWarg;
    public int NbTroll;
    public int NbDrake;
    public int SpawnBashSize=1;
    public float TimeBetweenSapwnBash=0.1f;
    public EnemyComponent EnemyPrefab;
    public EnemyComponent EnemyPrefabOrc;
    public EnemyComponent EnemyPrefabTroll;
    public EnemyComponent EnemyPrefabWarg;
    public EnemyComponent EnemyPrefabGhost;
    public EnemyComponent EnemyPrefabDrake;
    public GameObject DrakeAnimation;
    
    public  GameManagerComponent GameManagerComponent;

    public int WavesizeStartSize;
    private float _timer = 0f;
    //private PlayerManagerComponent _pm;
    
    private List<Vector3> _spawns = new List<Vector3>();
    private bool _isSpawning =false;
    private bool _isDrakeAnimDone = false;

    // Start is called before the first frame update
    void Start()
    {
       // _pm ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManagerComponent>();
        GameManagerComponent ??= GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerComponent>();

/*
        WaveSo[0].NombreOrcs = NbOrc;
        WaveSo[0].NombreTrolls = NbTroll;
        WaveSo[0].NombreWargs = NbWarg;
        WaveSo[0].NombreGhost = NbGhost;
*/
        _spawns = GameManagerComponent.EnnemisSpawnZones;
        //IsPlaying = true;
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
            NbDrake = WaveSo[WaveIndex].NombreDrake;
            _isSpawning = true;
            IsPlaying = true;
            EnnemisAlive.Clear();
            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        WavesizeStartSize = 0;
        int spawnInThisBash = 0;
        while (NbGhost + NbOrc + NbTroll + NbWarg + NbDrake > 0)
        {
            EnemyComponent mob = new EnemyComponent();
            Vector3 randomSpawn = _spawns[Random.Range(0, _spawns.Count - 1)];
            if (NbGhost > 0) {
                mob= Instantiate(EnemyPrefabGhost, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].GhostSO;
                NbGhost--;
            }
            else if (NbDrake > 0)
            {
                if (!_isDrakeAnimDone)
                {
                    Vector3 screenBottomCenter = new Vector3(Screen.width / 2, 0, 0);
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenBottomCenter);
                    
                    GameObject animatedDrake = Instantiate(DrakeAnimation, worldPos, Quaternion.identity);
                    
                    Vector3 target = randomSpawn;
                    float step = DrakeAnimSpeed * Time.deltaTime;
                    
                    if (animatedDrake.transform.position != target)
                    {
                        animatedDrake.transform.position = Vector3.MoveTowards(transform.position, target, step);
                    }
                    
                    else
                    {
                        _isDrakeAnimDone = true;
                    }
                }
                else
                {
                    mob =  Instantiate(EnemyPrefabDrake, randomSpawn, Quaternion.identity);
                    mob.Enemy = WaveSo[WaveIndex].DrakeSO;
                    NbDrake--;
                    _isDrakeAnimDone = false;
                }
            }
            
            else if (NbOrc > 0)
            {
                mob= Instantiate(EnemyPrefabOrc, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].OrcSO;
                NbOrc--;
            }else if (NbTroll > 0)
            {
                mob =  Instantiate(EnemyPrefabTroll, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].TrollSO;
                NbTroll--;
            }else if (NbWarg > 0)
            {
                mob =  Instantiate(EnemyPrefabWarg, randomSpawn, Quaternion.identity);
                mob.Enemy = WaveSo[WaveIndex].WargSO;
                NbWarg--;
            }

            mob.Playgrid = GameManagerComponent.PlayGrid;
            mob.WaveSystemeV2Component = this;
            EnnemisAlive.Add(mob);

            spawnInThisBash++;
            WavesizeStartSize++;
            if (spawnInThisBash > SpawnBashSize) {
                spawnInThisBash = 0;
                yield return new  WaitForSeconds(TimeBetweenSapwnBash);
            }
        }
        _isSpawning = false;
        yield return null;
    }
[ContextMenu("Lance la vague suivante")]
    public void LancheNextWave()
    {
        if (!_isSpawning)
        {
            WaveIndex++;
            if (WaveIndex <= WaveSo.Count - 1) {
                WaveTimer = 0;
                GenrateWave();
            }
            else
            {
                IsPlaying = false;
            }
        }
    }

    public void EnnemiDie(EnemyComponent ennemi)
    {
        if (EnnemisAlive.Contains(ennemi)) EnnemisAlive.Remove(ennemi);
    }

    public float GetWaveProgress() {
        if (IsPreWave) return 0;
        return  (float)EnnemisAlive.Count / WavesizeStartSize;
    }

    public float GetWaveTimePogress() {
        if (IsPreWave) return 1-(WaveTimer / PrewaveTimer);
        if (!IsPlaying) return 0;
        return 1 - (WaveTimer / WaveSo[WaveIndex].TimeToWait);
    }

    public int GetWaveNumber() {
        if (IsPreWave) return 0;
        if (!IsPlaying) return WaveSo.Count - 1 ;
        return 1 +WaveIndex;
    }
    
    

    private void Update()
    {
        if (IsPlaying) {
            WaveTimer += Time.deltaTime;
            if (IsPreWave && WaveTimer > PrewaveTimer)
            {
                WaveTimer = 0;
                GenrateWave();
                IsPreWave = false;
            }
            else if ((WaveTimer > WaveSo[WaveIndex].TimeToWait||GetWaveProgress()==0)&&!IsPreWave) 
            {
                WaveIndex++;
                Debug.Log(" Lance la nouvelle vague");
                if (WaveIndex <= WaveSo.Count - 1) {
                    WaveTimer = 0;
                    GenrateWave();
                }
                else {
                    IsPlaying = false;
                    GameManagerComponent.SetWin();
                }
            }
            
        }
    }
}
