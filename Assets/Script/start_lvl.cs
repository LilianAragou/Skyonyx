using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class start_lvl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string levelName; 
    public int levelIndex; 
    public string buttonText; 

    private Button button;
    private TextMeshProUGUI buttonTextComponent;
    private Color originalColor;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Level1"))
        {
            PlayerPrefs.SetInt("Level1", 1);
            PlayerPrefs.Save();
        }
        
        button = GetComponent<Button>();
        buttonTextComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        originalColor = button.GetComponent<Image>().color;

        buttonTextComponent.text = buttonText;

        UpdateButtonState();

        button.onClick.AddListener(OnButtonClick);
    }

    public void UpdateButtonState()
    {
        int levelState = PlayerPrefs.GetInt("Level" + levelIndex, 0); 

        if (levelState == 0)
        {
            originalColor = Color.red; 
            button.interactable = false; 
        }
        else if (PlayerPrefs.GetInt("Level" + levelIndex + "Won", 0) == 1)
        {
            originalColor = Color.green; 
        }
        else
        {
            originalColor = Color.white; 
        }

        button.GetComponent<Image>().color = originalColor;
    }

    private void OnButtonClick()
    {
        SceneController.instance.LoadScene(levelName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            Color darkerColor = originalColor * 0.8f; // Rend la couleur plus sombre (20% plus foncée)
            button.GetComponent<Image>().color = darkerColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.GetComponent<Image>().color = originalColor;
    }
}
