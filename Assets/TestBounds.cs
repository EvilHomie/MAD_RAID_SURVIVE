using UnityEngine;

public class TestBounds : MonoBehaviour
{
    [SerializeField] GameObject _boundsCube;

    Renderer _renderer;

    private void Start()
    {
        
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _boundsCube.transform.position = _renderer.bounds.center;
        _boundsCube.transform.localScale = _renderer.bounds.size;
    }
}
