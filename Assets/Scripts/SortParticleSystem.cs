using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortParticleSystem : MonoBehaviour
{

    public string LayerName = "Particles";

    // Start is called before the first frame update
    void Start()
    {

        this.GetComponent<Renderer>().sortingLayerName = LayerName;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
