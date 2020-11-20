using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnRingCountEnd;

    private GameObject _player;
    private GameObject _pop;
    internal float _popShowTime = 1f;
    [SerializeField] internal bool isStartingSizeSmall = true;
    private int _ringsCount;
    [SerializeField] private Transform _ringCounter;
    [SerializeField] private GameObject _ringImage;
    [SerializeField] private float _ringWidth = 0.35f;
    private List<GameObject> _ringImages = new List<GameObject>();

    private void Start()
    {
        StopAllCoroutines();
        Time.timeScale = 1;

        _player = GameObject.FindGameObjectWithTag("Player");
        _pop = GameObject.FindGameObjectWithTag("Pop");
        if(_pop != null)
        {
            _pop.transform.Translate(0, 100f, 0);
        }

        CreateRingImages();
    }

    private void Update()
    {
        if(_ringsCount <= 0)
        {
            OnRingCountEnd?.Invoke();
        }
    }

    public void ShowPop()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        if (_pop != null && _player != null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(PopShower());
        }
    }

    IEnumerator PopShower()
    {
        Time.timeScale = 0;
        if (_player == null)
        {
            yield return new WaitForSeconds(_popShowTime);
            StopCoroutine(PopShower());
        }

        _pop.transform.position = _player.transform.position;
        yield return new WaitForSecondsRealtime(_popShowTime);
        Time.timeScale = 1;
        _pop.transform.Translate(0, 100f, 0);
    }

    public bool GetIsStartingSizeSmall()
    {
        return isStartingSizeSmall;
    }

    public void CreateRingImages()
    {
        _ringsCount = GameObject.FindGameObjectsWithTag("Ring").Length;

        for (int i = 0; i < _ringsCount; i++)
        {
            if (_ringCounter != null)
            {
                var newRingImage = Instantiate(_ringImage, _ringCounter);
                newRingImage.transform.Translate((_ringWidth + _ringWidth / 5) * _ringImages.Count, 0, 0);
                _ringImages.Add(newRingImage);
                Debug.Log(_ringImages.Count);
            }
        }
    }

    public void AddRingToScore()
    {
        _ringsCount--;
        Debug.Log(_ringImages.Count);
        Destroy(_ringImages[(_ringImages.Count) - 1]);
        _ringImages.Remove(_ringImages[_ringImages.Count - 1]);
    }
}
