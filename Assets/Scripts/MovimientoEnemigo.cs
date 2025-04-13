using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MovimientoEnemigo : LivingEntity
{
    public UnityEngine.AI.NavMeshAgent pathfinder;
    public Transform target;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeath;

    float myCollisionRadius;
    float targetCollisionRadius;

    float distanciaAtaque = 2.5f;
    float NextAttackTime = 0f;
    float TimeBetweenAttack = 2f;
    bool Atacando = false;

    LivingEntity targetEntity;
    float damage = 1;
    bool finPartida = false;

    public AudioSource atackSound;

    void Awake()
    {
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        atackSound = GetComponent<AudioSource>();

        // Intentar encontrar al jugador, pero no asumir que siempre estará disponible
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            target = null; // Asegurarse de que target sea null si no hay jugador
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        targetEntity = target.GetComponent<LivingEntity>();
        JugadorController.OnDeathPlayer += FinalPartida;
    }

    void FinalPartida()
    {
        finPartida = true;
        pathfinder.enabled = false;
        StopAllCoroutines(); // Detiene todas las corrutinas en ejecución
    }

    // Update is called once per frame
    void Update()
    {
        // Intentar encontrar al jugador si aún no se ha asignado
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                targetEntity = target.GetComponent<LivingEntity>();

                // Asignar colisiones si el jugador aparece después
                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            }
        }

        if (!finPartida && target != null) // Solo continuar si el jugador está disponible
        {
            if (!Atacando)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + distanciaAtaque);
                pathfinder.SetDestination(targetPosition);

                if (Time.time > NextAttackTime)
                {
                    NextAttackTime = Time.time + TimeBetweenAttack;
                    float sqrDistanciaToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDistanciaToTarget <= Mathf.Pow(myCollisionRadius + targetCollisionRadius + distanciaAtaque, 2))
                    {
                        Debug.Log("Atacando al jugador");
                        StartCoroutine(Attack());
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if(!finPartida)
        {
            pathfinder.enabled = false;
            Atacando = true;

            // Reproducir sonido de ataque
            if(atackSound != null)
            {
                atackSound.Play();
            }


            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius);

            float percent = 0;
            float attackSpeed = 0.5f;

            bool hasAppliedDamage = false;

            while (percent <= 1)
            {
                if(percent >= .5f && !hasAppliedDamage)
                {
                    targetEntity.TakeDamage(damage);
                    hasAppliedDamage = true;
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
                yield return null;
            }

            // Habilitar el pathfinder y continuar moviéndose
            pathfinder.enabled = true;
            Atacando = false;
        }        
    }

    public override void TakeHit(float damage, RaycastHit hit)
    {
        // Lógica para reducir la salud del enemigo
        Debug.Log("Llamando a TakeHit de LivingEntity");
        base.TakeHit(damage, hit);
    }

    protected override void Die() // Cambiado a protected
    {
        base.Die();
        Debug.Log("Enemigo muerto");
        if (OnDeath != null)
        {
            Debug.Log("Invocando evento OnDeath");
            OnDeath();
        }
    }
}
