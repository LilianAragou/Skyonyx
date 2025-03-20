using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class reset : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string buttonText;

    private Button button;
    private TextMeshProUGUI buttonTextComponent;
    private Color originalColor;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonTextComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        originalColor = button.GetComponent<Image>().color;

        buttonTextComponent.text = buttonText;

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Supprimer toutes les données sauvegardées
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Level1", 1); // Débloque le premier niveau après reset
        PlayerPrefs.Save();

        // Met à jour tous les boutons après le reset
        UpdateAllButtons();
    }

    // Méthode pour mettre à jour tous les boutons dans la scène
    private void UpdateAllButtons()
    {
        // Trouver tous les boutons dans la scène avec le tag "LevelButton"
        GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");

        // Mettre à jour chaque bouton de niveau
        foreach (GameObject buttonObj in levelButtons)
        {
            start_lvl buttonScript = buttonObj.GetComponent<start_lvl>();
            if (buttonScript != null)
            {
                buttonScript.UpdateButtonState();
            }
        }
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
