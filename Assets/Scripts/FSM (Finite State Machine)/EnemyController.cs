using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Controller
{
    [SerializeField] private float rangoVision;
    [SerializeField] private float anguloVision;
    [SerializeField] private float attackDistance;
    [SerializeField] private float maximumVelocity;
    [SerializeField] private LayerMask queEsTarget;
    [SerializeField] private LayerMask queEsObstaculo;


    private State<EnemyController> currentState;
    private NavMeshAgent agent;
    private Transform target;
    private Animator anim;

    private PatrolState patrolState;
    private ChaseState chaseState;
    private AttackState attackState;

    #region getter & setters
    public NavMeshAgent Agent { get => agent; }
    public float RangoVision { get => rangoVision; } //Encapsulamos
    public LayerMask QueEsTarget { get => queEsTarget; }
    public LayerMask QueEsObstaculo { get => queEsObstaculo;  }
    public float AnguloVision { get => anguloVision; }
    public PatrolState PatrolState { get => patrolState; }
    public ChaseState ChaseState { get => chaseState; }
    public AttackState AttackState { get => attackState; }
    public Transform Target { get => target; set => target = value; }
    public float AttackDistance { get => attackDistance; }
    public Animator Anim { get => anim;}
    public float MaximumVelocity { get => maximumVelocity; }
    #endregion

    private void Awake()
    {
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        ChangeState(patrolState);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdateState();
        }    
    }

    public void ChangeState(State<EnemyController> newState)
    {
       if(currentState != null && currentState != newState)
        {
            currentState.OnExitState();
        }
       currentState = newState; // Mi estado actual pasa a ser nuevo
       currentState.OnEnterState(this); 
    }
}
