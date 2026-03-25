using UnityEngine;

public class KitchenObjects : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    private IkitchenObjectParent kitchenObjectParent;

    public KitchenObjectsSO GetKitchenObjectsSO()
    {
        return kitchenObjectsSO;

    }

    public void SetKitchenObjectParent (IkitchenObjectParent kitchenObjectParent)
    {
        if (kitchenObjectsSO != null && this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("this counter already has a kitchen object");    
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent= kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IkitchenObjectParent  GetIkitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }


    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {

        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }else
        {
            
            plateKitchenObject=null;
            return false;
        }
    }


    public static KitchenObjects SpawnKitchenObject(KitchenObjectsSO kitchenObjectsSO, IkitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab);

     KitchenObjects kitchenObjects =   kitchenObjectTransform.GetComponent<KitchenObjects>();
         kitchenObjects.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObjects;
    }
}
