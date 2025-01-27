using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuertaInterruptor : MonoBehaviour
{
    [Header("Hint UI")]
    [SerializeField] private CanvasManager canvasManager; // Asignar en Inspector
    [SerializeField] private string hintMessage = "Pulsa E para abrir la puerta";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            // Ocultar texto
            if (canvasManager != null)
            {
                canvasManager.HideHint();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
        {
            // Mostrar texto
            if (canvasManager != null)
            {
                canvasManager.ShowHint(hintMessage);
            }
        }
    }
}



