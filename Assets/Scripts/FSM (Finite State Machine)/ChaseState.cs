using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState : State<EnemyController>
{
    //[SerializeField]
    //private float chaseVelocity;

    [SerializeField]
    private float timeBeforeBackToPatrol;

    private Coroutine coroutine;
    
    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller); // Primero paso en dicha inicializacion y luego en cada estado haz lo que quieras

        Debug.Log("Empiezo a perseguir!!");

        
        controller.Agent.stoppingDistance = controller.AttackDistance;
        controller.Agent.speed = controller.MaximumVelocity; // tu limite es este 
        //controller.Agent.acceleration = 10000; // Para que siempre a la misma distancia de mi. 

    }
    public override void OnUpdateState()
    {
        controller.Anim.SetFloat("velocity", controller.Agent.velocity.magnitude / controller.MaximumVelocity); // velocidad actual que lleva el agente en todo momento / velocidad maxima
        // Sólo si el objetivo es alcanzable porque esta dentro de mi maya
        if (!controller.Agent.pathPending && controller.Agent.CalculatePath(controller.Target.position, new NavMeshPath()))
        {
            StopMyCoroutine();

            controller.Agent.SetDestination(controller.Target.position);

            // No tengo calculos pendientes y mi distancia hacia mi objetivo esta por debajo de mi distancia de parada.
            if (!controller.Agent.pathPending && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            {
                
                controller.ChangeState(controller.AttackState);
            }
        }
        else
        {
            //controller.ChangeState(controller.PatrolState);
            coroutine ??= StartCoroutine(StopandReturn()); // doble assignacion, mira si la corrutina esta nula y si esta nula le das un valor ( ??= operador de fusion nula)
            
        }
    }

    private void StopMyCoroutine() //con CTRL+R+M con codigo seleccionado creas el metodo
    {
        //StopCoroutine(coroutine);
        StopAllCoroutines();    
        coroutine = null;
    }

    private IEnumerator StopandReturn()
    {
        yield return new WaitForSeconds(timeBeforeBackToPatrol);
        controller.ChangeState(controller.PatrolState);
    }
    public override void OnExitState()
    {
        StopMyCoroutine();
    }
}
