using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
    [SerializeField]
    private Transform ruta;

    [SerializeField]
    private float tiempoDeEspera; 

    [SerializeField]
    private float patrolVelocity;

    private List<Vector3> puntosDeRuta = new List<Vector3>();
    private int indicePuntoActual = 0; // marca el indice de la lista

    private Vector3 destinoActual; // Marca coordenada mi destino actual

    //private int idWalking;
    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller); // Primero paso en dicha inicializacion y luego en cada estado haz lo que quieras
        Debug.Log("Empiezo a patrullar!");
        foreach (Transform punto in ruta)
        {
            puntosDeRuta.Add(punto.position);
        }
        //agent = GetComponent<NavMeshAgent>();
        //controller.Agent.SetDestination;

        //idWalking = Animator.StringToHash("walking");
        destinoActual = puntosDeRuta[indicePuntoActual]; // marco el primer destino

        
        controller.Agent.stoppingDistance = 0f;
        controller.Agent.speed = patrolVelocity; // tu limite es patrolVelocity
       // controller.Agent.acceleration = 8f; //magic number

        StartCoroutine(PatrullarYEsperar()); // me pongo a patrullar

    }
    public override void OnUpdateState()
    {
        controller.Anim.SetFloat("velocity", controller.Agent.velocity.magnitude / controller.MaximumVelocity); // velocidad actual que lleva el agente en todo momento / velocidad maxima


        // CONO DE VISION
        Collider[] collsDetectados = Physics.OverlapSphere(transform.position, controller.RangoVision, controller.QueEsTarget);
        if (collsDetectados.Length > 0) // Hay 1 target dentro del rango (1r paso)
        {
            Vector3 direccionATarget = (collsDetectados[0].transform.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, direccionATarget, controller.RangoVision, controller.QueEsObstaculo)) //  (2n paso)
            {
                if (Vector3.Angle(transform.forward, direccionATarget) <= controller.AnguloVision / 2)
                {
                    controller.Target = collsDetectados[0].transform;
                    controller.ChangeState(controller.ChaseState);
                }
            }
        }
    }
    public override void OnExitState()
    {
        StopAllCoroutines();
    }
    private IEnumerator PatrullarYEsperar()
    {
        while (true)
        {
            controller.Agent.SetDestination(destinoActual); // Voy yendo al destino
           // controller.Anim.SetBool(idWalking, true);
            yield return new WaitUntil( () => !controller.Agent.pathPending && controller.Agent.remainingDistance <= 0.2f);
            //controller.Anim.SetBool(idWalking, false); 
            yield return new WaitForSeconds(tiempoDeEspera); // Me espero en dicho punto.
            CalcularNuevoDestino();
        }
    }
    private void CalcularNuevoDestino()
    {
        indicePuntoActual++; // Avanzamos uno.
        indicePuntoActual = indicePuntoActual % (puntosDeRuta.Count); // me aseguro de no salirme de los puntos máximos
        destinoActual = puntosDeRuta[indicePuntoActual]; // actualizo mi destino actual
    }
}
