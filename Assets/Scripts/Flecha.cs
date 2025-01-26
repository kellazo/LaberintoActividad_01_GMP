using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    [SerializeField] private float speed = 10f;
    private bool activar; // Controla si la trampa se mueve

    // Distancia máxima que recorrerá la flecha antes de pararse
    [SerializeField] private float maxDistance = 10f;

    private Vector3 startPosition;   // Posición inicial cuando se activó

    // Para guardar la posición y rotación originales
    private Vector3 originalPos;
    private Quaternion originalRot;

    //para subscribirse se ejecuta despues del awake y antes del start
    private void OnEnable()
    {
        gM.OnAreaEntrada += Activar;
        gM.OnAreaSalida += Desactivar;
    }

    private void Start()
    {
        // Guardamos la posición y rotación originales de la flecha
        originalPos = transform.position;
        originalRot = transform.rotation;
    }
    // Cuando el player salga de la zona
    private void Desactivar(int id)
    {
        if (this.id == id)
        {

            activar = false;
            // La devolvemos inmediatamente a su posición y rotación original
            transform.position = originalPos;
            transform.rotation = originalRot;

        }
    }
    // Cuando el player entre en la zona
    private void Activar(int id)
    {
        if (this.id == id)
        {
            activar = true;
            // Guardar la posición inicial
            startPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si está activa la trampa, hacemos la oscilación
        if (activar)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
            // Medir la distancia recorrida
            float distanciaRecorrida = Vector3.Distance(transform.position, startPosition);
            // Si superó la distancia máxima, la paramos
            if (distanciaRecorrida >= maxDistance)
            {
                activar = false; 
            }
        }
    }

    //para desuscribirse se ejecuta antes del destroy/disable
    private void OnDisable()
    {
        // Importante desuscribirse para evitar errores
        gM.OnAreaEntrada -= Activar;
        gM.OnAreaSalida -= Desactivar;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el Player entre en el trigger del hacha
        if (activar && other.CompareTag("Player"))
        {
            // Buscamos su PlayerController para llamar a TakeDamage
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(2);
            }
        }
    }
}


