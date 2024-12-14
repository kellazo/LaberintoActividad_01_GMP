using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDynamic : MonoBehaviour
{
    [SerializeField] private float fuerza;
    private Rigidbody rb;
    private float hInput, vInput;
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
        

    }
    // Ciclo de fisicas: Es donde se gestinonan los calculos físicos.
    // Update vs Fixed: SON INDEPENDIENTES. Es fijo, pk se ejecuta en un ratio constante. Por tanto no afecta los frames(delta time). A que ratio va?
    private void FixedUpdate()
    {
        // movimiento de un cuerpo de forma dinamica (fisicas)
        rb.AddForce(new Vector3(hInput, 0, vInput).normalized * fuerza, ForceMode.Force); // aqui no podemos meter el delta time que es por segundo pero la fuerza es en Newtons.
    }
}
