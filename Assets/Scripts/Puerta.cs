using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    private bool abrir = false;
    private void OnEnable()
    {
        gM.OnBotonPulsado += Activar;
    }

    private void Activar(int id)
    {
        if (this.id == id)
        {
            abrir = true;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (abrir)
        {
            transform.Translate(Vector3.down * 5 * Time.deltaTime, Space.World);
        }
    }
}
