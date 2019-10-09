using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Leap;
using Leap.Unity.Attributes;
using Leap.Unity.Internal;

public class TrueMotionObjectVer : MonoBehaviour {

    public string serialPort;
    public float boardShift;

    public Leap.Unity.LeapServiceProvider deviceProvider;
    private Controller controller;
        
    private SerialPort storm;

    private string crc = "3334";
    private float pitchBuffer = 0, rollBuffer = 0, yawBuffer = 0;
    private int yawDriftCorrectionDelta = 680, yawDelta = 0, frameCount = 0;
    private float palmSpeed = 0f, shiftAmplitude = 100f;

    public bool enableSerial, invertAxis;
    private bool enabledSerial;
    private bool trackLoss;
    private bool borderCollision;

    private Vector3 newPos;

    // ================================================================================ MATH FUNCTIONS
    private static byte[] HexToByteArray(String hex) {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    private static string ByteArrayToString(byte[] ba) {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    private static string FloatToHex(float f) {
        StringBuilder sb = new StringBuilder();
        Byte[] ba = BitConverter.GetBytes(f);
        foreach (Byte b in ba)
            for (int i = 0; i < 8; i++) {
                sb.Insert(0, ((b >> i) & 1) == 1 ? "1" : "0");
            }

        string r = sb.ToString();
        byte[] bytes = new byte[4];
        for (int i = 0; i < 4; i++) {
            bytes[i] = Convert.ToByte(r.Substring(i*8, 8), 2);
        }

        return ByteArrayToString(bytes);
    }

	// ================================================================================ START
	void Start () {
        enableSerial = true; invertAxis = true; enabledSerial = true;

        Application.runInBackground = true;

        try
        {
            storm = new SerialPort(serialPort, 115200);
            if (!storm.IsOpen) storm.Open();

            controller = deviceProvider.GetLeapController();
            controller.FrameReady += onFrame;

            borderCollision = false;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
	}
	
	// ================================================================================ UPDATE
	void Update () {
        if(!enableSerial && enabledSerial) {
            resetPosition();
            enabledSerial = false;
        }
        if (enableSerial && !enabledSerial) enabledSerial = true;

        if (Input.GetKeyDown(KeyCode.Space) && enabledSerial && storm.IsOpen) resetPosition();
	}

    // ================================================================================ ONQUIT
    void OnApplicationQuit() {
        if (storm.IsOpen) storm.Close();
    }

    // ================================================================================ GETTERS
    public float getPitch() {
        return pitchBuffer;
    }

    public float getRoll() {
        return rollBuffer;
    }

    public float getYaw() {
        return yawBuffer;
    }

    public float getSpeed() {
        return palmSpeed;
    }

    public Vector3 getPos() {
        return newPos;
    }

    // ================================================================================ LEAP ONFRAME
    private void onFrame(object sender, FrameEventArgs args) {
        trackLoss = false;
        Frame currentFrame = args.frame;

        //MANUAL YAW CORRECTION
        if(frameCount == yawDriftCorrectionDelta) {
            frameCount = 0;
            yawBuffer++;
            yawDelta++;
        } else {
            frameCount++;
        }

        if (currentFrame.Hands.Count != 0) {
            Hand hand = currentFrame.Hands[0];
            Vector pos = hand.PalmPosition;
            int angleThreshold = 8; float angularSpeed = 1; int hThreshold = 50; int vThreshold = 30;

            //SPEED THRESHOLD
            palmSpeed = hand.PalmVelocity.Magnitude;
            if(palmSpeed > 1000f) {
                pos += hand.PalmVelocity.Normalized * shiftAmplitude;
                //pos += hand.PalmVelocity * 0.3f;
                //pos += hand.PalmVelocity.Normalized * (float) Math.Sqrt(0.8 * Math.Abs(palmSpeed));
                angularSpeed = 5.0f;
            }

            //POSITION CALCULATION
            float vAngle = 0;
            float hAngle = 0;

            if (invertAxis) {
                vAngle = Convert.ToSingle((Math.Atan(pos.z / pos.y)) * (180 / Math.PI));
                hAngle = Convert.ToSingle((Math.Atan(pos.x / pos.y)) * (180 / Math.PI));
            } else {
                vAngle = Convert.ToSingle((Math.Atan(-pos.z / pos.y)) * (180 / Math.PI));
                //hAngle = Convert.ToSingle((Math.Atan(-pos.x / pos.y)) * (180 / Math.PI));
            }

            

            if (vAngle > -angleThreshold && vAngle < angleThreshold) vAngle = 0;
            if (hAngle > -angleThreshold && hAngle < angleThreshold) hAngle = 0;

            if (vAngle < 0) vAngle = -angularSpeed;
            else if (vAngle > 0) vAngle = angularSpeed;
            if (hAngle < 0) hAngle = -angularSpeed;
            else if (hAngle > 0) hAngle = angularSpeed;

            pitchBuffer = Convert.ToSingle(pitchBuffer + vAngle); //DELTA
            yawBuffer = Convert.ToSingle(yawBuffer + hAngle); //THETA

            if (pitchBuffer < -vThreshold) pitchBuffer = -vThreshold;
            else if (pitchBuffer > vThreshold) pitchBuffer = vThreshold;
            if (yawBuffer < -hThreshold) yawBuffer = -hThreshold;
            else if (yawBuffer > hThreshold) yawBuffer = hThreshold;


            //SPHERICAL POS CALCULATION
            newPos.z = (float)(Math.Round((pos.y/1000) * Math.Cos(-pitchBuffer * Math.PI / 180) * Math.Cos(yawBuffer * Math.PI / 180), 3));
            newPos.x = (float)(Math.Round(-(pos.y/1000) * Math.Cos(-pitchBuffer * Math.PI / 180) * Math.Sin(yawBuffer * Math.PI / 180), 3));
            newPos.y = (float)(Math.Round((pos.y/1000) * Math.Sin(-pitchBuffer * Math.PI / 180), 3));
        
            //COMMAND TO GIMBALL
            string pitchCommand = FloatToHex(pitchBuffer).Substring(6, 2) + FloatToHex(pitchBuffer).Substring(4, 2) + FloatToHex(pitchBuffer).Substring(2, 2) + FloatToHex(pitchBuffer).Substring(0, 2);
            string rollCommand = "00000000";
            string yawCommand = FloatToHex(yawBuffer).Substring(6, 2) + FloatToHex(yawBuffer).Substring(4, 2) + FloatToHex(yawBuffer).Substring(2, 2) + FloatToHex(yawBuffer).Substring(0, 2);
            string flagstype = "0000";

            byte[] command = HexToByteArray("FA0E11" + pitchCommand + rollCommand + yawCommand + flagstype + crc);

            if (enableSerial && storm.IsOpen) {
                storm.Write(command, 0, command.Length);
            }
        } else {
            string pitchCommand = FloatToHex(pitchBuffer).Substring(6, 2) + FloatToHex(pitchBuffer).Substring(4, 2) + FloatToHex(pitchBuffer).Substring(2, 2) + FloatToHex(pitchBuffer).Substring(0, 2);
            string rollCommand = "00000000";
            string yawCommand = FloatToHex(yawBuffer).Substring(6, 2) + FloatToHex(yawBuffer).Substring(4, 2) + FloatToHex(yawBuffer).Substring(2, 2) + FloatToHex(yawBuffer).Substring(0, 2);
            string flagstype = "0000";

            byte[] command = HexToByteArray("FA0E11" + pitchCommand + rollCommand + yawCommand + flagstype + crc);

            if (enableSerial && storm.IsOpen) {
                storm.Write(command, 0, command.Length);
            }

            if(!trackLoss) {
                trackLoss = true;
                resetPosition();
            }
        }
    }

    public void resetPosition() {
            byte[] resetCommand = HexToByteArray("FA0612" + "000000000000" + crc);

            pitchBuffer = 0; rollBuffer = 0; yawBuffer = 0;

            storm.Write(resetCommand, 0, resetCommand.Length);
    }

    void FixedUpdate () {
        Vector3 pos;
        if(boardShift > 0) pos = new Vector3(getPos().x * 4.5f, 1.8415f, getPos().z * 5f + boardShift);
        else pos = new Vector3(-getPos().x * 4f, 1.8415f, -getPos().z * 5f + boardShift);

        if(pos.x < -0.75)  pos.x = -0.75f;
        if(pos.x > 0.75)  pos.x = 0.75f;
        if(pos.z < -2.3)  pos.z = -2.3f;

        if(!borderCollision) {
            gameObject.GetComponent<Rigidbody>().MovePosition(pos * 0.55f);
        }
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "AirHockeyTable") {
            borderCollision = true;
        }
    }

    void OnCollisionExit(Collision c) {
        borderCollision = false;
    }
}
