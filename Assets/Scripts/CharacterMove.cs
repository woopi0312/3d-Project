using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Animator _ani;
    float _moveValue = 0;
    [SerializeField] Transform _cam;
    [SerializeField] Collider _sword;
    [SerializeField] GameObject _gameoverUI;
    [SerializeField] Inventory _inven;

    int HP = 5;
    int _coin = 0;
    private void Awake()
    {
        _ani= GetComponent<Animator>();
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(_cam.transform.forward.x,0,_cam.transform.forward.z));
        float vX = Input.GetAxisRaw("Horizontal");
        float vZ = Input.GetAxisRaw("Vertical");
        float vY = GetComponent<Rigidbody>().velocity.y;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 v3 = forward * vZ + right * vX;
        Vector3 vYz = v3 * 4.5f;
        vYz.y += vY;
        GetComponent<Rigidbody>().velocity = vYz;

        _ani.SetFloat("AxisX", vX);
        _ani.SetFloat("AxisZ", vZ);

        if (Input.GetMouseButtonDown(0))
        {
            _sword.enabled = true;
            _ani.SetTrigger("Attack");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _ani.SetBool("Sprint", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _ani.SetBool("Sprint", false);
        }
        if (Input.GetKey(KeyCode.H))
        {
            _ani.SetTrigger("Hit");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _ani.SetTrigger("Jump");
        }
        //if (v3 != Vector3.zero) transform.rotation = Quaternion.LookRotation(v3);
    }

    bool canHitted = true;
    public void Hitted()
    {
        if (!canHitted) return;
        HP--;
        if (HP < 0)
        {
            _ani.Play("Dead");
            _gameoverUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _ani.Play("Hit");
        }
        canHitted = false;
        StartCoroutine(CoHittedCoolTime());
    }

    IEnumerator CoHittedCoolTime()
    {
        yield return new WaitForSeconds(1f);
        canHitted = true;
    }
    void EndAttack()
    {
        _sword.enabled = false;
    }

    public void AddCoin()
    {
        Item item = new Item();
        int count = Random.Range(1, 100);
        EItemType eType = (EItemType)Random.Range(1,(int)EItemType.Max -1) ;
        item._eType = eType;
        item._Count = count;
        _inven.AddItem(item);
        
    }
}
