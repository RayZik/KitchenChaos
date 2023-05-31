using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private const float ROTATE_SPEED = 10f;
    private const float PLAYER_HEIGHT = 2f;
    private const float PLAYER_RADIUS = .7f;
    private const float PLAYER_INTERACT_DISTANCE = 2f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchetObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Player instance");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementNormalizedVector();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }


        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, PLAYER_INTERACT_DISTANCE, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)
                {
                    SetSelectedClearCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedClearCounter(null);
            }
        }
        else
        {
            SetSelectedClearCounter(null);
        }
    }

    private void SetSelectedClearCounter(BaseCounter baseCounter)
    {
        selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = baseCounter
        });
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementNormalizedVector();

        Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);

        float movieDistance = Time.deltaTime * moveSpeed;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, moveDirection, movieDistance);


        if (!canMove)
        {
            Vector3 moveDirectionX = new(moveDirection.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, moveDirectionX, movieDistance);

            if (canMove)
            {
                moveDirection = moveDirectionX.normalized;
            }
            else
            {
                Vector3 moveDirectionZ = new(0, 0, moveDirection.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, moveDirectionZ, movieDistance);

                if (canMove)
                {
                    moveDirection = moveDirectionZ.normalized;
                }
            }
        }

        if (canMove)
        {
            transform.position += movieDistance * moveDirection;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * ROTATE_SPEED);

        isWalking = moveDirection != Vector3.zero;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchetObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
