using UnityEngine;
using UnityEngine.InputSystem;
using Farming;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))] // Input is required and we don't store a reference
    [RequireComponent(typeof(Farmer))]

    public class PlayerController : MonoBehaviour
    {
        //[SerializeField] private TileSelector tileSelector;
        //[SerializeField] private GameObject waterCan;
        //[SerializeField] private GameObject hoe;
        MovementController moveController;
        Farmer farmer;
        [SerializeField] private TileSelector tileSelector;
        AnimatedController animatedController;
        InventoryController inventoryController;

        void Start()
        {
            moveController = GetComponent<MovementController>();
            farmer = GetComponent<Farmer>();
            animatedController = GetComponent<AnimatedController>();
            inventoryController = GetComponent<InventoryController>();
            ////SetTool("None");

            //// TODO: Consider Debug.Assert vs RequireComponent(typeof(...))
            //Debug.Assert(animatedController, "PlayerController requires an animatedController");
            //Debug.Assert(moveController, "PlayerController requires a MovementController");
            //Debug.Assert(tileSelector, "PlayerController requires a TileSelector.");
        }
        public void OnMove(InputValue inputValue)
        {
            Vector2 inputVector = inputValue.Get<Vector2>();
            moveController.Move(inputVector);
        }

        public void OnJump(InputValue inputValue)
        {
            moveController.Jump();
        }

        public void OnInteract(InputValue value)
        {
            FarmTile tile = tileSelector.GetSelectedTile();
            farmer.TryTileInteraction(tile);
        }

        //public void SetTool(string tool)
        //{
        //    waterCan.SetActive(false);
        //    hoe.SetActive(false);

        //    switch (tool)
        //    {

        //        case "Watering Can":
        //            waterCan.SetActive(true);
        //            break;

        //        case "Hoe":
        //            hoe.SetActive(true);
        //            break;

        //        case "None":
        //            waterCan.SetActive(false);
        //            hoe.SetActive(false);   
        //            break;
        //    }
        //}
    }
}