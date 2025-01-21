using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaTrampa : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM; //manegador de eventos
    [SerializeField] private int id;
    [SerializeField] private float velocity;


    public void CambiarEstado()
    {
        gM.EventoArea(id);
        
    }

    public void SalirEstado()
    {
        gM.EventoAreaSalida(id);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            SalirEstado();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            CambiarEstado();
        }
    }

}
