using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D o Flechas Izq/Der
        float vertical = Input.GetAxis("Vertical");     // W/S o Flechas Arriba/Abajo

        // Usamos Time.deltaTime para que la rotación no dependa del framerate.
        transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

        // Creamos un vector de movimiento local: hacia adelante lo que indica "vertical".
        Vector3 moveDirection = new Vector3(0, 0, vertical);

        // Transformar este vector local al espacio global teniendo en cuenta la rotación actual del personaje.
        moveDirection = transform.TransformDirection(moveDirection);

        // Multiplicamos por la velocidad de movimiento y por deltaTime para que el desplazamiento sea consistente.
        moveDirection = moveDirection * moveSpeed * Time.deltaTime;

        // Aplicar el movimiento al CharacterController
        controller.Move(moveDirection);
    }
}
