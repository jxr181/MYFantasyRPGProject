using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.CameraUI;

namespace RPG.Characters
{

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]

    public class PlayerMovement : MonoBehaviour
    {
        // TODO Solve fight with serializefield and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        AICharacterControl aiCharacterControl = null;
        CameraRaycaster cameraRaycaster = null;
        GameObject walkTarget = null;
        //Vector3 currentClickTarget;


        //bool isInGamePadMode = false; 


        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");
            //currentClickTarget = transform.position;

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        }

        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case enemyLayerNumber:
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharacterControl.SetTarget(enemy.transform);
                    break;

                case walkableLayerNumber:
                    walkTarget.transform.position = raycastHit.point;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;

                default:
                    Debug.LogWarning("Don't know how to handle mouse click for player movement");
                    return;
            }
        }


















        //TODO Make this get called again
        void ProcessGamePadMovement()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }



        void OnDrawGizmos()
        {
            //print("Draw Gizmos");
        }
    }

}