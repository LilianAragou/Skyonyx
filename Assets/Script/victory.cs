using UnityEngine;
using TMPro; // Nécessaire pour utiliser TextMeshPro
using System.Collections;

public class ShowTextOnCollision : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Texte à afficher
    public float attackSpeedMultiplier = 20f; // Multiplie la vitesse d'attaque du joueur
    public int currentLevel;
    private SpriteRenderer spriteRenderer; // Référence au SpriteRenderer de l'objet
    private bool effectApplied = false; // Vérifie si l'effet a été appliqué

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false); // Cache le texte au début
        }

        spriteRenderer = GetComponent<SpriteRenderer>(); // Récupère le SpriteRenderer
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!effectApplied && other.CompareTag("Player")) // Vérifie si c'est le joueur et si l'effet n'a pas déjà été appliqué
        {
            ShowMessage();
            ModifyPlayerAttackSpeed(other.gameObject);
            HideObject();
            effectApplied = true; // Empêche de réappliquer l'effet plusieurs fois
            StartCoroutine(ResetEffectAndLoadMenu(other.gameObject)); // Lance la coroutine
        }
    }

    private void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
        }
    }

    private void ModifyPlayerAttackSpeed(GameObject player)
    {
        player.GetComponent<attackDist>().reloadTime = 1 / attackSpeedMultiplier;
    }

    private void HideObject()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Désactive l'affichage de l'objet
        }
    }

    private IEnumerator ResetEffectAndLoadMenu(GameObject player)
    {
        yield return new WaitForSeconds(5f); // Attend 5 secondes
        player.GetComponent<attackDist>().reloadTime = 1f; // Réinitialise la vitesse d'attaque
        PlayerPrefs.SetInt("Level" + currentLevel, 1); // Marque le niveau comme gagné
        PlayerPrefs.SetInt("Level" + currentLevel + "Won", 1); // Niveau terminé
        PlayerPrefs.SetInt("Level" + (currentLevel + 1), 1); // Débloque le suivant
        PlayerPrefs.Save();
        SceneController.instance.LoadScene("Menu"); // Charge la scène du menu
    }
}
