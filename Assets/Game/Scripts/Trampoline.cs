using UnityEngine;

public class Trampoline : Trap
{
    [SerializeField] float _power;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void ActivateTrap(PlayerInvokeCsharp player)
    {
        var rigid = player.Rigid;

        if (rigid != null)
        {
            _animator.SetTrigger("activate");
            rigid.velocity = Vector2.zero;
            var dir = transform.up;

            rigid.AddForce(dir * _power, ForceMode2D.Impulse);
        }

    }
}
