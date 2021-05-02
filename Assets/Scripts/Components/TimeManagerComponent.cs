
using UnityEngine;
using UnityEngine.Events;

public class TimeManagerComponent : Singleton<TimeManagerComponent> {

    [Header("Parameters")]
    public float Multiplicator;
    public float PhysicsMultiplicator;
    
    public UnityEvent BetaTick;
    public UnityEvent PhisicTick;

    private float _beta;
    public float DeltaTime;
    private float _physicTimer;
    public float PhysicDeltaTime;
    public static TimeManagerComponent TM;

    private void Awake()
    {
        TM = this;
    }

    private void Update()
    {
        DeltaTime = Time.deltaTime * Multiplicator;
        _beta += DeltaTime;
        if (_beta > 1)
        {
            BetaTick.Invoke();
            _beta = 0;
        }

        PhysicDeltaTime = Time.deltaTime * PhysicsMultiplicator;
        _physicTimer += PhysicDeltaTime;
        if (_physicTimer >= 1)
        {
            Debug.Log("Do Physics Tick");
            PhisicTick.Invoke();
            _physicTimer = 0;
        }
    }
}