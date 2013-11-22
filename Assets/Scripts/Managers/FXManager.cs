using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour
{
    public GameObject FX02;

    public FX02 createFX02(Vector3 pPosition)
    {
        FX02 fx = (GameObject.Instantiate(FX02, pPosition, Quaternion.identity) as GameObject).GetComponent<FX02>();
        return fx;
    }
}
