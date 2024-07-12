using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    //Used to check if the target has been hit
    public bool isHit = false;
    float randomTime;
    bool routineStarted = false;
    public bool isBoss = false;

    public float Enemyhealth = 10f;


    [Header("Customizable Options")]
    //Minimum time before the target goes back up
    public float minTime;
    //Maximum time before the target goes back up
    public float maxTime;

    public Transform target;  // El objetivo que el enemigo perseguirá, típicamente el jugador.
    public float chaseDistance = 6f;  // Distancia a partir de la cual el enemigo comenzará a perseguir al jugador.
    public float stopDistance = 2.0f;  // Distancia a la que el enemigo debe detenerse del jugador.
    public float speed = 3.5f;  // Velocidad a la que el enemigo se moverá hacia el jugador.
    public float damage = 10f;  // Daño que el enemigo inflige al jugador.
    public float damageInterval = 1.0f;  // Intervalo de tiempo entre cada golpe al jugador.
    private Vector3 initialPosition;  // Posición inicial del enemigo

    private NavMeshAgent agent;
    private Animator animator;
    private float nextDamageTime = 0f;

    [Header("Audio Settings")]

    public AudioClip beingHitSound;
    public AudioClip nearbySound;
    public AudioClip AttackSound;
    public AudioClip DeathSound;
    private AudioSource audioSource;

    private float currenthealth;



    [Header("HealthManager")]

    [Tooltip("HealthManager.")]
    [SerializeField]
    private HealthManager healthManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        healthManager = GetComponent<HealthManager>();
        currenthealth = Enemyhealth;


        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent.stoppingDistance = stopDistance;
        agent.speed = speed;

        StartCoroutine(CheckDistanceAndAct()); // Asegúrate de iniciar la corrutina
    }

    void Update()
    {
        //Generate random time based on min and max time values
        randomTime = Random.Range(minTime, maxTime);


        if (currenthealth == 0 && routineStarted == false)
        {
            animator.SetTrigger("Death");
            audioSource.PlayOneShot(DeathSound);

            if (isBoss)
            {
                StartCoroutine(Wait(14f));
            }
            Destroy(gameObject, 4f);
            routineStarted = true;
            return;
        }

        if (isHit == true)
        {
            if (routineStarted == false)
            {
                if (currenthealth > 0)
                {
                    // healthManager.AddScore(50);
                    currenthealth -= 1;
                    animator.SetTrigger("Damage");
                    Debug.Log("Enemy health: " + currenthealth);
                    PlaySound(beingHitSound);
                    StartCoroutine(DelayTimer(0f, beingHitSound, false));
                    routineStarted = true;

                }

            }

        }

    }

    IEnumerator CheckDistanceAndAct()
    {
        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget < chaseDistance)
            {
                ChasePlayer(distanceToTarget);
            }
            else
            {
                ReturnToInitialPosition();
            }

            yield return new WaitForSeconds(0.2f);  // Check every 0.2 seconds
        }
    }

    void ChasePlayer(float distanceToTarget)
    {
        HealthManager playerHealth = target.GetComponent<HealthManager>();

        if (!agent.hasPath || agent.destination != target.position)
        {
            if (currenthealth > 0 && playerHealth.currentHealth > 0)
            {
                // animator.SetFloat("speed", 0.7f); // corriendo
                animator.SetBool("Attack", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("Aiming", false);

                if (isBoss)
                {
                    animator.SetBool("running", true);
                }

                Debug.Log("Enemigo persiguiendo al jugador");
                agent.SetDestination(target.position);
                // audioSource.PlayOneShot(zombieSound);

            }
        }

        //si la distancia al objetivo es menor o igual a la distancia de parada y el tiempo actual es mayor o igual al tiempo de daño siguiente
        if (distanceToTarget <= stopDistance && Time.time >= nextDamageTime)
        {
            //el enemigo hará daño al jugador unicamente si los 2 estan vivos
            if (currenthealth > 0 && playerHealth.currentHealth > 0)
            {
                TryDamagePlayer();
                animator.SetBool("Aiming", true);
                animator.SetBool("Attack", true);
                // Debug.Log("Enemigo peleando con el jugador");
                animator.SetBool("isWalking", false);
                if (isBoss)
                {
                    animator.SetBool("running", false);
                }
                StartCoroutine(Wait(2f));
            }
            else
            {
                //deja de atacar si el jugador esta muerto o el mismo enemigo esta muerto

                if (currenthealth == 0 || playerHealth.currentHealth == 0)
                {

                    animator.SetBool("Attack", false);
                    animator.SetBool("Aiming", false);
                }
            }
        }
        else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Aiming", false);
            // animator.SetBool("Aiming", false);
            // animator.SetBool("isWalking", false);
            // if (isBoss)
            // {
            //     animator.SetBool("running", false);
            // }
        }
    }

    void ReturnToInitialPosition()
    {
        HealthManager playerHealth = target.GetComponent<HealthManager>();
        if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            if (!agent.hasPath || agent.destination != initialPosition)
            {
                if (currenthealth > 0 && playerHealth.currentHealth > 0)
                {
                    animator.SetBool("Attack", false);
                    animator.SetBool("Aiming", false);
                    animator.SetBool("isWalking", true);
                    if (isBoss)
                    {
                        animator.SetBool("running", true);
                    }
                    Debug.Log("Enemigo regresando a la posición inicial");

                    agent.SetDestination(initialPosition);

                }
            }
        }
        else
        {
            if (currenthealth > 0 && playerHealth.currentHealth > 0)
            {

                agent.ResetPath();
                animator.SetBool("Attack", false);
                animator.SetBool("Aiming", false);
                animator.SetBool("isWalking", false);
                if (isBoss)
                {
                    animator.SetBool("running", false);
                }
            }

            // animator.SetFloat("speed", 0f);
        }
    }

    void TryDamagePlayer()
    {
        HealthManager playerHealth = target.GetComponent<HealthManager>();

        if (playerHealth.currentHealth > 0 && playerHealth)
        {

            playerHealth.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
            PlaySound(AttackSound);
        }
    }

    private IEnumerator Wait(float time)
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(time);
    }
    private IEnumerator DelayTimer(float time, AudioClip sound, bool hasSound)
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(time);
        if (hasSound)
        {
            PlaySound(sound);
        }

        //Set the upSound as current sound, and play it
        // audioSource.GetComponent<AudioSource>().clip = upSound;
        // audioSource.Play();

        //Target is no longer hit
        isHit = false;
        routineStarted = false;
    }
    void PlaySound(AudioClip sound)
    {
        if (sound != null && audioSource != null)
        {
            if (!audioSource.isPlaying) // Evita que el sonido se reproduzca en bucle continuo
            {
                audioSource.PlayOneShot(sound);
            }
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}