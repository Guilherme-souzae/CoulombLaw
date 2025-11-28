using UnityEngine;

public class Charge : MonoBehaviour
{
    public float charge;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = charge > 0 ? Color.red : Color.blue;
    }

    private void Start()
    {
        charge = 16f;
        rb.mass = 1;
    }
}
