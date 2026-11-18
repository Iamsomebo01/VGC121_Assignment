using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Life targetLife = other.GetComponent<Life>();

        if (targetLife != null)
        {
            targetLife.TakeDamage(damage);
        }
    }
}
