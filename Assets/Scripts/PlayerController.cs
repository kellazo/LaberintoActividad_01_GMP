using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Velocidad de avance y giro. Ajustables desde el Inspector.
    [Tooltip("Factor multiplicador para la velocidad de movimiento hacia adelante/atrás.")]
    [SerializeField] private float moveSpeed; //= 3.0f;
    [Tooltip("Factor multiplicador para la gravedad de movimiento hacia abajo (y)")]
    [SerializeField] private float gravitySpeed; //= -9.81f;
    [Tooltip("Factor multiplicador para la velocidad de rotación (giro) Grados por segundo.")]
    [SerializeField] private float rotateSpeed; //= 120.0f; // grados por segundo
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;
    private float velocidadVertical;

    [Tooltip("Velocidad extra que se añade durante el impulso.")]
    [SerializeField] private float boostSpeed = 10.0f;
    [Tooltip("Duración del impulso en segundos.")]
    [SerializeField] private float boostDuration = 0.5f;
    // Timer para el impulso
    private float boostTimer = 0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boostClip;

    //[SerializeField] private TMPro.TextMeshProUGUI CubosConteo; // Referencia al Texto que muestra el conteo de cubos
    //[SerializeField] private TMPro.TextMeshProUGUI TextoVictoria;   // Referencia al Texto que muestra el mensaje de victoria

    // Referencia al CharacterController
    private CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos el componente CharacterController del mismo GameObject
        controller = GetComponent<CharacterController>();

        // Inicializar el texto
        //UpdateCubesText();
        // Asegurarnos que el texto de victoria esté vacío o desactivado al inicio
        //if (TextoVictoria != null)
        //    TextoVictoria.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // movimiento de un cuerpo de forma cinemática (sin fisicas)

        // getAxis tiene un impulso hasta llegar a 1 o -1 y GetAxisRaw es el absoluto.
        float hInput = Input.GetAxisRaw("Horizontal"); // A/D o Flechas Izq/Der  -1, 0, 1
        float vInput = Input.GetAxisRaw("Vertical");     // W/S o Flechas Arriba/Abajo -1, 0 , 1

        // Usamos Time.deltaTime para que la rotación no dependa del framerate. Y rotamos al personaje en el eje Y
        transform.Rotate(0, hInput * rotateSpeed * Time.deltaTime, 0);

        // Creamos un vector de movimiento local: hacia adelante lo que indica "vertical" y normalizamos (vector unitario) para que la diagonal entre "dentro del círculo"
        //Vector3 moveDirection = new Vector3(hInput, 0, vInput).normalized;
        // Crear vector de movimiento local hacia adelante/atrás según vInput
        Vector3 moveDirection = new Vector3(0, 0, vInput).normalized;
        // transform.Translate(moveDirection * moveSpeed * Time.deltaTime); //Manera en que lo ha dado en clase Fernando, es lo mismo que transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // Transformar este vector local al espacio global teniendo en cuenta la rotación actual del personaje.
        moveDirection = transform.TransformDirection(moveDirection);

        // Multiplicamos por la velocidad de movimiento y por deltaTime para que el desplazamiento sea consistente.
        //moveDirection *= moveSpeed * Time.deltaTime;
        //moveDirection *= moveSpeed; //* Time.deltaTime;

        // Actualizar boostTimer
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
        }

        // Detectar pulsación de espacio para el impulso
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boostTimer = boostDuration;
            // Reproducir el sonido del impulso si tienes el audioSource y el boostClip asignados
            if (audioSource != null && boostClip != null)
            {
                audioSource.PlayOneShot(boostClip);
            }
        }

        if (controller.isGrounded)
        {
            velocidadVertical = 0f;
        }
        else
        {
            velocidadVertical += gravitySpeed * Time.deltaTime;
        }

        // Añadir velocidad base
        float currentSpeed = moveSpeed;
        // Si hay impulso activo, añadir boostSpeed
        if (boostTimer > 0)
        {
            currentSpeed += boostSpeed;
        }
        Vector3 finalMove = moveDirection * currentSpeed * Time.deltaTime;
        finalMove.y = velocidadVertical * Time.deltaTime;

        controller.Move(finalMove);

        if (vInput != 0 && footstepSource.isPlaying == false)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
            footstepSource.Play();
        }
        else if (vInput == 0 && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }

        // Mover al personaje sumando el movimiento horizontal (avances/rotaciones) y la velocidad vertical (gravedad)
        //Vector3 finalMove = (moveDirection + movimientoVertical) * Time.deltaTime;
        // Aplicar el movimiento al CharacterController
        //controller.Move(moveDirection);

        // Añadir el componente vertical al movimiento final(convertido a vector)
        //Vector3 finalMove = new Vector3(moveDirection.x, velocidadVertical * Time.deltaTime, moveDirection.z);

        //controller.Move(finalMove);

        //AplicarGravedad();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CuboOro"))
        {
            // He tocado algo (solo pasa 1 vez en el primer ciclo de fisicas) no en el primer frime (diferenciar entre el update y el ciclo de fisicas )
            Destroy(other.gameObject);
            //cubosRecogidos++;
            //UpdateCubesText();
            // Notificar a ControlJuego que se ha recogido un cubo
            FindObjectOfType<ControlJuego>().CapturaCubo();
        }

        if (other.gameObject.CompareTag("SalidaFinal"))
        {
            // Comprobar si hemos recogido todos los cubos
            /*if (cubosRecogidos >= cubosTotales)
            {
                GanarPartida();
            }*/
            FindObjectOfType<ControlJuego>().CheckVictoria();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // se activa mientras estas dentro de la entidad que estas en contacto
    }
    private void OnTriggerExit(Collider other)
    {
        // Dejo de tocar algo   (es el ultimo ciclo de fisicas en la que etuvistes en contacto se va a ejecutar
    }
    private void AplicarGravedad()
    {
        //movimientoVertical.y += gravitySpeed * Time.deltaTime; // es como el impulso del salto.
        //controller.Move(movimientoVertical * Time.deltaTime); // hay dos time delta time porque la aceleracion es metros / por segundos al cuadrado
    }
    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}
