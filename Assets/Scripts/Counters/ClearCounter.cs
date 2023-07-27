using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Nothing on Counter
            if(player.HasKitchenObject())
            {
                // Player has something to put on counter
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }

        }
        else
        {
            // Something on Counter
            PlateKitchenObject plateKitchenObject;
            BreadKitchenObject breadKitchenObject;
            if (player.HasKitchenObject())
            {
                // Player has something
                if(player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // Player has plate
                    if (GetKitchenObject().TryGetBread(out breadKitchenObject))
                    {
                        if (plateKitchenObject.TryAddBurger(breadKitchenObject))
                        {
                            GetKitchenObject().DestroySelf();
                            return;
                        }
                    }
                    else if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        return;
                    }
                }
                if (player.GetKitchenObject().TryGetBread(out breadKitchenObject))
                {
                    // Player has bread
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddBurger(breadKitchenObject))
                        {
                            player.GetKitchenObject().DestroySelf();
                            return;
                        }
                    }
                    else if (breadKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        return;
                    }
                }
                if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // Counter has plate
                    if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                        return;
                    }
                }
                if(GetKitchenObject().TryGetBread(out breadKitchenObject))
                {
                    // Counter has bread
                    if (breadKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                        return;
                    }
                }

            }
            else
            {
                // Player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

}
