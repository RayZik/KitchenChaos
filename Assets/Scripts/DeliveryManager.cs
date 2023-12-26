using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField]
    private RecipeListSO recipeSOs;

    private List<RecipeSO> waitingRecipeSoList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int succesfullRecipesAmount;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSoList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeSOs.recipeSOs[UnityEngine.Random.Range(0, recipeSOs.recipeSOs.Count)];
                waitingRecipeSoList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSoList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSoList[i];

            if (waitingRecipeSO.kitchenObjectSOs.Count != plateKitchenObject.GetKitchenObjectSOs().Count)
            {
                continue;
            }

            bool plateContentsMatchesRecipe = true;


            foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOs)
            {
                bool ingredientFound = false;

                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOs())
                {
                    if (plateKitchenObjectSO == recipeKitchenObjectSO)
                    {
                        ingredientFound = true;
                        break;
                    }
                }

                if (!ingredientFound)
                {
                    plateContentsMatchesRecipe = false;
                    break;
                }
            }

            if (plateContentsMatchesRecipe)
            {
                succesfullRecipesAmount++;

                waitingRecipeSoList.RemoveAt(i);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }


        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSoList()
    {
        return waitingRecipeSoList;
    }

    public int GetSuccesfullRecipesAmount()
    {
        return succesfullRecipesAmount;
    }
}
