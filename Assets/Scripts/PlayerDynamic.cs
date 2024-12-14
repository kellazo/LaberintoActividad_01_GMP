using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDynamic : MonoBehaviour
{
    [SerializeField] private float fuerza;
    //[SerializeField] private float fuerzaSalto;
    [SerializeField] private float ratioCargaSalto;
    private float fuerzaSalto;
    private Rigidbody rb;
    private float hInput, vInput;
    private float fuerzaSaltoMaxima;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // los getcomponents llamarlos los minimo posible. Procurar que no se ejecute continuamente
    }

    // Update is called once per frame
    void Update()
    {
        
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        // salto simple
        /* if (Input.GetKeyDown(KeyCode.Space)) // En el primer frame de la tecla espacio saltar con impulse en la Y
         {
             rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
         }*/
        //salto acumulado
        if (Input.GetKey(KeyCode.Space)) // mientras mantengo el espacio
        {
            fuerzaSalto += ratioCargaSalto * Time.deltaTime; // por segundo cuando tu sumas algo, la fuerza de salto se va cargando a 3 por segundo
            fuerzaSalto = Mathf.Min(fuerzaSalto, fuerzaSaltoMaxima);
            /*if(fuerzaSalto > fuerzaSaltoMaxima)
            {
                fuerzaSalto = fuerzaSaltoMaxima;
            }*/
        }
        if (Input.GetKeyUp(KeyCode.Space)) // Cuando sueltas el espacio
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse); // haces el salto de impulso y dejas la fuerza a 0 importante
            fuerzaSalto = 0f;
        }


    }
    // Ciclo de fisicas: Es donde se gestinonan los calculos físicos. // SOLO fisicas continuas, que necesitan cálculos a lo largo de un tiempo.
    // Update vs Fixed: SON INDEPENDIENTES. Es fijo, pk se ejecuta en un ratio constante. Por tanto no afecta los frames(delta time). A que ratio va? (cada cuanto se ejecuta?) En Project Settings Time Fixed Timestep

    private void FixedUpdate() // solo meter físicas continuas (todo lo que se necesita recalcular en el tiempo) El force si, el impulse no. 
    {
        // movimiento de un cuerpo de forma dinamica (fisicas)
        rb.AddForce(new Vector3(hInput, 0, vInput).normalized * fuerza, ForceMode.Force); // aqui no podemos meter el delta time que es por segundo pero la fuerza es en Newtons.
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("CuboOro"))
        {
            Destroy(other.gameObject);
        }
    }
}
