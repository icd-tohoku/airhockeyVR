using System.Collections;
using UnityEngine;
using Leap;
using Leap.Unity.Attributes;

namespace AirHockey.UI
{
    public class OptionMenuObjectVer : MonoBehaviour
    {
        public GUISkin guiSkin;

        private string clicked = "";
        private float speed = 1.0f;
        private bool menuOpen = false;
        private Rect WindowRect = new Rect((Screen.width / 2) - 100, Screen.height / 2, 200, 200);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !menuOpen)
                menuOpen = true;
            else if (Input.GetKeyDown(KeyCode.Escape) && menuOpen)
                menuOpen = false;
        }

        private void OnGUI()
        {
            GUI.skin = guiSkin;

            //GUI.Box(new Rect (0, 10, Screen.width, 25), TrueMotionObjectVer.messageStatus);
            //GUI.Label(new Rect(10, 40, 500, 20), "FPS: " + TrueMotionObjectVer.leapFPS);
            //GUI.Label(new Rect(10, 55, 500, 20), "Pitch - DELTA - Latitude : " + TrueMotionObjectVer.getPitch());
            //GUI.Label(new Rect(10, 70, 500, 20), "Yaw - THETA - Longitude : " + TrueMotionObjectVer.getYaw());
            //GUI.Label(new Rect(10, 85, 500, 20), "Palm Speed : " + TrueMotionObjectVer.getSpeed());

            if (clicked == "" && menuOpen)
            {
                WindowRect = GUI.Window(0, WindowRect, menuFunc, "TrueMotion Menu");
            }
            else if (clicked == "options" && menuOpen)
            {
                WindowRect = GUI.Window(1, WindowRect, optionsFunc, "Options");
            }
        }

        private void optionsFunc(int id)
        {
            GUILayout.Box("Speed");
            speed = GUILayout.HorizontalSlider(speed, 0.0f, 1.0f);

            //TrueMotionObjectVer.enableSerial = GUILayout.Toggle(TrueMotionObjectVer.enableSerial, "Enable TrueMotion");
            //TrueMotionObjectVer.invertAxis = GUILayout.Toggle(TrueMotionObjectVer.invertAxis, "Invert TrueMotion Coordinates");

            if (GUILayout.Button("Reset TrueMotion position"))
            {
                //TrueMotionObjectVer.resetPosition();
            }

            if (GUILayout.Button("Back"))
            {
                clicked = "";
            }
        }

        private void menuFunc(int id)
        {
            if (GUILayout.Button("Resume"))
            {
                menuOpen = false;
            }

            if (GUILayout.Button("Options"))
            {
                clicked = "options";
            }

            if (GUILayout.Button("Quit"))
            {
                Application.Quit();
            }
        }
    }
}