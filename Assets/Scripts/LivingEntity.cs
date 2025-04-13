using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamagable
{
    protected bool dead;
    protected float health;
    public float healthStart;
    public Sprite[] sprites;
    public Image img;
    public int vidaSize;

    public virtual void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
        // Problema con el Raycast, no se puede acceder al punto de colisión
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        img.sprite = sprites[(int)health];
        Debug.Log("Daño recibido: " + damage + ", Salud restante: " + health);
        if(health <= 0 && !dead) {
            Die();
        }
    }

    protected virtual void Start()
    {
        health = healthStart;
        Debug.Log("Salud inicial: " + health);
    }

    protected virtual void Die()
    {
        dead = true;
        Debug.Log("Muerto");
        gameObject.SetActive(false);
    }
}