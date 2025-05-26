using UnityEngine;

public class DeadZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerInvokeCsharp>();
            if (player != null)
            {
                player.Die();
                GameManager.Instance.RespawnPlayer();
            }
        }
    }


}