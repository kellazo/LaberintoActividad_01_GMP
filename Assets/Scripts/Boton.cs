using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] public GameManagerSO gM;
    [SerializeField] private int id;
    [SerializeField] private float velocity;
    [SerializeField] private Transform destination;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSource;   // SU propio AudioSource
    [SerializeField] private AudioClip Clip;     // Sonido
    [SerializeField] private CanvasManager canvasManager;
    private bool opening = false;
    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Guardamos la posición inicial para poder restaurarla
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

    public void Interactuar()
    {
        opening = true;
        if (audioSource != null && Clip != null)
        {
            audioSource.PlayOneShot(Clip);
        }
        // Notificamos al GameManagerSO el ID de este botón
        gM.InteractuableEjecutadoSolo(id);

    }


}
