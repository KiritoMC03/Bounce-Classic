using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopController : MonoBehaviour
{
    private Transform _player;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPop()
    {
        gameObject.SetActive(true);
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(PopShower());
    }

    IEnumerator PopShower()
    {
        Debug.Log("1 " + _player == null);
        if(_player == null)
        {
            yield return new WaitForSeconds(1);
            StopCoroutine(PopShower());
        }

        transform.position = _player.position;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
