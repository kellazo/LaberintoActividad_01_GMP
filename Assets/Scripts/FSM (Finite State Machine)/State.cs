using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> : MonoBehaviour
{
    protected T controller;
    public virtual void OnEnterState(T controller)
    {
        this.controller = controller; // Virtual porque primero pasa aqui y luego override a las clases hijas. Debes ahcer base en la hija
    }

    public abstract void OnUpdateState();

    public abstract void OnExitState();

}
