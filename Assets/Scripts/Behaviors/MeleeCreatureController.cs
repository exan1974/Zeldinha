using System;
using EventArgs;
using UnityEngine;
using UnityEngine.AI;
using Zeldinha.Assets.Scripts.Behaviors.MeleeCreature.States;

public class MeleeCreatureController : MonoBehaviour 
{
    [HideInInspector] public MeleeCreatureHelper helper;
    [HideInInspector] public NavMeshAgent thisAgent;
    [HideInInspector] public Animator thisAnimator;
    [HideInInspector] public Life thisLife;
    [HideInInspector] public Collider thisCollider;
    [HideInInspector] public Rigidbody thisRigidbody;


    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Follow followState;
    [HideInInspector] public Attack attackState;
    [HideInInspector] public Hurt hurtState;
    [HideInInspector] public Dead deadState;

    [Header("General")]
    public float searchRadius = 5f;

    [Header("Idle")]
    public float targetSearchInterval = 1f;

    [Header("Follow")]
    public float ceaseFollowInterval = 4f;

    [Header("Attack")]
    public float distanceToAttack = 1f;
    public float attackRadius = 1.5f;
    public float damageDelay = 0f;
    public float attackSphereRadius = 1.5f;
    public float attackDuration = 1f;
    public int attackDamage = 1;

    [Header("Hurt")]
    public float hurtDuration = 1f;

    [Header("Dead")]
    public float destroyIfFar = 1f;

    [Header("Effects")]
    public GameObject knockoutEffect;

    [Header("Debug")]
    public string currentStateName;

    private void Awake() 
    {
        // Get components
        thisAgent = GetComponent<NavMeshAgent>();
        thisLife = GetComponent<Life>();
        thisAnimator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();
        thisRigidbody = GetComponent<Rigidbody>();

        // Create instances
        this.helper = new MeleeCreatureHelper(this);
    }

    private void Start() 
    {
        // Create StateMachine
        stateMachine = new StateMachine();

        idleState = new Idle (this);
        followState = new Follow (this);
        attackState = new Attack (this);
        hurtState = new Hurt (this);
        deadState = new Dead (this);

        stateMachine.ChangeState(idleState);

        // Register listeners
        thisLife.OnDamage += OnDamage;
    }

    private void OnDamage(object sender, DamageEventArgs args)
    {
        Debug.Log("monstro tomou dano de " + args.damage + " de " + args.attacker.name);
        stateMachine.ChangeState(hurtState);
    }

    // Update state machine
    private void Update()
    {
        stateMachine.Update();
        currentStateName = stateMachine.currentStateName;

        // Update animator
        var velocityRate = thisAgent.velocity.magnitude / thisAgent.speed;
        thisAnimator.SetFloat("fVelocity", velocityRate);
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

