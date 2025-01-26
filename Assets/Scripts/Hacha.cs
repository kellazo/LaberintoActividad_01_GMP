using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacha : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;

    [Header("Parámetros de oscilación")]
    [SerializeField] private float amplitude = 45f; // Grados de inclinación
    [SerializeField] private float speed = 2f;  // Velocidad de oscilación
    [Header("Dirección")]
    [SerializeField] private bool invertDirection = false;

    private bool activar; // Controla si la trampa se mueve
    private float initialRotationX;   // Guardará la rotación inicial en X

    //para subscribirse se ejecuta despues del awake y antes del start
    private void OnEnable()
    {
        gM.OnAreaEntrada += Activar;
        gM.OnAreaSalida += Desactivar;
    }

    private void Start()
    {
        // Guardamos la componente X inicial
        initialRotationX = transform.localEulerAngles.x;
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
            // Calculamos un ángulo usando seno, oscilando entre +amplitude y -amplitude
            float direction = invertDirection ? -1f : 1f;
            float angle = initialRotationX + (amplitude * direction) * Mathf.Sin(Time.time * speed);

            // Mantenemos las otras componentes de la rotación
            Vector3 currentRotation = transform.localEulerAngles;
            currentRotation.x = angle;
            transform.localEulerAngles = currentRotation;
        }
    }

    //para desuscribirse se ejecuta antes del destroy/disable
    private void OnDisable()
    {
        // Importante desuscribirse para evitar errores
        gM.OnAreaEntrada -= Activar;
        gM.OnAreaSalida -= Desactivar;
    }
}


