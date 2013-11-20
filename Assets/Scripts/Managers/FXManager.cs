using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour
{
    public GameObject FX02;

    public void createFX02(Vector3 pPosition)
    {
        FX02 fx = (GameObject.Instantiate(FX02, pPosition, Quaternion.identity) as GameObject).GetComponent<FX02>();
        fx.init();
    }
}
