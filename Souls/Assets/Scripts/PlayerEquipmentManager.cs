using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;

        [Header("Equipment Model Changers")]
        HelmetModelChanger helmetModelChanger;
        TorsoModelChanger torsoModelChanger;
        HipModelChanger hipModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedHandModel;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();

        }

        private void EquipAllEquipmentModelsOnStart()
        {
            helmetModelChanger.UnEquipAllHelmetModels();

            if(playerInventory.currentHelmetEquipment != null)
            {
                //nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                //Equip Default Head
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);

                //Player's head model (can be active when there's an exposed helmet, for example a hood)
                //nakedHeadModel.SetActive(true);
            }
            
            torsoModelChanger.UnEquipAllTorsoModels();

            if(playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            }
            else
            {
                //Equip Default Torso (naked)
            }

            hipModelChanger.UnEquipAllHipModels();

            if(playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
            }
            else
            {
                //Equip Default
            }
            
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
