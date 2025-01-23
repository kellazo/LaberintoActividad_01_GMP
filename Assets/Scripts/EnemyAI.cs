using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private NavMeshAgent agent;

    // preparando mis cosas pero no depende de otros, se ejecuta una sola vez. Nada mas comenzar el juego estes o no estes activo
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // cuando nos hemos activado (se puede repetir)
    private void OnEnable()
    {
        
    }
    // 1 sola vez, util para preparar otras entidades dependientes. Se ejecuta despues del awake y onenable
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
