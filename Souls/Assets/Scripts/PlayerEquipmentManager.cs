using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsmanager;

        [Header("Equipment Model Changers")]
        HelmetModelChanger helmetModelChanger;
        TorsoModelChanger torsoModelChanger;
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedHandModel;
        public string nakedLeftLegModel;
        public string nakedRightLegModel;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsmanager = GetComponent<PlayerStatsManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();

        }

        private void EquipAllEquipmentModelsOnStart()
        {
            //Helmet Equipment
            helmetModelChanger.UnEquipAllHelmetModels();

            if(playerInventoryManager.currentHelmetEquipment != null)
            {
                //nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsmanager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                //Equip Default Head
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);

                //Player's head model (can be active when there's an exposed helmet, for example a hood)
                //nakedHeadModel.SetActive(true);
                playerStatsmanager.physicalDamageAbsorptionHead = 0;
            }
            
            //Torso Equipment
            torsoModelChanger.UnEquipAllTorsoModels();

            if(playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
                playerStatsmanager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                //Equip Default Torso (naked)
                playerStatsmanager.physicalDamageAbsorptionBody = 0;
            }

            //Leg Equipment
            hipModelChanger.UnEquipAllHipModels();
            //leftLegModelChanger.UnEquipAllLeftLegModels();
            //rightLegModelChanger.UnEquipAllRightLegModels();

            if(playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                //leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                //rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegName);
                playerStatsmanager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                //Equip Default (naked)
                playerStatsmanager.physicalDamageAbsorptionLegs = 0;
            }
            
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
