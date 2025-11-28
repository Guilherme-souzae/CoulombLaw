using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeManager : MonoBehaviour
{
    public GameObject chargePrefab;

    private List<Rigidbody2D> charges = new List<Rigidbody2D>();

    public static ChargeManager Instance { get; private set; }

    // ---------- ESCALONAMENTOS ----------
    private const float MASS_SCALE = 1e30f;
    private const float CHARGE_SCALE = 1e19f;

    // Constante artificial reforçada — ideal para Unity
    private const float K = 150f;

    // Distância mínima para evitar explosões numéricas
    private const float MIN_DIST = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnCharge(+1.6e-19f, 9.11e-31f);
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SpawnCharge(-1.6e-19f, 9.11e-31f);
        }

        Debug.Log(charges.Count);
    }


    private void SpawnCharge(float realCharge, float realMass)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        GameObject newObj = Instantiate(chargePrefab, mousePos, Quaternion.identity);
        Rigidbody2D rb = newObj.GetComponent<Rigidbody2D>();
        Charge chargeScript = newObj.GetComponent<Charge>();

        float scaledCharge = realCharge * CHARGE_SCALE; // ~ ±1
        float scaledMass = realMass * MASS_SCALE;   // ~ 9.11

        chargeScript.Initialize(scaledCharge, scaledMass);

        rb.mass = scaledMass;
        rb.gravityScale = 0f;

        charges.Add(rb);
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < charges.Count; i++)
        {
            Rigidbody2D a = charges[i];
            Charge ca = a.GetComponent<Charge>();

            for (int j = i + 1; j < charges.Count; j++)
            {
                Rigidbody2D b = charges[j];
                Charge cb = b.GetComponent<Charge>();

                Vector2 dir = b.position - a.position;
                float dist = dir.magnitude;

                if (dist < MIN_DIST) dist = MIN_DIST;

                float forceMag = K * (ca.charge * cb.charge) / (dist * dist);
                Vector2 force = -dir.normalized * forceMag;

                // Ação e reação
                a.AddForce(force);
                b.AddForce(-force);
            }
        }
    }

    public void RemoveCharge(Rigidbody2D charge)
    {
        charges.Remove(charge);
    }
}
