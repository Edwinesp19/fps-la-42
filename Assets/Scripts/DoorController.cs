using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private Animator anim;
    public AudioClip sonidoAbrirPuerta;
    public AudioClip sonidoCerrarPuerta;
    [Header("Audio Settings")]
    private AudioSource audioSource;


    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }



    // Update is called once per frame
    void Update()
    {

    }

    // When the player is near the door, the door will open
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!audioSource.isPlaying) // Evita que el sonido se reproduzca en bucle continuo
            {
                audioSource.PlayOneShot(sonidoAbrirPuerta, 1f);
            }
            anim.SetBool("character_nearby", true);

        }
    }

    // When the player is far from the door, the door will close
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!audioSource.isPlaying) // Evita que el sonido se reproduzca en bucle continuo
            {
                StartCoroutine(CloseDoor());
            }
            anim.SetBool("character_nearby", false);
        }
    }


    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sonidoCerrarPuerta, 1f);
    }

}
