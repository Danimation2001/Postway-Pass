using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour
{
    bool _canBePressed; //can the note be hit?
    bool _beenHit; //has the note been hit?
    public InputAction hit;
    ButtonControllerv2 player;

    void OnEnable()
    {
        hit.Enable(); 
    }

    void OnDisable()
    {
        hit.Disable();
    }

    private void Start()
    {
        player = GameObject.Find("Buttons Controller").GetComponent<ButtonControllerv2>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the correct button is pressed and the note can be hit, hit
        if (hit.WasPressedThisFrame())
        {
            if (_canBePressed)
            {
                HitNote();
            }
        }
    }

    //if a note is over the hit line, allow it to be hit
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            _canBePressed = true;
        }
    }

    //if the note leaves the hit line without being hit, miss.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator" && !_beenHit)
        {
            _canBePressed = false;
            MissNote();
        }
    }

    void HitNote()
    {
        _beenHit = true;

        //gain health and destroy
        player.TakeDamage(-10);
        Destroy(gameObject);
    }

    void MissNote()
    {
        // lose health and destroy
        player.TakeDamage(10);
        Destroy(gameObject);
    }
}
