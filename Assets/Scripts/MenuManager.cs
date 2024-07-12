using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip sonidoClick;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Jugar()
    {
        ejecutarSonido(sonidoClick);
        CambiarDeNivel("01-galeria-de-tiro");
    }
    public void Opciones()
    {
        ejecutarSonido(sonidoClick);
        // CambiarDeNivel("MenuOpciones");
    }
    public void ReiniciarNivel()
    {
        ejecutarSonido(sonidoClick);
        switch (SceneManager.GetActiveScene().name)
        {
            case "01-galeria-de-tiro":
                CambiarDeNivel("01-galeria-de-tiro");
                break;

            case "02-boss":
                CambiarDeNivel("01-galeria-de-tiro");
                break;
            default:
                CambiarDeNivel("01-galeria-de-tiro");
                break;

        }

    }
    public void Salir()
    {
        ejecutarSonido(sonidoClick);
        Debug.Log("Salir...");
        Application.Quit();
    }
    public void Atras()
    {
        ejecutarSonido(sonidoClick);
        CambiarDeNivel("MenuPrincipal");
    }


    public void ejecutarSonido(AudioClip sonido, float volumen = 1f)
    {
        StartCoroutine(ExecSound(sonido, 0.7f, volumen));

    }

    IEnumerator ExecSound(AudioClip sonido, float delay, float volumen)
    {
        //esperar el delay despues de ejecutar el sonido
        audioSource.PlayOneShot(sonido, volumen);
        yield return new WaitForSeconds(delay);

    }

    public void CambiarDeNivel(string escena, float delay = 0f)
    {
        StartCoroutine(CambiarEscena(escena, delay));
    }

    IEnumerator CambiarEscena(string escena, float delay)
    {

        //esperar el delay antes de ir a la escena
        yield return new WaitForSeconds(delay);
        //cargar la escena
        SceneManager.LoadScene(escena);

    }

}
