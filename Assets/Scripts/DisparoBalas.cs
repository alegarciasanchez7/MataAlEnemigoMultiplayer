using UnityEngine;
using System.Collections.Generic;

public class DisparoBalas : MonoBehaviour
{
    public Transform salida;
    public GameObject balaPrefab;
    public float velocidadEntreDisparos = 100f;
    float proximoDisparo = 0f;
    public int poolSize = 20;

    private List<GameObject> poolBalas;

    public AudioSource disparoAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        disparoAudio = GetComponent<AudioSource>();
        poolBalas = new List<GameObject>();
        for(int i = 0; i < poolSize; i++) {
            GameObject bala = Instantiate(balaPrefab);
            bala.SetActive(false);
            poolBalas.Add(bala);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disparar()
    {
        if(Time.time >= proximoDisparo) {
            GameObject balaDisponible = ObtenerBalaInactiva();
            if(balaDisponible != null)
            {
                if(disparoAudio != null)
                {
                    disparoAudio.Play();
                }

                balaDisponible.transform.position = salida.position;
                balaDisponible.transform.rotation = salida.rotation;
                balaDisponible.SetActive(true);
                proximoDisparo = Time.time + 1f / velocidadEntreDisparos;
            }
        }
    }

    private GameObject ObtenerBalaInactiva()
    {
        foreach(GameObject bala in poolBalas) {
            if(!bala.activeInHierarchy) {
                return bala;
            }
        }

        // Si no hay balas disponibles, creamos una nueva
        GameObject nuevaBala = Instantiate(balaPrefab);
        nuevaBala.SetActive(false);
        poolBalas.Add(nuevaBala);
        return nuevaBala;
    }
}
