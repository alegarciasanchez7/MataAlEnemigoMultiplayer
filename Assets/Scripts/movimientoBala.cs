using UnityEngine;

public class movimientoBala : MonoBehaviour
{
    public float velocidad = 30f;
    public LayerMask collisionMask;
    public float damage = 1f;
    public AudioClip hitmarkerClip;
    public float hitmarkerVolume = 0.1f; // Variable para ajustar el volumen

    // Update is called once per frame
    void Update()
    {
        float moveDistancia = Time.deltaTime * velocidad;
        transform.Translate(Vector3.forward * moveDistancia);
        CheckCollision(moveDistancia);
    }

    void CheckCollision(float moveDistancia)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistancia, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Enemigo"))
        {
            MovimientoEnemigo enemigo = hit.collider.gameObject.GetComponent<MovimientoEnemigo>();
            if (enemigo != null)
            {
                if (hitmarkerClip != null)
                {
                    Debug.Log("Sonido de hitmarker");
                    PlayHitmarkerSound();
                }
                enemigo.TakeHit(damage, hit);
            }
        }
        gameObject.SetActive(false);
    }

    void PlayHitmarkerSound()
    {
        GameObject audioObject = new GameObject("HitmarkerSound");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = hitmarkerClip;
        audioSource.volume = hitmarkerVolume; // Ajustar el volumen
        audioSource.Play();
        Destroy(audioObject, hitmarkerClip.length);
    }
}
