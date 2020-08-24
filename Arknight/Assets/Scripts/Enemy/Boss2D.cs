using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2D : MonoBehaviour
{
    public float movePower = 1f;
    Animator m_Anim;
    SpriteRenderer m_Render;

    Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;


        //if (Input.GetAxisRaw("Horizontal") < 0)
        if (Input.GetKey(KeyCode.LeftArrow))

        {
            m_Render.flipX = true;

            m_Anim.SetBool("walking", true);
            moveVelocity = Vector3.left;
        }
        //       else if (Input.GetAxisRaw("Horizontal") > 0)
        else if (Input.GetKey(KeyCode.RightArrow))

        {
            m_Render.flipX = false;

            m_Anim.SetBool("walking", true);
            moveVelocity = Vector3.right;
        }
        //else if (Input.GetAxisRaw("Vertical" = 1))
        else if (Input.GetKey(KeyCode.UpArrow))

        {
            moveVelocity = Vector3.forward;
            m_Render.flipY = false;

            m_Anim.SetBool("walking", true);

        }

        //else if (Input.GetAxisRaw("Vertical" < 0))
        else if (Input.GetKey(KeyCode.DownArrow))

        {
            moveVelocity = -Vector3.forward;
            //   m_Render.flipY = true;

            m_Anim.SetBool("walking", true);

        }

        else if (Input.GetKey(KeyCode.Space))
        {
            m_Anim.SetBool("dying", true);

        }
        else if (Input.GetKey(KeyCode.Return))
        {
            m_Anim.SetBool("attack", true);


        }
        else

        {
            m_Anim.SetBool("attack", false);

            m_Anim.SetBool("dying", false);

            m_Anim.SetBool("walking", false);

        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
}
