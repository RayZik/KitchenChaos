using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {

            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
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

                if (player.GetKitchenObject().TryGetPalate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    if (GetKitchenObject().TryGetPalate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
}
