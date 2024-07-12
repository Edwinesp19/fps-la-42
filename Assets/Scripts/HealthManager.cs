using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Character))]
public class HealthManager : MonoBehaviour
{
    [Header("Character")]

    [Tooltip("Character.")]
    [SerializeField]


    public AudioSource quienEmite;
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBar;
    public Image scoreBar;

    public Image backgroundPerder;
    public GameObject gameOverLayout;
    public GameObject gameWinnerLayout;
    public TMP_Text textoPerder;
    public TMP_Text textoPuntaje;
    public TMP_Text textoPuntajeGanar;

    public AudioClip sonidoPerder;
    public AudioClip sonidoGanar;

    private Character playerCharacter;


    private string DIED_MESSAGE = "HAZ MUERTO";
    // private string GAME_OVER_MESSAGE = "PERDISTE";
    private string SCORE_MESSAGE = "SCORE: ";

    [Header("Audio Settings")]
    private AudioSource audioSource;

    [Header("ScoreManager")]

    [Tooltip("Score Manager.")]
    [SerializeField]
    private ScoreManager ScoreManager;

    private bool playerWin = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        playerCharacter = GetComponent<Character>();
        ScoreManager = GetComponent<ScoreManager>();
        ScoreManager.ResetScore();
        // backgroundPerder = GetComponent<Image>();
        // textoPerder = GetComponent<TMP_Text>();
        // textoPuntaje = GetComponent<TMP_Text>();
        playerCharacter.LockCursor();
        playerWin = false;

        HideGameOverMenu();
        HideGameWinnerMenu();
    }

    void Update()
    {
        UpdateScoreBar();
        if (ScoreManager.currentScore == ScoreManager.maxScore && playerWin == false)
        {
            playerWin = true;
            Win();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth >= 0 && currentHealth <= 30)
        {
            healthBar.color = new Color(255, 0, 0); //color rojo 
        }
        else
        {
            healthBar.color = new Color(255, 255, 255); //color blanco
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    void UpdateScoreBar()
    {
        scoreBar.fillAmount = ScoreManager.currentScore / 100f;
    }


    void ShowGameOverMenu()
    {
        textoPerder.text = DIED_MESSAGE;
        textoPuntaje.text = SCORE_MESSAGE + ScoreManager.currentScore.ToString();
        gameOverLayout.gameObject.SetActive(true);
    }
    void ShowGameWinnerMenu()
    {
        textoPerder.text = DIED_MESSAGE;
        textoPuntajeGanar.text = SCORE_MESSAGE + ScoreManager.currentScore.ToString();
        gameWinnerLayout.gameObject.SetActive(true);
    }
    void HideGameWinnerMenu()
    {
        gameWinnerLayout.gameObject.SetActive(false);
    }
    void HideGameOverMenu()
    {
        gameOverLayout.gameObject.SetActive(false);
    }

    private void Die()
    {
        quienEmite.PlayOneShot(sonidoPerder, 1f);
        playerCharacter.UnlockCursor();
        Debug.Log("HAZ MUERTO");
        ScoreManager.ResetScore();
        ShowGameOverMenu();
    }
    private void Win()
    {
        //disable mouse click key input for 1 second

        StartCoroutine(Wait(1f));
        quienEmite.PlayOneShot(sonidoGanar, 1f);
        playerCharacter.UnlockCursor();
        Debug.Log("GANASTE");
        ShowGameWinnerMenu();
    }
    private IEnumerator Wait(float time)
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(time);
    }

    public void ejecutarSonido(AudioClip sonido, float volumen = 1f)
    {
        StartCoroutine(ExecSound(sonido, 1f, volumen));
    }

    IEnumerator ExecSound(AudioClip sonido, float delay, float volumen)
    {
        //esperar el delay despues de ejecutar el sonido
        audioSource.PlayOneShot(sonido, volumen);
        yield return new WaitForSeconds(delay);

    }
}
