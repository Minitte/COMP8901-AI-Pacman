using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour
{
    public delegate void BitDelegate();

    public static event BitDelegate OnBitCollected;

    public static event BitDelegate OnBitPowerCollected;

    public bool isPower;

    private void LateUpdate()
    {
        if (isPower) this.transform.Rotate(Vector3.forward, 30);
        else this.transform.Rotate(Vector3.forward, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (isPower) OnBitPowerCollected?.Invoke();
        else OnBitCollected?.Invoke();

        Destroy(this.gameObject);
    }
}
