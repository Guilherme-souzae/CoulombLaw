using UnityEngine;
using UnityEngine.InputSystem;

public class Charge : MonoBehaviour
{
    public float charge;
    public float simulatedMass;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isDragging = false;
    private Vector2 dragOffset;

    [Header("Arrasto com Força")]
    public float dragForce = 40f;
    public float maxDragSpeed = 25f;

    [Header("Atrito Durante Arrasto")]
    public float dragDamping = 8f;  // Quanto maior, mais lento o movimento durante arrasto

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float scaledCharge, float scaledMass)
    {
        charge = scaledCharge;
        simulatedMass = scaledMass;

        rb.mass = scaledMass;
        sr.color = scaledCharge > 0 ? Color.red : Color.blue;
    }

    private void Update()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                isDragging = true;
                dragOffset = rb.position - mouseWorld;
            }
        }

        if (Mouse.current.middleButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 targetPos = mouseWorld + dragOffset;

            Vector2 dir = targetPos - rb.position;

            // Força de arrasto tipo mola
            rb.AddForce(dir * dragForce);

            // ATRITO DURANTE O ARRASTO (damping suave)
            rb.linearVelocity *= Mathf.Exp(-dragDamping * Time.fixedDeltaTime);

            // Limitar velocidade máxima
            if (rb.linearVelocity.magnitude > maxDragSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * maxDragSpeed;
        }
    }

    private void OnBecameInvisible()
    {
        if (ChargeManager.Instance != null)
            ChargeManager.Instance.RemoveCharge(rb);

        Destroy(gameObject);
    }
}
