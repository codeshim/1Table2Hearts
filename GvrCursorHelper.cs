using System;
using UnityEngine;

namespace Gvr.Internal
{
    public class GvrCursorHelper
    {
        private static bool cachedHeadEmulationActive;

        private static bool cachedControllerEmulationActive;

        public static bool HeadEmulationActive
        {
            set
            {
                cachedHeadEmulationActive = value;
                UpdateCursorLockState();
            }
        }

        public static bool ControllerEmulationActive
        {
            set
            {
                cachedControllerEmulationActive = value;
                UpdateCursorLockState();
            }
        }

        private static void UpdateCursorLockState()
        {
            bool active = cachedHeadEmulationActive || cachedControllerEmulationActive;
            Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}