using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;


    [Header("Dirección")]
    [SerializeField] private bool invertDirection = false;


    private bool activar; // Controla si la trampa se mueve


    //para subscribirse se ejecuta despues del awake y antes del start
    private void OnEnable()
    {
        gM.OnAreaEntrada += Activar;
        gM.OnAreaSalida += Desactivar;
    }

    private void Start()
    {
        
    }
    // Cuando el player salga de la zona
    private void Desactivar(int id)
    {
        if (this.id == id)
        {

            activar = false;

        }
    }
    // Cuando el player entre en la zona
    private void Activar(int id)
    {
        if (this.id == id)
        {
            activar = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si está activa la trampa, hacemos la oscilación
        if (activar)
        {
           
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


