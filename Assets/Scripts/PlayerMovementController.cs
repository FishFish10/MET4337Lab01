using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float gravity = 20.0f;

    private Vector2 m_beginPos;
    private Vector2 m_dragPos;

    private float m_speed;

    private Vector3 m_moveDirection;

    private float m_rotateAngle;

    private CharacterController m_charCtrller;

    [SerializeField]
    private float m_speedOffset = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        m_charCtrller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch currentTouch = Input.touches[0];

            if (currentTouch.phase == TouchPhase.Began)
            {
                m_beginPos = currentTouch.position;
            }

            if (currentTouch.phase == TouchPhase.Moved)
            {
                m_dragPos = currentTouch.position;

                m_speed = (m_dragPos - m_beginPos).magnitude;

                m_rotateAngle = Mathf.Atan2
                    ((m_dragPos.y - m_beginPos.y), (m_dragPos.x - m_beginPos.x)) * Mathf.Rad2Deg;

                transform.eulerAngles = new Vector3(0f, m_rotateAngle, 0f);

                if (m_rotateAngle <= 180)
                    m_rotateAngle -= 360;
            }

            if (currentTouch.phase == TouchPhase.Ended)
            {
                m_rotateAngle = 0f;
                m_speed = 0f;
            }

            if (m_charCtrller.isGrounded)
            {
                m_moveDirection = transform.forward * Time.deltaTime * m_speed * m_speedOffset;
            }

            m_moveDirection.y -= gravity * Time.deltaTime;

            m_charCtrller.Move(m_moveDirection);

            this.GetComponent<Animator>().SetInteger("AnimationPar", m_speed > 5f ? 1 : 0);
        }
    }
}
