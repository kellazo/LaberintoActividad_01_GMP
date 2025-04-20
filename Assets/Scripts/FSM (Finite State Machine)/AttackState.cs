using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
   // [SerializeField]
   // private float timeBetweenAttacks;

    [SerializeField]
    private float baseAttackDamage;

   // private float timer;

    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller); // Primero paso en dicha inicializacion y luego en cada estado haz lo que quieras
        Debug.Log("Empiezo a atacaar!");
     //   timer = timeBetweenAttacks;

        //controller.Agent.isStopped = true;
        controller.Agent.stoppingDistance = controller.AttackDistance;
        controller.Anim.SetBool("attacking", true);
    }
    public override void OnUpdateState()
    {
        FaceTarget();
        //controller.Agent.SetDestination(controller.Target.position);

        // Si tengo el player en rango de ataque...
       /* if (!controller.Agent.pathPending && controller.Agent.remainingDistance < controller.Agent.stoppingDistance)
        {

            timer += Time.deltaTime;
            if (timer >= timeBetweenAttacks)
            {
                Debug.Log("Hago Daño!");
                timer = 0f;
            }
        }
        else
        {
            controller.ChangeState(controller.PatrolState);
        }*/

    }

    private void FaceTarget() // Asegurarme que el enemy enfoca al player en todo momento. 
    {
        // hacemos trigonometria de 2 vectores. Sacas la direccion al objetivo,
        // la direccion de un puto a a un punto b siempre es destino menos origen (b-a) i normalizo, vector unitario pk me importa la direccion no la diferencia de distancia
        Vector3 directionToTarget = (controller.Target.transform.position - transform.position).normalized;
        // te doy una rotacion basandome en esa direccion
        directionToTarget.y=0f; // ponerlo plano para que si subes que no se ponga al suelo
        transform.rotation = Quaternion.LookRotation(directionToTarget); // transforma una direccion en una rotación.
    }

    public override void OnExitState()
    {
        // cuando se termine la animación
    }

    // Se ejecuta cuando SE TERMINA la animación de atacar.
    public void OnFinishAttackAnimation()
    {
        // se nos ha escapado el jugador de nuestro rango de ataque...
        if(Vector3.Distance(transform.position, controller.Target.transform.position) > controller.AttackDistance)
        {
            controller.Anim.SetBool("attacking", false);
            controller.ChangeState(controller.ChaseState);
        }
    }

}
