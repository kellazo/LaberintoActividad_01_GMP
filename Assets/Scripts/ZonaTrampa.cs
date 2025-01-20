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

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            CambiarEstado();
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
