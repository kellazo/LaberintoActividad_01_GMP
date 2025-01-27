using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonCombinacion : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private Renderer meshRenderer;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSource;   // SU propio AudioSource
    [SerializeField] private AudioClip correctClip;     // Sonido si acierta
    [SerializeField] private AudioClip errorClip;       // Sonido si falla

    [Header("IDs y movimiento")]
    [SerializeField] private int id;               // Identificador de este bot�n
    [SerializeField] private float velocity;       // Velocidad de movimiento al pulsar
    [SerializeField] private Transform destination;// Hacia d�nde se mueve

    private bool opening = false;
    private Vector3 originalPosition;

    // Propiedad p�blica para acceder al ID en la Puerta
    public int Id => id;


    // Start is called before the first frame update
    void Start()
    {
        // Guardamos la posici�n inicial para poder restaurarla
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            //CINEMATICO: Sin fisicas
            transform.position = Vector3.MoveTowards(transform.position, destination.position, velocity * Time.deltaTime);

        }
    }
    // Llamado cuando el jugador interact�a con este bot�n.
    public void Interactuar()
    {
        //opening = true;
        // Notificamos al GameManagerSO el ID de este bot�n
        gM.InteractuableEjecutado(id);
    }

    // Cambia el color en el material (Base Map).
    // �til para indicar acierto o restablecer a blanco.

    public void CambiarColor(Color nuevoColor)
    {
        if (meshRenderer != null && meshRenderer.material != null)
        {
            // En URP/HDRP: "_BaseColor". 
            // En Standard (Built-in) podr�a ser "_Color".
            meshRenderer.material.SetColor("_BaseColor", nuevoColor);
        }
    }

    // Devuelve este bot�n a su posici�n original y detiene el movimiento.
    public void ResetPosition()
    {
        transform.position = originalPosition;
        opening = false;
    }

    // Activa o desactiva el movimiento del bot�n.
    public void SetOpening(bool value)
    {
        opening = value;
    }

    // puerta llama a este m�todo cuando la pulsaci�n es parte de la secuencia correcta.
    public void PlayCorrectSound()
    {
        if (audioSource != null && correctClip != null)
        {
            audioSource.PlayOneShot(correctClip);
        }
    }
    // puerta llama a este m�todo cuando la pulsaci�n es err�nea.
    public void PlayErrorSound()
    {
        if (audioSource != null && errorClip != null)
        {
            audioSource.PlayOneShot(errorClip);
        }
    }
}
