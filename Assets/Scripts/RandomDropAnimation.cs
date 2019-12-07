using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDropAnimation : MonoBehaviour
{
    public bool animationRunning;

    private Vector3 m_originalPos;

    private Rigidbody2D m_rb;

    private bool m_initalized;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_originalPos = this.transform.position;
        m_initalized = true;

        if (animationRunning) RunAnimation();
        else ResetAnimation();
    }

    private void Update()
    {
        if (!animationRunning)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, m_originalPos, Time.deltaTime * 10f);
            this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, Vector3.zero, Time.deltaTime * 10f);
        }
    }

    public void RunAnimation()
    {
        if (animationRunning || !gameObject.activeSelf) return;

        if (!m_initalized)
        {
            m_originalPos = this.transform.position;
            m_rb = GetComponent<Rigidbody2D>();
            m_initalized = true;
        }

        m_rb.bodyType = RigidbodyType2D.Dynamic;
        m_rb.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(1f, 4f));
        m_rb.angularVelocity = Random.Range(-90, 90f);

        this.transform.position = m_originalPos;

        animationRunning = true;
    }

    public void ResetAnimation()
    {
        if (!animationRunning || !gameObject.activeSelf) return;

        if (!m_initalized)
        {
            m_originalPos = this.transform.position;
            m_rb = GetComponent<Rigidbody2D>();
            m_initalized = true;
        }

        m_rb.bodyType = RigidbodyType2D.Static;
        m_rb.velocity.Set(0, 0);

        animationRunning = false;
    }
}
