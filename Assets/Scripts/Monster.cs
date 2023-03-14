using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EMonState
{
    None,
    Idle,
    Move,
    Attack,
    Die,
}

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject _mon;
    [SerializeField] Transform _target;
    [SerializeField] GameObject _coin;
    [SerializeField] GameObject _attackSpace;
    EMonState _eState = EMonState.None;

    Animator _ani;
    int _hp = 0;

    float _searchDis = 10;
    float _attackDis = 2;
    float _speed = 2;
    void Start()
    {
        _ani = _mon.GetComponent<Animator>();
        StartCoroutine(CoSpawn());

    }

    IEnumerator CoSpawn()
    {
        int rand = Random.Range(2, 5);
        yield return new WaitForSeconds(rand);
        Spawn();
    }

    void Spawn()
    {
        _hp = 5;
        _mon.SetActive(true);
        _eState = EMonState.Idle;

    }
    private void Update()
    {
        if (_eState == EMonState.Idle) moveAndSearch();
        if (_eState == EMonState.Attack) followAndAttack();
        _attackSpace.SetActive(isAttackStat());
    }

    bool isAttackStat()
    {
        return _eState == EMonState.Attack;
    }

    void followAndAttack()
    {
        if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Hitted") == false || _ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
        }
        _ani.Play("Attack");
        float dis = Vector3.Distance(_target.position, transform.position);
        if (dis > _attackDis)
        {
            if (dis > _searchDis)
            {
                _eState = EMonState.Idle;
            }
            else
            {
                Vector3 lookDir = _target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            }
        }
    }
    void moveAndSearch()
    {
        _ani.Play("Idle");
        float dis = Vector3.Distance(_target.position, transform.position);
        if (dis < _searchDis)
        {
            if (dis < _attackDis)
            {
                _eState = EMonState.Attack;
            }
            else
            {
                Vector3 lookDir = _target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
                // transform.translate((target.position - transform.position) * time.deltatime * _speed);
                //transform.position += (target.position - transform.position) * time.deltatime * _speed);
            }
        }
        else
        {
            _eState = EMonState.Move;
            StartCoroutine(CoRandomMove());
        }
    }

    IEnumerator CoRandomMove()
    {
        {
            bool canMove = false;
            RaycastHit hit;
            Vector3 targetDir = Vector3.zero;
            while (!canMove)
            {
                Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                transform.rotation = Quaternion.LookRotation(randomDir);

                yield return new WaitForSeconds(1f);
                targetDir = transform.position + transform.forward * 2;//movedir

                Vector3 front = transform.position + transform.forward + new Vector3(0, 1f, 0);

                Debug.DrawRay(front, transform.forward * 2, Color.red, 5);

                if (Physics.Raycast(front, transform.forward * 2, out hit, 2))
                {
                    Debug.Log("hit collider : " + hit.collider.name);
                }
                else
                {
                    canMove = true;
                }
            }


            Debug.DrawRay(targetDir + new Vector3(0, 0.4f, 0), new Vector3(0, -2f, 0), Color.red, 5);
            if (Physics.Raycast(targetDir + new Vector3(0, 0.5f, 0), new Vector3(0, -2f, 0), out hit))
            {
                while (Vector3.Distance(transform.position, targetDir) > 0.1)
                {
                    _ani.Play("Move");
                    transform.position = Vector3.MoveTowards(transform.position, targetDir, Time.deltaTime * _speed);
                    yield return null;

                }
            }
            yield return new WaitForSeconds(1f);
            _eState = EMonState.Idle;
        }

    }

    public void hitted()
    {
        if (!canHitted) return;
        _hp--;
        if (_hp <= 0)
        {
            _ani.Play("Die");
            _eState = EMonState.Die;
        }
        else
        {
            _ani.Play("Hitted");
        }
        canHitted = false;
        StartCoroutine(CoHittedCollTime());
    }

    bool canHitted = true;

    IEnumerator CoHittedCollTime()
    {
        yield return new WaitForSeconds(1f);
        canHitted = true;
    }
    public void DieEnd()
    {
        _mon.gameObject.SetActive(false);
        GameObject tmp = Instantiate(_coin);
        tmp.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hero"))
        {
            other.GetComponent<CharacterMove>().Hitted();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hero"))
        {
            other.GetComponent<CharacterMove>().Hitted();
        }
    }
}