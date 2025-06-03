using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class ConectorALlave : MonoBehaviour
{
    [SerializeField] private Transform NuevaPosicion;
    [SerializeField] private GameObject Padre;
    [SerializeField] private GameObject colliders; //Estos hay que quitarlos para que el conector del sistema de suero no pueda ser cogido
    [SerializeField] private SoltarConectorSistema soltarConectorSistema;
    private SoltarConectorSistema soltarConectorSistema2;
    private Grabbable grabbable;
    private Grabbable myGrab;
    private ReiniciarPosicion reiniciarPosicion;
    private Rigidbody rb;

    private bool stepDone = false;

    [SerializeField] private ColisionSistemaSuero colisionSistemaSuero;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        myGrab = GetComponent<Grabbable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Llave3Pasos") && colisionSistemaSuero.pinchoConectado && !stepDone)
        {
            //rb.LockKinematic();
            grabbable = other.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                myGrab.enabled = false;
                grabbable.enabled = false; // Desactivar el Grabbable
                StartCoroutine(EsperarUnSegundo());
            }

            /*
            other.transform.SetParent(Padre.transform);
            other.transform.position = NuevaPosicion.position;
            other.transform.rotation = NuevaPosicion.rotation;
            transform.SetParent(other.transform);
            */

            //other.transform.SetParent(Padre.transform);
            other.transform.position = NuevaPosicion.position;
            other.transform.rotation = NuevaPosicion.rotation;

            ConfigurableJoint joint = grabbable.gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = rb;

            //other.GetComponent<Rigidbody>().isKinematic = true;

            // Configuraciones b�sicas
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            // Usar rotaci�n esf�rica (Slerp)
            joint.rotationDriveMode = RotationDriveMode.Slerp;

            JointDrive slerpDrive = new JointDrive
            {
                positionSpring = 10000f,     // Cu�nta fuerza para alinear rotaci�n
                positionDamper = 100f,       // Amortiguaci�n para evitar temblores
                maximumForce = Mathf.Infinity
            };

            joint.slerpDrive = slerpDrive;

            // Desactivar el script ReiniciarPosicion y activar el limite del cable
            reiniciarPosicion = other.GetComponent<ReiniciarPosicion>();
            if (reiniciarPosicion != null)
            {
                reiniciarPosicion.enabled = false;
            }
            if (soltarConectorSistema != null)
            {
                soltarConectorSistema.enabled = true;
            }
            soltarConectorSistema2 = other.GetComponent<SoltarConectorSistema>();
            if (soltarConectorSistema2 != null)
            {
                soltarConectorSistema2.enabled = true;
            }


            colliders.SetActive(false);

            stepDone = true;

            GameManager.controladorAplicacion.CambiarEstadoJuego(GameState.ColocarCompresor);
        }
    }



    IEnumerator EsperarUnSegundo()
    {
        // Esperamos 1 segundo
        yield return new WaitForSeconds(1f);

        // Despu�s de 1 segundo, se ejecuta este c�digo
        grabbable.enabled = true;
        myGrab.enabled = true;
    }
}

