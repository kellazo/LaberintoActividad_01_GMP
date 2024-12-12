using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Velocidad de avance y giro. Ajustables desde el Inspector.
    [Tooltip("Factor multiplicador para la velocidad de movimiento hacia adelante/atrás.")]
    public float moveSpeed = 3.0f;

    [Tooltip("Factor multiplicador para la velocidad de rotación (giro).")]
    public float rotateSpeed = 120.0f; // grados por segundo

    // Referencia al CharacterController
    private CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos el componente CharacterController del mismo GameObject
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {   // getAxis tiene un impulso hasta llegar a 1 o -1 y GetAxisRaw es el absoluto.
        float hInput = Input.GetAxis("Horizontal"); // A/D o Flechas Izq/Der  -1, 0, 1
        float vInput = Input.GetAxis("Vertical");     // W/S o Flechas Arriba/Abajo -1, 0 , 1

        // Usamos Time.deltaTime para que la rotación no dependa del framerate.
        transform.Rotate(0, hInput * rotateSpeed * Time.deltaTime, 0);

        // Creamos un vector de movimiento local: hacia adelante lo que indica "vertical" y normalizamos (vector unitario) para que la diagonal entre "dentro del círculo"
        Vector3 moveDirection = new Vector3(hInput, 0, vInput).normalized;
        // transform.Translate(moveDirection * moveSpeed * Time.deltaTime); //Manera en que lo ha dado en clase Fernando, es lo mismo que transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // Transformar este vector local al espacio global teniendo en cuenta la rotación actual del personaje.
        moveDirection = transform.TransformDirection(moveDirection);

        // Multiplicamos por la velocidad de movimiento y por deltaTime para que el desplazamiento sea consistente.
        moveDirection = moveDirection * moveSpeed * Time.deltaTime;

        // Aplicar el movimiento al CharacterController
        controller.Move(moveDirection);
    }
    private void OnTriggerEnter(Collider other)
    {
        // He tocado algo (solo pasa 1 vez en el primer ciclo de fisicas) no en el primer frime (diferenciar entre el update y el ciclo de fisicas )
        Destroy(other.gameObject);
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
