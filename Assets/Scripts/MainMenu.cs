using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer; 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuPanel.SetActive(false);
            optionsMenuPanel.SetActive(true);
        }
    }
    // Se ejecuta cuando clicquemos el boton de opciones
    public void OnOptionsButtonClicked()
    {
        //ocultar mainmenupanel i mostrar optionsmenupanel:

        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void SetNewVolumeToMusic(float volume)
    {
        mainMixer.SetFloat("musicVolume", volume);
    }

    public void SetNewVolumeToSounds(float volume)
    {
        mainMixer.SetFloat("soundsVolume", volume);
    }

    public void SetNewFullScreenState(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
