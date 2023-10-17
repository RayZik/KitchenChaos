using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;
    public class OnCuttingProgressChangedEventArgs : EventArgs
    {
        public float progressNormilized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;

    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs
                    {
                        progressNormilized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
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
        if (HasKitchenObject())
        {
            KitchenObjectSO currentKitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();

            if (HasRecipeWithInput(currentKitchenObjectSO))
            {
                cuttingProgress++;

                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(currentKitchenObjectSO);

                OnCuttingProgressChanged?.Invoke(this, new OnCuttingProgressChangedEventArgs
                {
                    progressNormilized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });


                if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                {
                    KitchenObjectSO kitchenObjectSO = GetOutputForInput(currentKitchenObjectSO);
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
                }
            }
        }

    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipeSOs)
        {
            if (recipeSO.input == kitchenObjectSO)
            {
                return recipeSO;
            }
        }

        return null;
    }
}
