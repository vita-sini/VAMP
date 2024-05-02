using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirizmDetected : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _boundarySprite;

    private List<Enemy> _enemyDetected = new List<Enemy>();

    public List<Enemy> EnemyDetected => _enemyDetected;

    public void RenderSprite()
    {
        _boundarySprite.gameObject.SetActive(true);
    }

    public void DeactivationSprite()
    {
        _boundarySprite.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            _enemyDetected.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            _enemyDetected.Remove(enemy);
    }
}
