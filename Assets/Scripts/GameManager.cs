using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    private GameObject _pop;

    private void Start()
    {
        Debug.Log("!!! START !!!");
        StopAllCoroutines();
        Time.timeScale = 1;

        _player = GameObject.FindGameObjectWithTag("Player");
        _pop = GameObject.FindGameObjectWithTag("Pop");


        if(_pop != null)
        {
            //_pop.SetActive(false);
            _pop.transform.Translate(0, 100f, 0);
        }
    }

    private void Update()
    {

    }

    public void ShowPop()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        if (_pop != null && _player != null)
        {
            Debug.Log("ShowPop If");
            //_pop.SetActive(true);
            _player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(PopShower());
        }
        Debug.Log("ShowPop End");
    }

    IEnumerator PopShower()
    {
        Debug.Log("PopShower Start");
        Time.timeScale = 0;
        if (_player == null)
        {
            yield return new WaitForSeconds(1);
            StopCoroutine(PopShower());
        }

        _pop.transform.position = _player.transform.position;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        _pop.transform.Translate(0, 100f, 0);
        Debug.Log("PopShower End");
    }
}
