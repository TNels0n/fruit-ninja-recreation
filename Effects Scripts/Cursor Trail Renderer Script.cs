using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTrailRendererScript : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        // Get the cursor position in world space
        Vector3 cursorPosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;

        // Update the position of the CursorTrail GameObject
        transform.position = cursorPosition;

    }
}
