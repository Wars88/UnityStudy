using UnityEngine;

public class Fruit : MonoBehaviour
{
    public enum FruitType
    {
        Apple = 0,
        Banana = 1,
        Cherry = 2,
        Kiwi = 3,
        Melon = 4,
        Orange = 5,
        Pineapple = 6,
        Strawberry = 7,
    }

    [SerializeField] GameObject _vfxPrefab;

    private Animator _animator;

    [SerializeField] FruitType _fruitType;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetFruitAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GetFruit();

            if (_vfxPrefab != null)
            {
                GameObject vfx = Instantiate(_vfxPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void SetFruitAnimation() => _animator.SetFloat("fruitIndex", (int)_fruitType);
}