using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;

namespace RPG.Characters
{

    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] int enemyLayer = 9;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimeBetweenHits = 0.5f;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] AnimatorOverrideController animOverrideController;

        GameObject currentTarget;
        public float currentHealthPoints;
        float lastHitTime;

        CameraRaycaster cameraRaycaster;


        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / (float)maxHealthPoints;
            }
        }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            OverrideAnimatorController();
        }

        void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        void OverrideAnimatorController()
        {
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animOverrideController;
            animOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();
        }

        void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No dominantHand found, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominantHand Scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        // TODO Refactor to reduce lines
        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;


                // Check if enemy is in range
                if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
                {
                    return;
                }

                currentTarget = enemy;

                var enemyComponent = enemy.GetComponent<Enemy>();
                if (Time.time - lastHitTime > minTimeBetweenHits)
                {
                    enemyComponent.TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }

            }
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}