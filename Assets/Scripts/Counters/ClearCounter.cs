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
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
}
