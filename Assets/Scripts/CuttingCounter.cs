using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {

            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
            else
            {
                // player isn't carring something
            }

        }
        else
        {
            // Counter is not empty

            if (player.HasKitchenObject())
            {
                // player is carring something
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

    public override void InteractAlternate(Player player)
    {

        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO kitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
        }

    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        if (GetOutputForInput(inputKitchenObjectSO))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipeSOs)
        {
            if (recipeSO.input == inputKitchenObjectSO)
            {
                return recipeSO.output;
            }
        }

        return null;
    }
}
