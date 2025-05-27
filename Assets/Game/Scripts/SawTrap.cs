using UnityEngine;

public class SawTrap : Trap
{
    [SerializeField] Transform[] _sawTransforms = new Transform[0];
    [SerializeField] float _sawSpeed = 3.0f;
    [SerializeField] float _sawWaitTime = 1.0f;

    private int _moveTargetSawIndex = 1;
    private int _moveDirection = 1; // 1 for forward, -1 for backward

    private Vector3[] _sawPositions;

    private void Start()
    {
        Initialize();
        transform.position = _sawPositions[0];
    }

    private void Update()
    {
        Move();
    }

    protected override void ActivateTrap(PlayerInvokeCsharp player)
    {
        player.KnockBack();
    }

    private void Initialize()
    {
        _sawPositions = new Vector3[_sawTransforms.Length];

        for (int i = 0; i < _sawTransforms.Length; i++)
        {
            _sawPositions[i] = _sawTransforms[i].position;
        }
    }
    
    private void Move()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, _sawPositions[_moveTargetSawIndex], _sawSpeed * Time.deltaTime);

        if (_moveTargetSawIndex == 0 || _moveTargetSawIndex == _sawPositions.Length - 1)
            _moveDirection *= -1;


    }
}
