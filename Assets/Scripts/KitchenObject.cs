using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }

        kitchenObjectParent = newKitchenObjectParent;

        newKitchenObjectParent.SetKitchenObject(this);


        transform.parent = newKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
