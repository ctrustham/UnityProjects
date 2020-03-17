using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed = 5;

    Rigidbody rigidbody;
    Camera camera;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // -- Object follow mouse for non-ortho camera
        //Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        //float midpoint = (transform.position - camera.transform.position).magnitude * 0.5f;
        //transform.LookAt(mouseRay.origin + mouseRay.direction * midpoint);

        // -- Object follow mouse for ortho camera
        //transform.LookAt(camera.ScreenToWorldPoint(Input.mousePosition));



        Vector3 mousepos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.y)); // camera.transform.position.y is for non-orthigraphic camera -> the distance from the view camera to the ground plane
        transform.LookAt(mousepos + Vector3.up * transform.position.y); // rotate to look at mouse position -> need to make sure the mouse pos is level with the character's pos (so char doesnt need to bend to look at it)

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
