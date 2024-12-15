using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlJuego : MonoBehaviour
{
    [SerializeField] private GameObject prefabCubo;
    [SerializeField] private int totalCubos;
    //public const float radioUniverso = 5f;
    //public const float factorVelocidad = 0.02f;
    //public const float velocidadInicial = 1f;
    private int cubosCapturados;
    [SerializeField] private TMP_Text textoMensaje;
    [SerializeField] private TMP_Text textoTiempo;
    private System.DateTime inicioJuego;
    private System.TimeSpan tiempoJuego;

    void Awake()
    {
        Debug.Log("ControlJuego.Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ControlJuego.Start");
        inicioJuego = System.DateTime.Now;

        CuboOro cubo = prefabCubo.GetComponent<CuboOro>();


       

    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void CapturaCubo()
    {
        cubosCapturados++;
    }
}
