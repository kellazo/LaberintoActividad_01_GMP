using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboOro : MonoBehaviour
{
    [SerializeField] private float velocidadRotacion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Vector3 direccionRotacion = Vector3.up; //  es lo mismo que Vector3 direccionRotacion = new Vector3(0, 1, 0);
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime, Space.World); // rotacion en la cordenada Y positiva global (self por defecto es local)
    }

    private void OnTriggerEnter(Collider other)
    {
       // He tocado algo (solo pasa 1 vez en el primer ciclo de fisicas) no en el primer frime (diferenciar entre el update y el ciclo de fisicas )
    }
    private void OnTriggerStay(Collider other)
    {
        // se activa mientras estas dentro de la entidad que estas en contacto
    }
    private void OnTriggerExit(Collider other)
    {
        // Dejo de tocar algo   (es el ultimo ciclo de fisicas en la que etuvistes en contacto se va a ejecutar
    }
}
