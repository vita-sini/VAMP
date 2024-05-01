using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2 _detectorSize = Vector2.one;
    [SerializeField] private SpriteRenderer _boundarySprite;
    [SerializeField] private VampirizmDetected _vampirizmDetected;

    public TMP_Text CooldownVampirizmText;

    private Enemy _enemy;
    private Player _player;

    private float _healthTransferRate = 1f;
    private float _abilityDuration = 6f;
    private float _cooldownDuration = 10f;
    private float _currentCooldownDuration;

    private Coroutine _coroutineVampirism;
    private Coroutine _coroutineCooldown;

    private bool _isAbilityActive = false;

    private void Start()
    {
        _currentCooldownDuration = _cooldownDuration;
        CooldownVampirizmText.text = _currentCooldownDuration.ToString();
        _player = GetComponent<Player>();
    }

    public void ActivatingAbility()
    {
        if (_isAbilityActive == false && _coroutineCooldown == null)
        {
            RenderSprite();

            _coroutineCooldown = StartCoroutine(Cooldown());

            if (_vampirizmDetected.EnemyDetected.Count > 0 && _coroutineVampirism == null)
            {
                _coroutineVampirism = StartCoroutine(TransferHealth());
            }
            else if (_vampirizmDetected.EnemyDetected.Count <= 0)
            {
                StopVampirism();
                DeactivationSprite();
            }
        }
    }

    private void RenderSprite()
    {
        _boundarySprite.gameObject.SetActive(true);
    }

    private void DeactivationSprite()
    {
        _boundarySprite.gameObject.SetActive(false);
    }

    private void ApplyVampirism(float healthToTransfer, Enemy enemy)
    {
        if (enemy != null)
        {
            _player.Healing(healthToTransfer);
            enemy.ApplyDamage(healthToTransfer);
        }
    }

    private void StopVampirism()
    {
        if (_coroutineVampirism != null)
        {
            StopCoroutine(_coroutineVampirism);
            _coroutineVampirism = null;
        }
    }

    private void StopCooldown()
    {
        if (_coroutineCooldown != null)
        {
            StopCoroutine(_coroutineCooldown);
            _coroutineCooldown = null;
        }
    }

    private IEnumerator TransferHealth()
    {
        float elapsedTime = 0;
        _isAbilityActive = true;

        while (_vampirizmDetected.EnemyDetected.Count > 0)
        {
            for (int i = 0; i < _vampirizmDetected.EnemyDetected.Count; i++)
            {
                var enemy = _vampirizmDetected.EnemyDetected[i];
                while (elapsedTime < _abilityDuration && _player.CurrentHealth < _player.HealthMax && enemy.CurrentHealth > 0)
                {
                    elapsedTime += Time.deltaTime;

                    float healthToTransfer = _healthTransferRate * Time.deltaTime;

                    ApplyVampirism(healthToTransfer, enemy);

                    yield return new WaitWhile(() => _vampirizmDetected.EnemyDetected.Count <= 0);
                }
            }
        }

        DeactivationSprite();
        _isAbilityActive = false;
        StopVampirism();
    }

    private IEnumerator Cooldown()
    {
        _currentCooldownDuration = 0;

        while (_currentCooldownDuration < _cooldownDuration)
        {
            _currentCooldownDuration += Time.deltaTime;

            CooldownVampirizmText.text = ((int)_currentCooldownDuration).ToString();

            yield return null;
        }

        StopCooldown();
    }
}
