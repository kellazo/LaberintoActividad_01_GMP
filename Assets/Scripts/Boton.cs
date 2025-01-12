using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] public GameManagerSO gM;
    [SerializeField] private int id;
    [SerializeField] private float velocity;
    [SerializeField] private Transform destination;

    private bool opening = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            //CINEMATICO: Sin fisicas
            transform.position = Vector3.MoveTowards(transform.position, destination.position, velocity * Time.deltaTime);
        }
    }

    public void Interactuar()
    {
        opening = true;
        gM.InteractuableEjecutado(id);
    }
}
