using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{

    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D UnknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
        // TODO Solve fight with serializefield and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        CameraRaycaster cameraRaycaster;

        // Use this for initialization
        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;

        }

        // Update is called once per frame
        void OnLayerChanged(int newLayer)
        {
            switch (newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;

                case enemyLayerNumber:
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(UnknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }
    }
}