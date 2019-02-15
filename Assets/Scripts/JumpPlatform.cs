using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public AudioClip jumpSound;

    public float jumpMangnitude = 30;

    public void ControllerEnter2D (CharacterController controller)
    {
        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        controller.SetVerticalForce(jumpMangnitude);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
