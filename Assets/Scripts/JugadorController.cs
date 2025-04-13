using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]

public class JugadorController : LivingEntity
{
    CharacterController controlador;
    public float velocidad = 15f;
    Vector3 moveInput, movVelocidad;
    private Rigidbody rb;
    DisparoBalas controladorDisparo;

    public delegate void OnDeathJugador();
    public static event OnDeathJugador OnDeathPlayer;

    void Awake() {
        controlador = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        controladorDisparo = GetComponent<DisparoBalas>();
    }

    void Update()
    {
        // Recojo la entrada del jugador
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        
        // Creo un rayo que va desde la cámara hasta el suelo
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distancia;
        Plane sueloTmp = new Plane(Vector3.up, Vector3.zero); // Plano que representa el suelo

        // Si el rayo colisiona con el suelo, miro hacia el punto de colisión
        if(sueloTmp.Raycast(ray, out distancia)) {
            Vector3 punto = ray.GetPoint(distancia);
            Debug.DrawLine(ray.origin, punto, Color.red);
            rb.transform.LookAt(new Vector3(punto.x, transform.position.y, punto.z));
        }

        // Si pulso el botón izquierdo del ratón, disparo
        if(Input.GetButtonDown("Fire1")) {
            controladorDisparo.Disparar();
        }
    }

    void FixedUpdate() {
        movVelocidad = moveInput.normalized * velocidad;
        controlador.Move(movVelocidad * Time.deltaTime);
    }

    protected override void Die()
    {
        base.Die();
        if(OnDeathPlayer != null)
        {
            OnDeathPlayer();
        }
        // Cambiar a la escena "DeathScene" cuando el jugador muera
        SceneChanger.CambiarEscena("DeathScene", 1f);
    }

    void OnDestroy()
    {
        if(OnDeathPlayer != null)
        {
            OnDeathPlayer();
        }
    }
}
