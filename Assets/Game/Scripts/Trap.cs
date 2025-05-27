using UnityEngine;

public abstract class Trap : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            var player = collision.GetComponent<PlayerInvokeCsharp>();

            ActivateTrap(player);
        }
    }

    protected virtual void ActivateTrap(PlayerInvokeCsharp player) { }

}
