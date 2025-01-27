using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MiGameManagerSO")]
public class GameManagerSO : ScriptableObject
{

    public event Action<int> OnBotonPulsado;
    public void InteractuableEjecutado(int idBoton)
    {
        //como lanzo el evento?
        OnBotonPulsado?.Invoke(idBoton); // lanzar de un evento de que un boton ha sido pulsado. //para que el otro objeto se entere que se tiene que subscribir
    }
    public event Action<int> OnBotonPulsadoSolo;
    public void InteractuableEjecutadoSolo(int idBoton)
    {
        //como lanzo el evento?
        OnBotonPulsadoSolo?.Invoke(idBoton);
    }
    public event Action<int> OnAreaEntrada;
    public void EventoArea(int idBoton)
    {
        //como lanzo el evento?
        OnAreaEntrada?.Invoke(idBoton);
    }
    public event Action<int> OnAreaSalida;
    public void EventoAreaSalida(int idBoton)
    {
        //como lanzo el evento?
        OnAreaSalida?.Invoke(idBoton);
    }
}
