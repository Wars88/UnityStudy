using UnityEngine;

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] Vector2 _movingOffset;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _renderer.material.mainTextureOffset += _movingOffset * Time.deltaTime;
    }
}