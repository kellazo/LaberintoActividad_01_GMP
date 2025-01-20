using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    [SerializeField] private float velocity;
    private float yDirection = 1;
    private bool activar;

    //para subscribirse se ejecuta despues del awake y antes del start
    private void OnEnable()
    {
        gM.OnAreaEntrada += Activar;
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
            yDirection *= -1f; //inviertes el estado
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activar)
        {
            //transform.Translate(Vector3.down * 5 * Time.deltaTime, Space.World);
            //transform.Translate(new Vector3(0, yDirection, 0) * 5 * Time.deltaTime, Space.World);
            // transform.Rotate
            transform.position = Vector3.MoveTowards(transform.position, Vector3.forward, velocity * Time.deltaTime);
        }
    }

    //para desuscribirse se ejecuta antes del destroy/disable
    private void OnDisable()
    {
        gM.OnAreaEntrada -= Activar;
    }
}

