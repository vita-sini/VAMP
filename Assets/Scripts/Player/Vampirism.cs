using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private VampirizmDetected _vampirizmDetected;

    public TMP_Text CooldownVampirizmText;

    private Player _player;

    private float _healthTransferRate = 1f;
    private float _abilityDuration = 6f;
    private float _cooldownDuration = 10f;
    private float _currentCooldownDuration;

    private Coroutine _coroutineVampirism;

    private bool _isColldowned = false;

    private void Start()
    {
        _currentCooldownDuration = _cooldownDuration;
        CooldownVampirizmText.text = _currentCooldownDuration.ToString();
        _player = GetComponent<Player>();
    }

    public void ActivatingAbility()
    {
        if (_isColldowned == false)
        {
            StartCoroutine(Cooldown());

            if (_coroutineVampirism != null)
                StopCoroutine(_coroutineVampirism);

            _coroutineVampirism = StartCoroutine(TransferHealth());
        }
    }

    private void ApplyVampirism(float healthToTransfer, Enemy enemy)
    {
        if (enemy != null)
        {
            if (_player.CurrentHealth <= _player.HealthMax && enemy.CurrentHealth > 0)
                _player.Healing(healthToTransfer);

            if (enemy.CurrentHealth > 0)
                enemy.ApplyDamage(healthToTransfer);
        }
    }

    private Enemy GetNearestEnemy()
    {
        if (_vampirizmDetected.EnemyDetected.Count == 0)
            return null;

        List<Enemy> enemies = _vampirizmDetected.EnemyDetected;
        Enemy enemy = enemies[0];

        for (int i = 1; i < enemies.Count; i++)
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < Vector3.Distance(transform.position, enemies[i - 1].transform.position))
                enemy = enemies[i];

        return enemy;
    }

    private IEnumerator TransferHealth()
    {
        float elapsedTime = 0;
        _vampirizmDetected.RenderSprite();

        while (elapsedTime < _abilityDuration)
        {
            elapsedTime += Time.deltaTime;

            float healthToTransfer = _healthTransferRate * Time.deltaTime;

            Enemy enemy = GetNearestEnemy();

            if (enemy != null)
                ApplyVampirism(healthToTransfer, enemy);

            yield return null;
        }

        _vampirizmDetected.DeactivationSprite();
    }

    private IEnumerator Cooldown()
    {
        float delay = 1f;
        _currentCooldownDuration = 0;
        _isColldowned = true;

        var wait = new WaitForSeconds(delay);

        while (_currentCooldownDuration < _cooldownDuration)
        {
            _currentCooldownDuration += delay;

            CooldownVampirizmText.text = $"{((int)_currentCooldownDuration)}";

            yield return wait;
        }

        _isColldowned = false;
    }
}
