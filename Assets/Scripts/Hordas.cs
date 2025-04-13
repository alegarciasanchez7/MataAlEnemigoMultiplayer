using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hordas : MonoBehaviour
{
    public ValoresEnemigos[] hordas;
    public Transform[] puntosSpawn;
    private ValoresEnemigos hordaActual;
    float tiempoEspera = 0;
    int numeroHordaActual = 0;
    int enemigosPorCrear = 0;
    int enemigosParaMatar = 0;
    public float tiempoEntreHordas = 7f; // Tiempo de espera entre hordas

    public Image image;
    public Sprite[] sprites;

    public AudioSource rondasAudio;
    public AudioSource musicaFondo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Iniciando primera horda");

        // Obtener ambos AudioSource
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            rondasAudio = audioSources[0];
            musicaFondo = audioSources[1];
        }
        else
        {
            Debug.LogError("No se encontraron suficientes componentes AudioSource en el objeto.");
        }

        if (musicaFondo != null)
        {
            musicaFondo.loop = true; // Hacer que la música se repita
            musicaFondo.Play(); // Iniciar la reproducción de la música de fondo
        }

        NextHorda();
    }

    void ActualizarIndicadorRonda()
    {
        if(numeroHordaActual - 1 < sprites.Length)
        {
            image.sprite = sprites[numeroHordaActual - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemigosPorCrear > 0 && Time.time >= tiempoEspera) {
            this.enemigosPorCrear--;
            tiempoEspera = Time.time + hordaActual.tiempoEntreEnemigos;
            CrearEnemigo();
        }
    }

    void MuereOtro()
    {
        enemigosParaMatar--;
        Debug.Log("Enemigos restantes: " + enemigosParaMatar);
        if(enemigosParaMatar <= 0) {
            Debug.Log("Todos los enemigos de la horda han muerto. Iniciando siguiente horda.");
            if(rondasAudio != null)
            {
                rondasAudio.Play();
            }
            Invoke("NextHorda", tiempoEntreHordas); // Espera antes de iniciar la siguiente horda
        }
    }
    

    void NextHorda()
    {
        if (numeroHordaActual < hordas.Length)
        {
            hordaActual = hordas[numeroHordaActual];
            enemigosPorCrear = hordaActual.numEnemigos;
            enemigosParaMatar = hordaActual.numEnemigos;
            numeroHordaActual++;

            Debug.Log("Iniciando horda numero: " + numeroHordaActual);
            

            ActualizarIndicadorRonda();
        }
        else
        {
            Debug.Log("Todas las hordas han sido completadas.");
            if(musicaFondo != null)
            {
                musicaFondo.Stop();
            }
            // Cambiar a la escena EndGame
            SceneManager.LoadScene("EndGame");
        }
    }

    void CrearEnemigo()
    {
        Transform puntoSpawn = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
        GameObject enemigo = Instantiate(hordaActual.tipoEnemigo, puntoSpawn.position, puntoSpawn.rotation);
        MovimientoEnemigo movimientoEnemigo = enemigo.GetComponent<MovimientoEnemigo>();
        if (movimientoEnemigo != null)
        {
            movimientoEnemigo.OnDeath += MuereOtro; // Suscribirse al evento OnDeath del enemigo
            Debug.Log("Enemigo creado y suscrito al evento OnDeath");
        }
    }
}
