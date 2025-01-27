using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    private bool abrir = false;
    [Header("Movimiento de la puerta")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceToMove = 3f; // hasta dónde se moverá

    [Header("Sonido de apertura de puerta")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorOpenClip; // Sonido al abrir puerta

    private float movedDistance = 0f;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        gM.OnBotonPulsadoSolo += Activar;
    }
    private void OnDisable()
    {
        // Desuscribirse para evitar problemas
        gM.OnBotonPulsadoSolo -= Activar;
    }

    private void Activar(int id)
    {
        if (this.id == id)
        {
            abrir = true;
            // Reproducir sonido de puerta abriéndose
            if (doorAudioSource != null && doorOpenClip != null)
            {
                doorAudioSource.PlayOneShot(doorOpenClip);
            }

        }
    }

    void Start()
    {
        // Guardamos la pos. inicial para mover la puerta
        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (abrir)
        {
            float step = speed * Time.deltaTime;
            transform.Translate(Vector3.down * step, Space.World);

            movedDistance += step;
            if (movedDistance >= distanceToMove) // Parar tras cierta distancia
            {
                abrir = false;  // Paramos
            }

            
            
        }
    }
}
