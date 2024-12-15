using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlJuego : MonoBehaviour
{
    [SerializeField] private int totalCubos;
    //public const float radioUniverso = 5f;
    //public const float factorVelocidad = 0.02f;
    //public const float velocidadInicial = 1f;
    private int cubosCapturados;
    [SerializeField] private TMP_Text textoMensaje;
    [SerializeField] private TMP_Text textoTiempo;
    [SerializeField] private TMP_Text textoReiniciar; // Texto que se mostrará al ganar para reiniciar
    private DateTime inicioJuego;
    private TimeSpan tiempoJuego;
    private bool juegoTerminado = false;
    
    [SerializeField] private GameObject jugador; // Referencia al jugador
    [SerializeField] private Vector3 posicionInicialJugador; // Posición inicial del jugador
    [SerializeField] private GameObject prefabCubo;
    [SerializeField] private List<Transform> posicionesCubos; // Lista de posiciones iniciales para reinstanciar los cubos

    // Lista para guardar referencias a los cubos instanciados
    private List<GameObject> cubosInstanciados = new List<GameObject>();
    private List<Vector3> cubosPositions = new List<Vector3>();
    private List<Quaternion> cubosRotations = new List<Quaternion>();



    void Awake()
    {
        Debug.Log("ControlJuego.Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ControlJuego.Start");

        // Guardar posiciones y rotaciones al inicio
        foreach (Transform t in posicionesCubos)
        {
            cubosPositions.Add(t.position);
            cubosRotations.Add(t.rotation);
        }
        // Instanciamos los cubos al inicio
        InstanciarCubos();

        inicioJuego = DateTime.Now;
        ActualizarMensaje();

        if (textoReiniciar != null)
            textoReiniciar.gameObject.SetActive(false);

        //CuboOro cubo = prefabCubo.GetComponent<CuboOro>();

    }

    // Update is called once per frame
    void Update()
    {
        // Mientras el juego no haya terminado, mostrar el tiempo transcurrido
        //if (cubosCapturados < totalCubos &&!juegoTerminado)
        if (!juegoTerminado)
        {
            tiempoJuego = DateTime.Now - inicioJuego;
            textoTiempo.text = tiempoJuego.ToString("mm\\:ss");
        }
        else
        {
            // Si el juego ha terminado, comprobamos si se pulsa espacio para reiniciar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReiniciarJuego();
            }
        }

        /*
        // dependiendo del estado del juego aparece un mensaje u otro
        if (cubosCapturados == totalCubos)
        {
            textoMensaje.text = "Game Over";
        }
        else
        {
            if (cubosCapturados == 0)
            {
                textoMensaje.text = "Objetivo: Capturar todos los cubos y salir del laberinto.";
            }
            else
            {
                textoMensaje.text = "Quedan " + (totalCubos - cubosCapturados) + " cubos";
            }
        }
        // siempre que queden esferas por capturar hay que anotar el tiempo transcurrido.
        // el operador '-' de C# se puede aplicar a los tipos 'DateTime' y 'TimeSpan'
        if (cubosCapturados < totalCubos)
        {
            tiempoJuego = System.DateTime.Now - inicioJuego;
        }
        // se muestra el tiempo en pantalla
        // el método 'ToString' devuelve el valor del tiempoJuego formateado
        textoTiempo.text = tiempoJuego.ToString("mm\\:ss");
        */
    }
    public void CapturaCubo()
    {
        cubosCapturados++;
        ActualizarMensaje();
    }

    public void CheckVictoria()
    {
        // Si ya se han capturado todos los cubos, el jugador gana
        if (cubosCapturados >= totalCubos && !juegoTerminado)
        {
            juegoTerminado = true;
            MostrarVictoria();
        }
    }

    private void ActualizarMensaje()
    {
        if (cubosCapturados == 0)
        {
            textoMensaje.text = "Objetivo: Capturar todos los cubos y salir del laberinto.";
        }
        else if (cubosCapturados < totalCubos)
        {
            textoMensaje.text = "Quedan " + (totalCubos - cubosCapturados) + " cubos.";
        }
        else
        {
            // Todos los cubos capturados, a la espera de que el jugador llegue a la salida
            textoMensaje.text = "Todos los cubos capturados. Dirígete a la salida.";
        }
    }

    private void MostrarVictoria()
    {
        textoMensaje.text = "¡Has ganado! Has recogido todos los cubos y encontrado la salida.";

        // Mostramos mensaje para reiniciar
        if (textoReiniciar != null)
        {
            textoReiniciar.gameObject.SetActive(true);
            textoReiniciar.text = "Pulsa la barra espaciadora para reiniciar el juego.";
        }

        // Deshabilitar el movimiento del personaje
        PlayerController pc = jugador.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.enabled = false;
        }
    }

    private void InstanciarCubos()
    {
        // Instancia los cubos en las posiciones indicadas
        for (int i = 0; i < cubosPositions.Count; i++)
        {
            GameObject nuevoCubo = Instantiate(prefabCubo, cubosPositions[i], cubosRotations[i]);
            cubosInstanciados.Add(nuevoCubo);
        }
    }

    private void DestruirCubos()
    {
        // Destruir todos los cubos existentes para reiniciar su estado
        foreach (GameObject cubo in cubosInstanciados)
        {
            if (cubo != null)
                Destroy(cubo);
        }
        cubosInstanciados.Clear();
    }

    private void ReiniciarJuego()
    {
        // Volver a 0 el conteo
        cubosCapturados = 0;
        juegoTerminado = false;

        // Reiniciar el tiempo
        inicioJuego = DateTime.Now;

        // Colocar al jugador en la posición inicial
        // Mover al jugador a la posición inicial
        jugador.transform.position = posicionInicialJugador;
        jugador.transform.rotation = Quaternion.identity;
        PlayerController pc = jugador.GetComponent<PlayerController>();
        if (pc != null)
            pc.enabled = true;

        // Destruir e instanciar de nuevo los cubos
        DestruirCubos();
        InstanciarCubos();

        // Ocultar el mensaje de reiniciar
        if (textoReiniciar != null)
            textoReiniciar.gameObject.SetActive(false);

        // Actualizar el mensaje inicial
        ActualizarMensaje();
    }
}
