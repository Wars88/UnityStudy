using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator _animator;
    private bool _isActivated = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActivated)
            return;

        if (collision.CompareTag("Player"))
        { 
            var player = collision.GetComponent<PlayerInvokeCsharp>();  

            if (player != null)
            {
                ActivateCheckPoint();
                GameManager.Instance.ChangeSpawnPoint(transform);
            }
        }
    }

    private void ActivateCheckPoint()
    {
        _isActivated = true;
        _animator.SetTrigger("activate");
    }
}
