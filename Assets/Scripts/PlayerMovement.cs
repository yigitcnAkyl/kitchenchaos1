using System.Net.Http.Headers;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEditor.Timeline;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour , IkitchenObjectParent
{

    
    public static PlayerMovement Instance { get;  private set; }



    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private Image sprintCooldownImage;
    [SerializeField] private Image sprintCooldownImageBackground;


    private bool isWalking;

    
    private bool isSprinting;
    private float sprintDuration = 0.5f; 
    private float sprintTimer;
    private float sprintSpeedMultiplier = 2f;

    
    private bool sprintOnCooldown;
    private float sprintCooldown = 5f; 
    private float sprintCooldownTimer;


    private Vector3 lastInteractionDir;
    private BaseCounter selectedCounter;
    private KitchenObjects kitchenObjects;

    private void Awake()
    { if (Instance != null)
        {
            Debug.Log("there is more than 1 player instance");
                }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnSprintAction += GameInput_OnSprintAction;
    }

    private void GameInput_OnSprintAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()) return;

        
        if (isSprinting || sprintOnCooldown) return;

       
        isSprinting = true;
        sprintTimer = sprintDuration;

        
        sprintOnCooldown = true;
        sprintCooldownTimer = sprintCooldown;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0)
            {
                isSprinting = false;
            }
        }

      
        if (sprintOnCooldown && !isSprinting)
        {
            sprintCooldownTimer -= Time.deltaTime;
            if (sprintCooldownTimer <= 0)
            {
                sprintOnCooldown = false;
            }
        }
        
        
        if (sprintOnCooldown)
        {
            sprintCooldownImageBackground.gameObject.SetActive(true);
            sprintCooldownImage.gameObject.SetActive(true);
            sprintCooldownImage.fillAmount = sprintCooldownTimer / sprintCooldown;
        }
        else
        {
            sprintCooldownImageBackground.gameObject.SetActive(false);
            sprintCooldownImage.gameObject.SetActive(false);
        }


        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();


        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero) { lastInteractionDir = moveDir; }
        float interactDis = 2f;

        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDis, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //has clearcounter
                if (baseCounter != selectedCounter)
                {
                   SetSelectedCounter(baseCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }

        }
        else {
            SetSelectedCounter(null);
        }
       
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();


        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float playerHeight = 2f;

        float playerRadius = 1.0f;
        float currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed *= sprintSpeedMultiplier;
        }
        float moveDistance = currentSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);



        if (!canMove)
        {
            //Cannot move to moveDir
            //attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can move only X 
                moveDir = moveDirX;
            }
            else
            {
                //cannot move on the x 
                // attempt move only z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                { //cannot move any dir
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }



        isWalking = moveDir != Vector3.zero;


        float rotateSpeed = 10f;


        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    private void SetSelectedCounter(BaseCounter selectedCounter) { 
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObjects kitchenObjects)
    {
        this.kitchenObjects = kitchenObjects;
        if (kitchenObjects != null) 
        {
        OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObjects GetKitchenObjects()
    {

        return kitchenObjects;
    }

    public void ClearKitchenObject()
    {
        kitchenObjects = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObjects != null;
    }
}
