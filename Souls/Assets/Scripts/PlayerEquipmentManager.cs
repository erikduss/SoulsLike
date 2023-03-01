using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        PlayerStats playerStats;

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
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();

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

            if(playerInventory.currentHelmetEquipment != null)
            {
                //nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
                playerStats.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                //Equip Default Head
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);

                //Player's head model (can be active when there's an exposed helmet, for example a hood)
                //nakedHeadModel.SetActive(true);
                playerStats.physicalDamageAbsorptionHead = 0;
            }
            
            //Torso Equipment
            torsoModelChanger.UnEquipAllTorsoModels();

            if(playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                playerStats.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                //Equip Default Torso (naked)
                playerStats.physicalDamageAbsorptionBody = 0;
            }

            //Leg Equipment
            hipModelChanger.UnEquipAllHipModels();
            //leftLegModelChanger.UnEquipAllLeftLegModels();
            //rightLegModelChanger.UnEquipAllRightLegModels();

            if(playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                //leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                //rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegName);
                playerStats.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
            }
            else
            {
                //Equip Default (naked)
                playerStats.physicalDamageAbsorptionLegs = 0;
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
