using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlJuego : MonoBehaviour
{
    //[SerializeField] private GameObject prefabCubo;
    [SerializeField] private int totalCubos;
    //public const float radioUniverso = 5f;
    //public const float factorVelocidad = 0.02f;
    //public const float velocidadInicial = 1f;
    private int cubosCapturados;
    [SerializeField] private TMP_Text textoMensaje;
    [SerializeField] private TMP_Text textoTiempo;
    private DateTime inicioJuego;
    private TimeSpan tiempoJuego;
    private bool juegoTerminado = false;

    void Awake()
    {
        Debug.Log("ControlJuego.Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ControlJuego.Start");
        inicioJuego = DateTime.Now;
        ActualizarMensaje();

        //CuboOro cubo = prefabCubo.GetComponent<CuboOro>();

    }

    // Update is called once per frame
    void Update()
    {
        // Mientras queden cubos por capturar y el juego no haya terminado, mostrar el tiempo transcurrido
        if (cubosCapturados < totalCubos && !juegoTerminado)
        {
            tiempoJuego = DateTime.Now - inicioJuego;
            textoTiempo.text = tiempoJuego.ToString("mm\\:ss");
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
            textoMensaje.text = "Todos los cubos capturados. Encuentra la salida.";
        }
    }

    private void MostrarVictoria()
    {
        textoMensaje.text = "¡Has ganado! Has recogido todos los cubos y salido del laberinto.";
        // falta parar tiempo, movimiento jugador y boton pulsar para resetear.
    }
}
