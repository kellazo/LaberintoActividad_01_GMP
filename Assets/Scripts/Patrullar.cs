using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrullar : MonoBehaviour
{
    [SerializeField]
    private Transform ruta;
    [SerializeField]
    private float rangoVision; // distancia de vision
    [SerializeField]
    private LayerMask queEsTarget; //PAra el enemigo que es para el un objetivo
    [SerializeField]
    private LayerMask queEsObstaculo; 
    [SerializeField]
    private float anguloVision; // cuanto esta el cono de abierto
    // Start is called before the first frame update
    private List<Vector3> puntosDeRuta =  new List<Vector3>();
    private NavMeshAgent agent;
    private int indicePuntoActual = 0; // marca el indice de la lista
    private Vector3 destinoActual; // Marca coordenada mi destino actual
    [SerializeField]
    private float tiempoDeEspera; // se puede randomizar para que sea mas organico (tiempo de espera entre punto y punto)
    private void Awake()
    {
        foreach (Transform punto in ruta)
        {
            puntosDeRuta.Add(punto.position);
        }
        agent = GetComponent<NavMeshAgent>();

        destinoActual = puntosDeRuta[indicePuntoActual]; // marco el primer destino
        StartCoroutine(PatrullarYEsperar()); // me pongo a patrullar
    }

    private IEnumerator PatrullarYEsperar()
    {
        while (true)
        {
            // ir a un punto en concreto y esperare x tiempo y yre a otro punto
            agent.SetDestination(destinoActual); // voy yendo al destino 
           
            // yield return new WaitUntil(HeLlegado); // Esperate en este punto hasta que hayas llegado
            yield return new WaitUntil( () => !agent.pathPending && agent.remainingDistance <= 0.2f); // expresion lambda, te ahorras lo comentado de abajo. funcion anonima. | me espero  en este punto hasta que llegue. Tambien mira que no tenga calculos pendientes, ya qu eel navmesh el calculo es complejo y le cuesta.
          //  yield return new WaitForSeconds(tiempoDeEspera); // me espero en dicho punto.  
            CalcularNuevoDestino();
            
        }
    }
   // private bool HeLlegado()
   // {
        /*if(agent.remainingDistance <= 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
        */
     //   return agent.remainingDistance <= 0.2;
   // }

    private void CalcularNuevoDestino()
    {
        
        // ya estoy listo y calculo nuevo destino
        indicePuntoActual++; // avanzamos uno
        /*if(indicePuntoActual >= puntosDeRuta.Count)
        {
            indicePuntoActual = 0;
        }*/ // es lo mismo

        indicePuntoActual = indicePuntoActual % (puntosDeRuta.Count); // me aseguro de no salirme de los puntos máximos

        destinoActual = puntosDeRuta[indicePuntoActual]; // actualizo mi destino actual
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Collider[] collsDetectados = Physics.OverlapSphere(transform.position, rangoVision, queEsTarget);
        if(collsDetectados.Length > 0 ) // Hay 1 target dentro del rango (1r paso)
        {
            // vamos a detectar si hay algun obstaculo en esa dirección en la que hemos detectado el target
            Vector3 direccionATarget = (collsDetectados[0].transform.position - transform.position).normalized; // destino menos origen para saber la dirección y normalizas haciendo el vector unitario.

            // miramos si en esa dirección desde mi posicion(enemgio) y en dirección al objetivo con un angulo de vision, miro si hay obstaculo pero en negativo !, es decir vira a ver que NO haya ningun obstaculo
            if(!Physics.Raycast(transform.position, direccionATarget, rangoVision, queEsObstaculo)) //  (2n paso)
            {
                // Resumen hasta aqui: si yo se donde estas y trazo una linea en tu direccion no quiero que haya ningun obstaculo 

                // falta saber si estas dentro del cono (rango vision)
                if(Vector3.Angle(transform.forward, direccionATarget) <= anguloVision/2)  // calculo el angulo entre 2 vectores y verifico, es entre el frontal, la direccion a la que miro y la dirección hacia el objetivo
                {
                    this.enabled = false;
                }


            }

            // vamos a detectar solo el primero, pk el juego es singleplayer
           // collsDetectados[0].
        }
    }
}
