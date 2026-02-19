using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- NOUVEAU : Indispensable pour lire la manette VR

public class GameManagerVR : MonoBehaviour
{
    [Header("Logique du Jeu")]
    public TrashBallCounter compteurPoubelle;
    public GameObject generateurPapier;

    [Header("Les Panneaux (Canvas)")]
    public GameObject panelMainMenu;
    public GameObject panelHUD;
    public GameObject panelGameOver;
    public GameObject panelPause;             // NOUVEAU : Le panneau de Pause

    [Header("Les Textes (TextMeshPro)")]
    public TextMeshProUGUI texteScoreJeu;
    public TextMeshProUGUI texteTempsJeu;
    public TextMeshProUGUI texteScoreFin;

    [Header("Paramètres")]
    public float tempsMax = 120f;
    private float tempsRestant;
    private bool jeuEnCours = false;
    private bool enPause = false;             // NOUVEAU : État de la pause

    [Header("Contrôles VR")]
    public InputActionReference boutonPauseManette; // NOUVEAU : Le bouton pour pauser

    void Start()
    {
        // Active l'écoute du bouton de la manette
        if (boutonPauseManette != null) boutonPauseManette.action.Enable();

        panelMainMenu.SetActive(true);
        panelHUD.SetActive(false);
        panelGameOver.SetActive(false);
        if (panelPause != null) panelPause.SetActive(false);

        if (generateurPapier != null) generateurPapier.SetActive(false);
    }

    void Update()
    {
        // 1. ÉCOUTE DE LA MANETTE : Si on appuie sur le bouton choisi
        if (boutonPauseManette != null && boutonPauseManette.action.WasPressedThisFrame())
        {
            // On ne peut pauser que si on a déjà lancé la partie (pas dans le Main Menu)
            if (jeuEnCours || enPause)
            {
                TogglePause();
            }
        }

        // Si on est dans le menu ou en pause, le chrono ne tourne pas
        if (!jeuEnCours || enPause) return;

        // 2. CHRONOMÈTRE
        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0)
        {
            tempsRestant = 0;
            DeclencherGameOver();
        }

        AfficherTemps(tempsRestant, texteTempsJeu);

        if (compteurPoubelle != null)
        {
            texteScoreJeu.text = "SCORE : " + compteurPoubelle.GetScore();
        }
    }

    void AfficherTemps(float temps, TextMeshProUGUI texteUI)
    {
        int minutes = Mathf.FloorToInt(temps / 60);
        int secondes = Mathf.FloorToInt(temps % 60);
        texteUI.text = string.Format("TIME : {0:00}:{1:00}", minutes, secondes);
    }

    // --- FONCTIONS POUR TES BOUTONS D'INTERFACE ---

    public void BoutonPlay()
    {
        tempsRestant = tempsMax;
        jeuEnCours = true;
        enPause = false;
        Time.timeScale = 1f; // On s'assure que le temps coule normalement

        panelMainMenu.SetActive(false);
        panelHUD.SetActive(true);

        if (generateurPapier != null) generateurPapier.SetActive(true);
    }

    void DeclencherGameOver()
    {
        jeuEnCours = false;
        panelHUD.SetActive(false);
        panelGameOver.SetActive(true);

        if (generateurPapier != null) generateurPapier.SetActive(false);

        if (compteurPoubelle != null && texteScoreFin != null)
        {
            texteScoreFin.text = "SCORE FINAL : " + compteurPoubelle.GetScore();
        }
    }

    // --- NOUVELLES FONCTIONS DE PAUSE ---

    public void TogglePause()
    {
        enPause = !enPause; // Alterne le mode Pause

        if (enPause)
        {
            Time.timeScale = 0f; // FIGE LE TEMPS (les objets en l'air s'arrêtent)
            if (panelPause != null) panelPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // RELANCE LE TEMPS
            if (panelPause != null) panelPause.SetActive(false);
        }
    }

    public void BoutonResume() // A relier à ton bouton "Reprendre"
    {
        if (enPause) TogglePause();
    }

    public void BoutonRestart()
    {
        Time.timeScale = 1f; // Toujours remettre le temps à la normale avant de relancer !
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BoutonQuit()
    {
        Application.Quit();
    }
}