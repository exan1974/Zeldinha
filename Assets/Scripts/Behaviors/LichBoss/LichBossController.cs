using System;
using EventArgs;
using UnityEngine;
using UnityEngine.AI;
using Behaviors.LichBoss.States;
using System.Collections.Generic;
using System.Collections;

namespace Behaviors.LichBoss
{
public class LichBossController : MonoBehaviour 
{
    // Helper
    [HideInInspector] public LichBossHelper helper;

    // Components
    [HideInInspector] public NavMeshAgent thisAgent;
    [HideInInspector] public Animator thisAnimator;
    [HideInInspector] public Life thisLife;
    [HideInInspector] public Rigidbody thisRigidbody;

    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Follow followState;
    [HideInInspector] public AttackNormal attackNormalState;
    [HideInInspector] public AttackSuper attackSuperState;
    [HideInInspector] public AttackRitual attackRitualState;
    [HideInInspector] public Hurt hurtState;
    [HideInInspector] public Dead deadState;

    [Header("General")]
    public float lowHealthThreshold = 0.3f;
    public Transform staffTop;
    public Transform staffBottom;

    [Header("Idle")]
    public float idleDuration = 0.3f;

    [Header("Follow")]
    public float ceaseFollowInterval = 4f;

    [Header("Attack")]
    public int attackDamage = 1;
    public Vector3 aimOffset = new Vector3(0, 1.4f, 0);
    
    [Header("Attack normal")]
    public float attackNormalDelay = 0f;
    public float attackNormalDuration = 1f;
    public float attackNormalImpulse = 10f;

    [Header("Attack super")]
    public float attackSuperMagicDelay = 0f;
    public float attackSuperMagicDuration = 1f;
    public float attackSuperDuration = 1f;
    public int attackSuperMagicCount = 5;
    public float attackSuperImpulse = 10f;


    [Header("Attack ritual")]
    public float distanceToRitual = 2.5f;
    public float attackRitualDelay = 2.5f;
    public float attackRitualDuration = 2.5f;

    [Header("Hurt")]
    public float hurtDuration = 1f;
    public int hitBackAfterHits = 3;

    [Header("Magic")]
    public GameObject fireballPrefab;
    public GameObject energyBallPrefab;
    public GameObject ritualPrefab;

    [Header("Debug")]
    public string currentStateName;

    [HideInInspector] internal int hitsTakenWithoutRitual;

    // State coroutines
    [HideInInspector] public List<IEnumerator> stateCoroutines = new List<IEnumerator>();


    private void Awake() 
    {
        // Get components
        thisAgent = GetComponent<NavMeshAgent>();
        thisLife = GetComponent<Life>();
        thisAnimator = GetComponent<Animator>();

        // Create instances
        this.helper = new LichBossHelper(this);
    }

    private void Start() 
    {
        // Create StateMachine
        stateMachine = new StateMachine();

        idleState = new Idle (this);
        followState = new Follow (this);
        attackNormalState = new AttackNormal (this);
        attackSuperState = new AttackSuper (this);
        attackRitualState = new AttackRitual (this);
        hurtState = new Hurt (this);
        deadState = new Dead (this);

        stateMachine.ChangeState(idleState);

        // Register listeners
        thisLife.OnDamage += OnDamage;
    }

    private void OnDamage(object sender, DamageEventArgs args)
    {
        Debug.Log("Boss tomou dano de " + args.damage + " de " + args.attacker.name);
        stateMachine.ChangeState(hurtState);
    }

    // Update state machine
    private void Update()
    {
        // Update state machine
        var bossBattleHandler = GameManager.Instance.bossBattleHandler;
        if (bossBattleHandler.IsActive())
        {
            stateMachine.Update();

        }
        currentStateName = stateMachine.currentStateName;

        // Update animator
        var velocityRate = thisAgent.velocity.magnitude / thisAgent.speed;
        thisAnimator.SetFloat("fVelocity", velocityRate);

        // Face player
        if(!thisLife.IsDead())
        {
            var player = GameManager.Instance.player;
            var vecToPlayer = player.transform.position - transform.position;
            vecToPlayer.y = 0;
            vecToPlayer.Normalize();
            var desiredRotation = Quaternion.LookRotation(vecToPlayer);
            var newRotation = Quaternion.LerpUnclamped(transform.rotation, desiredRotation, 0.2f);
            transform.rotation = newRotation;
        }
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

 

}
}

