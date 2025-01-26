using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    [SerializeField] private float velocity;
    private float yDirection = 1;
    private bool activar; // Controla si la trampa se mueve

    public float speed = 2f;           // Velocidad de movimiento
    public float yArriba = 2f;         // Posición Y objetivo arriba
    public float yAbajo = 0f;          // Posición Y objetivo abajo  
    private bool goingUp = true;       // Indica si está subiendo
    public float rotationSpeed = 100f; // Velocidad de rotación (grados/segundo)

    //para subscribirse se ejecuta despues del awake y antes del start
    private void OnEnable()
    {
        gM.OnAreaEntrada += Activar;
        gM.OnAreaSalida += Desactivar;
    }

    private void Desactivar(int id)
    {
        if (this.id == id)
        {
            
            activar = false;
            
        }
    }

    private void Activar(int id)
    {
        if (this.id == id)
        {
            //invertimos el estado (es como un interruptor que permuta)
            //bajando = !bajando;
            //yDirection *= -1f; //invertimos el estado

            //flip flop
            activar = true;
            //yDirection *= -1f; //inviertes el estado
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (activar)
        //{
            //transform.Translate(Vector3.down * 5 * Time.deltaTime, Space.World);
            //transform.Translate(new Vector3(0, yDirection, 0) * 5 * Time.deltaTime, Space.World);
            // transform.Rotate
           // transform.position = Vector3.MoveTowards(transform.position, Vector3.forward, velocity * Time.deltaTime);
        //}

        if (activar)
        {
            // Movimiento arriba/abajo
            float targetY = goingUp ? yArriba : yAbajo;
            // Calculamos la posición objetivo (manteniendo X y Z)
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            // Movemos la trampa hacia la posición objetivo
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // Si ya alcanzó la posición objetivo, invertimos el estado
            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                goingUp = !goingUp;
            }

            // Rotación
            // Rota en el eje Y local. 
            // (Si quieres rotar en otro eje, cambia Vector3.up a Vector3.forward, Vector3.right, etc.)
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
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


