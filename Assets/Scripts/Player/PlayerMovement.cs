using Unity.Netcode;
using UnityEngine;


public class PlayerMovement : NetworkBehaviour
{
    private PlayerControls controls;
    
    [SerializeField] private Player player;
    [SerializeField] private CharacterController characterController;
    
    [SerializeField] private Transform bodyTransform;
    [Header("Movement Info")]
    [SerializeField] private float speed = 20f;

    [SerializeField] private float turningRate = 30f;
        
        
    private Vector2 movementDirection;

    public Vector2 moveInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        AssignInputEvents();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            ApplyMovement();

        }
    
    }


    private void ApplyMovement()
    {
        movementDirection = new Vector2(moveInput.x,moveInput.y);
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
            float zRotation = movementDirection.x * -turningRate * Time.deltaTime;
            bodyTransform.Rotate(0f,0f,zRotation);
        }
    }

    private void AssignInputEvents()
    {
        controls = player.controls;
        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

    }
}
