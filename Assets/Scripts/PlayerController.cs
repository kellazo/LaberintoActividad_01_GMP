using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


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
    [SerializeField] private float mouseSensitivity = 100f; // Sensibilidad del ratón
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource pickUpSource;
    [SerializeField] private AudioClip pickUpClip;
    [Tooltip("Distancia detección del raycast hit")]
    [SerializeField] private float distanciaDeteccionInteractuable;
    [SerializeField] private float distanciaDeteccionSuelo;
    private float velocidadVertical;
    private bool isGameOver = false;

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

    [Header("Salud del jugador")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Tooltip("Referencia al CanvasManager (para actualizar la barra de vida).")]
    [SerializeField] private CanvasManager canvasManager;

    [Header("Audio para Game Over")]
    [SerializeField] private AudioSource gameOverAudioSource; // Un AudioSource dedicado
    [SerializeField] private AudioClip gameOverClip;          // El sonido de GameOver

    // Referencia al CharacterController
    private CharacterController controller;

    private PlayerInput input;

    private Vector2 inputMovement;


    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        input.actions["Jump"].started += Jump;
        input.actions["Movement"].performed += UpdateMovement;
        input.actions["Movement"].canceled += CancelMovement;
    }

    private void CancelMovement(InputAction.CallbackContext ctx)
    {
        inputMovement = Vector2.zero;
    }

    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        inputMovement = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        input.actions["Jump"].started -= Jump;
        input.actions["Movement"].performed -= UpdateMovement;
        input.actions["Movement"].canceled -= CancelMovement;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //if (Physics.Raycast(this.transform.position, transform., distanciaDeteccionSuelo))
        if (Physics.Raycast(transform.position, Vector3.down, distanciaDeteccionSuelo))
        {
            Debug.DrawRay(transform.position, Vector3.down * distanciaDeteccionSuelo, Color.red, 3);
            //rb.AddForce(Vector3.up.normalized * fuerzaSalto, ForceMode.Impulse);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        // Obtenemos el componente CharacterController del mismo GameObject
        controller = GetComponent<CharacterController>();
        // Opcional: Para bloquear el cursor en pantalla
        Cursor.lockState = CursorLockMode.Locked;
        // Inicializar el texto
        //UpdateCubesText();
        // Asegurarnos que el texto de victoria esté vacío o desactivado al inicio
        //if (TextoVictoria != null)
        //    TextoVictoria.gameObject.SetActive(false);

        // Iniciar salud al máximo
        currentHealth = maxHealth;
        // Actualizar la barra por primera vez
        if (canvasManager != null)
        {
            canvasManager.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // movimiento de un cuerpo de forma cinemática (sin fisicas)

        // getAxis tiene un impulso hasta llegar a 1 o -1 y GetAxisRaw es el absoluto.
        float hInput = Input.GetAxisRaw("Horizontal"); // A/D o Flechas Izq/Der  -1, 0, 1
        float vInput = Input.GetAxisRaw("Vertical");     // W/S o Flechas Arriba/Abajo -1, 0 , 1

        // Usamos Time.deltaTime para que la rotación no dependa del framerate. Y rotamos al personaje en el eje Y
       // transform.Rotate(0, hInput * rotateSpeed * Time.deltaTime, 0);

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

        //Si doy a la e...
        if (Input.GetKeyDown(KeyCode.E))
        {
            // lanzo un raycast
            if (Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hit, distanciaDeteccionInteractuable))
            {
                // si impacto en el botón
                if (hit.transform.TryGetComponent(out Boton boton))
                {
                    // interactua con el botón
                    boton.Interactuar();
                }
                else if (hit.transform.TryGetComponent(out BotonCombinacion botonCombinacion))
                {
                    // interactua con el botón de secuencia
                    botonCombinacion.Interactuar();
                }
            }
        }

        //  GRAVEDAD (por un CharacterController)
        if (controller.isGrounded)
        {
            velocidadVertical = 0f;
        }
        else
        {
            velocidadVertical += gravitySpeed * Time.deltaTime;
        }

        // El movimiento final en Y (gravedad)
        moveDirection.y = velocidadVertical;

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

        // ROTACIÓN CON EL RATÓN EN EJE Y(horizontal)
        float mouseX = Input.GetAxis("Mouse X");        // Movimiento horizontal del ratón
        float yaw = mouseX * mouseSensitivity * Time.deltaTime;
        //transform.Rotate(0, hInput * rotateSpeed * Time.deltaTime, 0);
        transform.Rotate(Vector3.up, yaw);

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

        // Si estamos en Game Over y pulsa Intro, cargar menú principal
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;
            var allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (var src in allAudioSources)
            {
                src.enabled = true;
            }


        }
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

            // Reproducir el sonido de recogida de cubo
            if (pickUpSource != null && pickUpClip != null)
            {
                pickUpSource.PlayOneShot(pickUpClip);
            }
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

        // Recibir daño si es una trampa (por ejemplo, un hacha con tag "Hacha")
        if (other.gameObject.CompareTag("Hacha"))
        {
            // Por ejemplo, 10 de daño
            TakeDamage(10f);
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

    public void TakeDamage(float damage)
    {
        if (isGameOver) return; // Ya está en game over, no procesar más
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Actualizar la barra de vida
        if (canvasManager != null)
        {
            canvasManager.UpdateHealthBar(currentHealth, maxHealth);
            canvasManager.ShowDamageFeedback(); // <-- Overlay + Sonido de dolor
        }

        // Si la vida llegó a 0, Game Over
        if (currentHealth <= 0)
        {
            GameOver();
            Debug.Log("Game Over!");
            // Aquí podrías desactivar controles del jugador, mostrar panel de GameOver, etc.
        }
    }
    public void StopFootsteps()
    {
        if (footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
    private void GameOver()
    {
        isGameOver = true;
        //Reproduce el sonido de Game Over
        if (gameOverAudioSource != null && gameOverClip != null)
        {
            gameOverAudioSource.PlayOneShot(gameOverClip);

            // Iniciamos una corutina que, tras reproducir el sonido, desactiva el resto de audios
            StartCoroutine(StopAllSoundsAfterClip(gameOverClip.length));
        }
        else
        {
            // Si falta audioSource o gameOverClip, simplemente desactiva todo
            StopAllSounds();
        }
        // Avisamos al CanvasManager para que muestre la pantalla de Game Over
        canvasManager.ShowGameOverScreen();
    }
    private IEnumerator StopAllSoundsAfterClip(float clipLength)
    {
        // Esperamos el tiempo que dura el clip
        yield return new WaitForSeconds(clipLength);

        // Desactivamos todos los sonidos
        StopAllSounds();
    }
    private void StopAllSounds()
    {
        //Pausar todo el audio del juego
        //AudioListener.pause = true;

        var allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var src in allAudioSources)
        {
            if (src != gameOverAudioSource)
                src.enabled = false;
        }


    }
}
