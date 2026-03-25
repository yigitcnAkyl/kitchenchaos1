using System;
using UnityEngine;

    public class BaseCounter : MonoBehaviour, IkitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlacedHere;

    public static void resetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
  
        [SerializeField] private Transform TopCounterPoint;

        private KitchenObjects kitchenObjects;


        public virtual void Interact(PlayerMovement playerMovement)
        {
            Debug.LogError("BaseCounter.Interact();");
        }


        public virtual void InteractAlternate(PlayerMovement playerMovement)
        {
            //Debug.LogError("BaseCounter.InteractAlternate();");
        }
        public Transform GetKitchenObjectFollowTransform()
        {
            return TopCounterPoint;
        }

        public void SetKitchenObject(KitchenObjects kitchenObjects)
        {
            this.kitchenObjects = kitchenObjects;
        if (kitchenObjects != null) 
        {
            OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty);
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
