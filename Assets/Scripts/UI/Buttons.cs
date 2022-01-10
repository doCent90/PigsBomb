using System;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] private Button _fire;

    public event Action FireClicked;

    private void OnEnable()
    {
        if (_fire == null)
            throw new NullReferenceException(nameof(Buttons));

        _fire.onClick.AddListener(OnFire);
    }

    private void OnDisable()
    {
        _fire.onClick.RemoveListener(OnFire);
    }

    private void OnFire()
    {
        FireClicked?.Invoke();
    }
#if UNITY_EDITOR

    private float _spentTime;
    private float _delay = 1f;
    private bool _hasClicked = false;

    private void Update()
    {
        if (!_hasClicked)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                OnFire();
                _hasClicked = true;
            }
        }
        else
        {
            _spentTime += Time.deltaTime;

            if(_spentTime > _delay)
            {
                _spentTime = 0;
                _hasClicked = false;
            }
        }
    }
#endif
}
