using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrullar : MonoBehaviour
{
    [SerializeField]
    private Transform ruta;
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
            Debug.Log("aaaaaaaa");
            // yield return new WaitUntil(HeLlegado); // Esperate en este punto hasta que hayas llegado
            yield return new WaitUntil( () => !agent.pathPending && agent.remainingDistance <= 0.2f); // expresion lambda, te ahorras lo comentado de abajo. funcion anonima. | me espero  en este punto hasta que llegue. Tambien mira que no tenga calculos pendientes, ya qu eel navmesh el calculo es complejo y le cuesta.
          //  yield return new WaitForSeconds(tiempoDeEspera); // me espero en dicho punto.  
            CalcularNuevoDestino();
            Debug.Log("cccccccc");
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
        Debug.Log("bbbbbbbbb");
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
}
