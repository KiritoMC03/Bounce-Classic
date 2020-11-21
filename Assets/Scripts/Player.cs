using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDead;

    private Rigidbody2D _rigidbody;
    private Transform _spawnPosition;
    [SerializeField] private float _baseSpeed = 1f;
    [SerializeField] private float _baseJumpForce = 1f;
    [SerializeField] private float _bySizeJumpModifier = 1.16f;
    private float _speed;
    private float _jumpForce;
    private bool isGrounded = false;
    private bool _isJump = false;
    [SerializeField] private bool _isSmall = true;
    [SerializeField] private GameObject _smallBallSprite;
    [SerializeField] private GameObject _bigBallSprite;
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private GameObject _popController;

    private bool _allInitializate = false;

    private void Awake()
    {
        _speed = _baseSpeed * 7.2f /*0.12f*/;
        _jumpForce = _baseJumpForce * 1.44f /*2.4f*/;
    }

    void Start()
    {
        SetStartedParams();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
		{
            _isJump = false;
		}	
		
        Move();
    }

    private void FixedUpdate()
    {
        if (CheckKeyDown(KeyCode.Space))
        {
            _isJump = true;
        }

        if (_isJump && isGrounded)
        {
            Jump();
        }
    }

    private void SetStartedParams()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spawnPosition = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.position = _spawnPosition.position;

        if(_gameManager != null)
        {
            ChangeSize(_gameManager.GetComponent<GameManager>().GetIsStartingSizeSmall(), false);
        }
    }

    private void Move()
    {
		var input = Input.GetAxis("Horizontal");
        var offset = new Vector3(_speed * input, 0, 0);
        var booferVelocity = _rigidbody.velocity;

        if (input == 0)
		{
            booferVelocity.x = 0;
		}
        else
        {
            booferVelocity.x = _speed * input;
        }
        _rigidbody.velocity = booferVelocity;
    }
    
    private void Jump()
    {
        var offset = new Vector2(0, _jumpForce * 2);
        
        var booferVelocity = _rigidbody.velocity;
        booferVelocity.y = 0;
        _rigidbody.velocity = booferVelocity;

        _rigidbody.AddRelativeForce(offset);
    }

    public void Respawn()
    {
        StopCoroutine(Respawner());

        _spawnPosition = GameObject.FindGameObjectWithTag("Respawn").transform;

        //ChangeLocalScaleTo(gameObject, 0);
        var tempScale = gameObject.transform.localScale;
        tempScale.x = 0;
        gameObject.transform.localScale = tempScale;

        if (_spawnPosition != null)
        {
            transform.position = _spawnPosition.transform.position;

            if (_gameManager != null)
            {
                var isStartingSizeSmall = _gameManager.GetComponent<GameManager>().GetIsStartingSizeSmall();

                if (isStartingSizeSmall)
                {
                    ChangeSize(true, false);
                }
                else if (isStartingSizeSmall == false)
                {
                    ChangeSize(false, true);
                }


                if (_popController != null)
                {
                    StartCoroutine(Respawner());
                }
            }
        }
    }

    IEnumerator Respawner()
    {
        yield return new WaitForSecondsRealtime(_popController.GetComponent<PopController>()._popShowTime);

        //ChangeLocalScaleTo(gameObject, 1);


        var tempScale = gameObject.transform.localScale;
        tempScale.x = 1;
        gameObject.transform.localScale = tempScale;

        StopCoroutine(Respawner());
    }

    public void Died()
    {
        //ToDo: сделать нормальную механику;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSize(bool toSmall, bool changeJumpForce = false)
    {
        // ToDo: оптимизировать через класс объекта?
        if (toSmall)
        {
            _bigBallSprite.SetActive(false);
            _smallBallSprite.SetActive(true);
            _isSmall = true; // Не выноси это за условный блок.
        }
        else
        {
            _smallBallSprite.SetActive(false);
            _bigBallSprite.SetActive(true);
            _isSmall = false; // Аналогично.
        }

        if (toSmall && changeJumpForce)
        {
            _jumpForce /= _bySizeJumpModifier;
            return;
        }
        else if(!toSmall && changeJumpForce)
        {
            _jumpForce *= _bySizeJumpModifier;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Life")
        {
            LifeCountManager.ChangeCountTo(1);
            Destroy(collision.gameObject);
        }

        if(collision.transform.tag == "Portal")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ground" || collision.transform.tag == "RingEdge" || collision.transform.tag == "Deflater")
        {
            isGrounded = true;
        }

        if(collision.transform.tag == "Thorn")
        {
            if(LifeCountManager.GetCount() > 1)
            {
                LifeCountManager.ChangeCountTo(-1);
                OnPlayerDead?.Invoke();
                Respawn();
                return;
            }
            else
            {
                OnPlayerDead?.Invoke();
                Died();
                return;
            }
        }

        if(collision.transform.tag == "Pumper" && _isSmall)
        {
            ChangeSize(false, true);
        }
        else if (collision.transform.tag == "Deflater" && !_isSmall)
        {
            ChangeSize(true, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "RingEdge")
        {
            isGrounded = false;
        }
    }
}
