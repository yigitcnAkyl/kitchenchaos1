using UnityEngine;

public interface IkitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();
 
    public void SetKitchenObject(KitchenObjects kitchenObjects);



    public KitchenObjects GetKitchenObjects();


    public void ClearKitchenObject();


    public bool HasKitchenObject();
   
        
        }
