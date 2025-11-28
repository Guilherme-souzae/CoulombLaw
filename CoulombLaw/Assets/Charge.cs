using UnityEngine;

public class Charge : MonoBehaviour
{
    [Tooltip("Carga elétrica simulada, já escalonada pelo ChargeManager.")]
    public float charge;

    [Tooltip("Massa simulada, já escalonada pelo ChargeManager.")]
    public float simulatedMass;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private SphereCollider collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Chamado logo após a instância da prefab
    public void Initialize(float scaledCharge, float scaledMass)
    {
        this.charge = scaledCharge;
        this.simulatedMass = scaledMass;

        rb.mass = scaledMass;   // massa escalonada

        // A cor continua igual
        sr.color = scaledCharge > 0 ? Color.red : Color.blue;
    }

    private void OnBecameInvisible()
    {
        ChargeManager.Instance.RemoveCharge(rb);
        Destroy(gameObject);
    }
}
