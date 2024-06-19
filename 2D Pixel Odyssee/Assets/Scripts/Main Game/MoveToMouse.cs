using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public static List<MoveToMouse> moveableObjects = new List<MoveToMouse>();
    public float speed = 5f;
    public Vector3 target;
    private bool selected;

    void Awake()
    {
        moveableObjects.Clear();
    }

    void Start()
    {
        moveableObjects.Add(this);
        target = transform.position;
    }

    void Update()
    {
        // Change to right mouse button (index 1)
        if (Input.GetMouseButtonDown(1) && selected)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            target.y = transform.position.y;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    // Change to right mouse button (index 1)
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selected = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            foreach (MoveToMouse obj in moveableObjects)
            {
                if (obj != this)
                {
                    obj.selected = false;
                    obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            var cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
            if (cinemachine != null)
            {
                cinemachine.Follow = this.transform;
            }
        }
    }
}
