using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaSecuencia : MonoBehaviour
{
    [Header("Referencia a GameManagerSO")]
    [SerializeField] private GameManagerSO gM;

    [Header("Secuencia de botones permitida")]
    [Tooltip("IDs de los botones en el orden correcto (ej: [2,1,3])")]
    [SerializeField] private int[] sequence;

    [Header("Botones a controlar (arrástralos en Inspector)")]
    [SerializeField] private BotonCombinacion[] botones;

    [Header("Movimiento de la puerta")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceToMove = 3f; // hasta dónde bajará

    [Header("Colores de feedback")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;

    [Header("Sonido de apertura de puerta")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorOpenClip; // Sonido al abrir puerta

    private int currentSequenceIndex = 0; // Siguiente botón que esperamos
    private bool abrir = false;
    private float movedDistance = 0f;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        // Nos suscribimos al evento del GameManager
        gM.OnBotonPulsado += BotonPulsado;
    }

    private void OnDisable()
    {
        // Desuscribirse para evitar problemas
        gM.OnBotonPulsado -= BotonPulsado;
    }

    private void Start()
    {
        // Guardamos la pos. inicial para mover la puerta
        initialPosition = transform.position;

        // Al inicio, ponemos todos los botones a color blanco
        foreach (var b in botones)
        {
            b.CambiarColor(defaultColor);
        }
    }

    private void Update()
    {
        // Movimiento de la puerta si "abrir" está activado
        if (abrir)
        {
            // Mover la puerta hacia atras a 'speed' unidades/seg.
            float step = speed * Time.deltaTime;
            transform.Translate(Vector3.back * step, Space.World);

            movedDistance += step;
            if (movedDistance >= distanceToMove) // Parar tras cierta distancia
            {
                abrir = false;  // Paramos
            }
        }
    }
    // Llamado cuando se pulsa un botón. Verificamos si es el que esperamos
    // en la secuencia. Si se completa la secuencia, abrimos la puerta.
    private void BotonPulsado(int botonID)
    {
        // Si el ID coincide con el que esperamos
        if (botonID == sequence[currentSequenceIndex])
        {
            // Cambiamos el color a "correcto"
            var botonAcertado = BuscarBotonPorID(botonID);
            if (botonAcertado != null)
            {
                // 1) Color a "correctColor"
                botonAcertado.CambiarColor(correctColor);

                // 2) Permitir que se MUEVA 
                botonAcertado.SetOpening(true);

                // 3) Sonido "acierto" del botón
                botonAcertado.PlayCorrectSound();
            }                

            // Pasamos al siguiente de la secuencia
            currentSequenceIndex++;

            // ¿Llegamos al final de la secuencia?
            if (currentSequenceIndex >= sequence.Length)
            {
                // Abrir la puerta
                abrir = true;
                movedDistance = 0f; // reiniciamos para mover la puerta

                // Reproducir sonido de puerta abriéndose
                if (doorAudioSource != null && doorOpenClip != null)
                {
                    doorAudioSource.PlayOneShot(doorOpenClip);
                }

                Debug.Log("Secuencia completada. Puerta abriéndose...");
            }
        }
        else
        {
            // Orden incorrecto: Reiniciamos índice
            currentSequenceIndex = 0;
                
            Debug.Log("Orden incorrecto. Secuencia reiniciada.");

            // A todos los botones:
            foreach (var b in botones)
            {
                b.SetOpening(false);            // Detener movimiento
                b.ResetPosition();             // Vuelve a pos original
                b.CambiarColor(defaultColor);  // Color = blanco
                b.PlayErrorSound();            // Sonido de error
            }
        }
    }
    private BotonCombinacion BuscarBotonPorID(int id)
    {
        foreach (var b in botones)
        {
            if (b.Id == id)
            {
                return b;
            }
        }
        return null;
    }
}
