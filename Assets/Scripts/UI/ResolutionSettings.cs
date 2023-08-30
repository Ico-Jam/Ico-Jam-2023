using UnityEngine;
using TMPro;
using System.Linq;

public class ResolutionSettings : MonoBehaviour
{
    Resolution[] resolutions;

    public GameObject previousButton;
    public GameObject nextButton;
    public TextMeshProUGUI textValue;

    private int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
            previousButton.SetActive(index > 0);
            nextButton.SetActive(index < resolutions.Length - 1);
            ChangeResolution();
            textValue.text = resolutions[index].width.ToString() + " x " + resolutions[index].height.ToString();
        }
    }
    private int index;

    private void ChangeResolution()
    {
        Resolution resolution = resolutions[Index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void IncrementIndex() { Index++; }

    public void DecrementIndex() { Index--; }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
            return;
        }

        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRateRatio.value == 60).ToArray();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                Index = i;
            }
        }
    }
}
