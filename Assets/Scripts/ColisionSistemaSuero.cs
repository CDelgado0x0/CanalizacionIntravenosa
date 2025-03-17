using Oculus.Interaction;
using UnityEngine;

public class ColisionSistemaSuero : MonoBehaviour
{
    public Transform puntoColocacion; // Lista de puntos de colocaci�n
    private GameObject sistema;
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SistemaSuero"))
        {
            sistema = other.gameObject; // Guardamos referencia al bote
            rb = sistema.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.LockKinematic();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SistemaSuero") && sistema == other.gameObject)
        {
            if (rb != null)
            {
                rb.UnlockKinematic();
            }
            sistema = null; // Si el bote sale, lo olvidamos
        }
    }

    public void SoltarBote()
    {
        if (sistema != null)
        {
            sistema.GetComponent<SistemaGrabbable>().enabled = false;


            sistema.transform.parent = puntoColocacion;
            sistema.transform.localPosition = Vector3.zero;

            sistema.transform.rotation = puntoColocacion.rotation;

            sistema.GetComponent<SistemaGrabbable>().enabled = true;
        }
    }
}
