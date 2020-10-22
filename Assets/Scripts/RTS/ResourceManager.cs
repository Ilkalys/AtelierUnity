using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public static class ResourceManager
    {
        public static float ScrollSpeed {  get { return 5; } }
        public static float MoveSpeed { get { return 10; } }

        // Largeur de la bordure de l'ecran où la camera bougera si la souris passe
        public static int ScroolWidth { get { return 25; } }

        public static float MinCameraHeight { get { return 3; } }

        public static float MaxCameraHeight { get { return 30; } }

        public static float ORDERS_BAR_WIDTH { get { return 150; } }
        public static float RESOURCE_BAR_HEIGHT { get { return 30; } }

        private static Vector3 invalidPosition = new Vector3(-9999, -9999, -9999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }

        private static int[] unitPositionPerRing = new int[] {1, 10, 25, 40, 60 };
        public static int[] UnitPositionPerRing { get { return unitPositionPerRing; } }
        public static int BuildSpeed { get { return 2; } }
        public static int MAXUNITINCHARGE { get { return 60; } }

        private static GameObjectList gameObjectList;
        public static void SetGameObjectList(GameObjectList objectList)
        {
            gameObjectList = objectList;
        }
        public static GameObject GetBuilding(string name)
        {
            return gameObjectList.GetBuilding(name);
        }

        public static GameObject GetUnit(string name)
        {
            return gameObjectList.GetUnit(name);
        }

        public static GameObject GetWorldObject(string name)
        {
            return gameObjectList.GetWorldObject(name);
        }

        public static GameObject GetPlayerObject()
        {
            return gameObjectList.GetPlayerObject();
        }

        public static Sprite GetBuildImage(string name)
        {
            return gameObjectList.GetBuildImage(name);
        }
    }
}
