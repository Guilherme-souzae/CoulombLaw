using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeManager : MonoBehaviour
{
    public GameObject chargePrefab;

    private List<GameObject> charges = new List<GameObject>();

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GameObject newCharge = Instantiate(chargePrefab, mousePos, Quaternion.identity);
            charges.Add(newCharge);
        }
    }

    private void FixedUpdate()
    {
        foreach (GameObject charge in charges)
        {
            foreach(GameObject otherCharge in charges)
            {
                if (charge != otherCharge)
                {
                    Vector2 direction = otherCharge.transform.position - charge.transform.position;
                    float distance = direction.magnitude;
                    if (distance > 0)
                    {
                        Charge chargeComponent = charge.GetComponent<Charge>();
                        Charge otherChargeComponent = otherCharge.GetComponent<Charge>();
                        float forceMagnitude = (chargeComponent.charge * otherChargeComponent.charge) / (distance * distance);
                        Vector2 force = -direction.normalized * forceMagnitude;
                        Rigidbody2D rb = charge.GetComponent<Rigidbody2D>();
                        rb.AddForce(force);
                    }
                }
            }
        }
    }
}
