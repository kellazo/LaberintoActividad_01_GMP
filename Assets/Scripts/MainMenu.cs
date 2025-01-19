using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer; 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private TMP_Dropdown qualityDropodown;
    [SerializeField] private TMP_Dropdown resolutionsDropodown;


    private Resolution[] resolutions; // Resoluciones disponibles en mi pantalla.

    private List<string> resolutionsOptions = new List<string>();
    private void Start()
    {
        InitResolutionDropdown();
        // Inicializar el dropdown con el valor por defecto de nivel de calidad
        qualityDropodown.value = QualitySettings.GetQualityLevel();
        qualityDropodown.RefreshShownValue(); // Refresco los valores.

        resolutionsDropodown.value = GetDefaultResolution();
        resolutionsDropodown.RefreshShownValue();


    }

    private void InitResolutionDropdown()
    {
        resolutions = Screen.resolutions;

        foreach (var resolution in resolutions)
        {
            resolutionsOptions.Add(resolution.width + "x" + resolution.height);
        }
        resolutionsDropodown.AddOptions(resolutionsOptions);
    }

    private int GetDefaultResolution()
    {
        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuPanel.SetActive(true);
            optionsMenuPanel.SetActive(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    // Se ejecuta cuando clicquemos el boton de opciones
    public void OnOptionsButtonClicked()
    {
        //ocultar mainmenupanel i mostrar optionsmenupanel:

        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
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

    public void SetNewQualityLevel(int  qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetNewResolution(int resolutionIndex)
    {
        Resolution chosenResolution = resolutions[resolutionIndex];
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }
}
