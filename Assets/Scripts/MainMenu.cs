using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    
    // Se ejecuta cuando clicquemos el boton de opciones
    public void OnOptionsButtonClicked()
    {
        //ocultar mainmenupanel i mostrar optionsmenupanel:

        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }
}
