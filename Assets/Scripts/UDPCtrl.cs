using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UDPCtrl : MonoBehaviour {

    // 
    private Rect rect = new Rect(0, 0, Screen.width, Screen.height);
    //private bool randomID = true;
	private bool isSet = false;

    // Layout Params
    private float BUTTON_HEIGHT = (Screen.height * 0.07f);

    // for Sender
    private UDPSender udp_send = null;
    private UDPReciever udp_rvc = null;

    private string sendhost = "192.168.179.8";
    private int sendport = 8899;

    //
    private int zoneID = 0;
    private static float wait_time = 1.0f;
    private static string waitT = "1.0";

    //
    private static int diffColorSlider = 0;
    private static int diffBrightnessSlider = 0;


    // lights
    private Dictionary<int, LightUnit> lights = new Dictionary<int, LightUnit>()
    {
        { 1, new LightUnit(1) },
        { 2, new LightUnit(2) },
        { 3, new LightUnit(3) },
        { 4, new LightUnit(4) },
        { 0, new LightUnit(0) },
    };

    private string[] message = new string[]{

    };

	private string show_sendms = "";

	// for Receiver
	private int localport = 22227;
    private static bool isStartSending = false;
	private static bool isStopSending = true;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && !isStartSending) {
            isStartSending = true;
            isStopSending = false;
        }

		if (isStartSending) {
			isStartSending = false;
			StartCoroutine(AutoSendMessage());
		}

	}

    void OnGUI()
    {
        // draw panels on gui layer
        rect = GUILayout.Window(1, rect, setMenu, "setMenu");
    }

	// end processing
	void OnApplicationQuit() {
		if (isSet) {
            ReleaseUdpClient();
		}
	}

    void SetUdpClient() {
        udp_send = new UDPSender(sendhost, sendport);
        udp_rvc = new UDPReciever(localport);
		udp_rvc.TStart ();
    }

	void ReleaseUdpClient (){
		udp_send.CloseClient ();
		udp_rvc.AppQuit ();
	}

    int value = 185;
	private IEnumerator AutoSendMessage(){
        Debug.Log("StartAutoSending");
        var value = 0;
        while (true){
            yield return new WaitForSeconds(wait_time);
			if(!isStopSending && isSet){
                doSlider("Color", value++);
            } else {
				break;
			}
            Debug.Log(value);
            if (value == 255) { value = 0; }
            if (value == 63) { value = 82; }
            if (value == 178) { value = 209; }
        }
	}

    private void setMenu(int windowID)
    {
		if (!isSet) {
	        //
			GUILayout.Label("----- Sendhost -----");
			GUILayout.BeginHorizontal ();
			GUILayout.Label("IPAddress");
			sendhost = GUILayout.TextField(UDPSender.HOST, 50);
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label("Port");
			sendport = int.Parse(GUILayout.TextField(UDPSender.PORT.ToString(), 10));
			GUILayout.EndHorizontal ();
			
			GUILayout.Label("----- Receivehost -----");
			GUILayout.BeginHorizontal ();
			GUILayout.Label("LocalPort");
			localport = int.Parse(GUILayout.TextField(localport.ToString(), 10));
			GUILayout.EndHorizontal ();

			if (GUILayout.Button ("Address Set", GUILayout.MinHeight(BUTTON_HEIGHT))) {
				SetUdpClient ();
				isSet = true;
			}
		} else {
			//
			GUILayout.Label("----- Sendhost -----");
			if (GUILayout.Button ("Disconnect", GUILayout.MinHeight(BUTTON_HEIGHT * 0.5f))) {
				ReleaseUdpClient ();
				isSet = false;
			}
            GUILayout.Label("SendMessage");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ALL_ON", GUILayout.MinHeight(BUTTON_HEIGHT))) {
                udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ALL_ON));
                show_sendms = "ALL_ON";
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ALL_OFF", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ALL_OFF));
                show_sendms = "ALL_OFF";
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ALL_WHITE", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ALL_CHANGE_WARM), this);
                show_sendms = "ALL_WHITE";
            }
            GUILayout.EndHorizontal();

            //
            GUILayout.Label ("----- ZONE -----");
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("1", GUILayout.MinHeight(BUTTON_HEIGHT))) {
				zoneID = 1;
				diffColorSlider = (int)(lights[1].Color());
                diffBrightnessSlider = (int)(lights[1].Brightness());
            }
			if (GUILayout.Button ("2", GUILayout.MinHeight(BUTTON_HEIGHT))) {
				zoneID = 2;
                diffColorSlider = (int)(lights[2].Color());
                diffBrightnessSlider = (int)(lights[2].Brightness());
            }
            if (GUILayout.Button("3", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                zoneID = 3;
                diffColorSlider = (int)(lights[3].Color());
                diffBrightnessSlider = (int)(lights[3].Brightness());
            }
            if (GUILayout.Button("4", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                zoneID = 4;
                diffColorSlider = (int)(lights[4].Color());
                diffBrightnessSlider = (int)(lights[4].Brightness());
            }
            if (GUILayout.Button ("All", GUILayout.MinHeight(BUTTON_HEIGHT))) {
                zoneID = 0;
                diffColorSlider = (int)(lights[0].Color());
                diffBrightnessSlider = (int)(lights[0].Brightness());
            }
			GUILayout.EndHorizontal ();

            //
            GUILayout.Label("----- Switch -----");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ON", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                switch(zoneID)
                {
                    case 1:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_1_ON));
                        lights[1].State(true);
                        show_sendms = "ZONE_1_ON";
                        break;
                    case 2:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_2_ON));
                        show_sendms = "ZONE_2_ON";
                        lights[2].State(true);
                        break;
                    case 3:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_3_ON));
                        show_sendms = "ZONE_3_ON";
                        lights[3].State(true);
                        break;
                    case 4:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_4_ON));
                        show_sendms = "ZONE_4_ON";
                        lights[4].State(true);
                        break;
                    case 0:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ALL_ON));
                        show_sendms = "ALL_ON";
                        foreach(var light in lights) {
                            lights[light.Key].State(true);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (GUILayout.Button("OFF", GUILayout.MinHeight(BUTTON_HEIGHT)))
            {
                switch (zoneID)
                {
                    case 1:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_1_OFF));
                        show_sendms = "ZONE_1_OFF";
                        lights[1].State(false);
                        break;
                    case 2:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_2_OFF));
                        show_sendms = "ZONE_2_OFF";
                        lights[2].State(false);
                        break;
                    case 3:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_3_OFF));
                        show_sendms = "ZONE_3_OFF";
                        lights[3].State(false);
                        break;
                    case 4:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ZONE_4_OFF));
                        show_sendms = "ZONE_4_OFF";
                        lights[4].State(false);
                        break;
                    case 0:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.ALL_OFF));
                        show_sendms = "ALL_OFF";
                        foreach (var light in lights) {
                            lights[light.Key].State(false);
                        }
                        break;
                    default:
                        break;
                }
            }
            GUILayout.EndHorizontal();

            //
            GUILayout.Label("----- Color -----");
            GUILayout.BeginHorizontal();
            var color = GUILayout.HorizontalSlider(lights[zoneID].Color(), 0.0f, 255.0f, GUILayout.MinHeight(BUTTON_HEIGHT));
            lights[zoneID].Color(color);
            doSlider("Color", (int)color);
            GUILayout.EndHorizontal();

            //
            GUILayout.Label("----- Brightness -----");
            GUILayout.BeginHorizontal();
            var brightness = GUILayout.HorizontalSlider(lights[zoneID].Brightness(), 0.0f, 26.0f, GUILayout.MinHeight(BUTTON_HEIGHT));
            lights[zoneID].Brightness(brightness);
            doSlider("Brightness", (int)brightness);
            GUILayout.EndHorizontal();

            //
            GUILayout.Label ("----- AutoSending -----");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Send Interval");
            waitT = GUILayout.TextField(waitT, 10);
            GUILayout.Label("(s)");
            GUILayout.EndHorizontal();
            if (waitT != "") { wait_time = float.Parse(waitT); }

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Auto=ON", GUILayout.MinHeight(BUTTON_HEIGHT))) {
				isStartSending = true;
				isStopSending = false;
			}
			if (GUILayout.Button ("Auto=OFF", GUILayout.MinHeight(BUTTON_HEIGHT))) {
				isStartSending = false;
				isStopSending = true;
			}
			GUILayout.EndHorizontal ();

			//
			GUILayout.Label ("----- SendMessage -----");
			GUILayout.Label (show_sendms);
		}
        
        //GUI.DragWindow();
    }

    public static void MessageReceived(bool status) {
        if (status) {
			isStartSending = true;
			isStopSending = false;
        }else {
			isStartSending = false;
			isStopSending = true;
        }
    }

    public static void ReturnReceived() {
		MessageReceived (false);
		isStartSending = false;
    }

    private bool isSliderChanged(string type, int value)
    {
        switch(type)
        {
            case "Color":
                if (diffColorSlider != value) {
                    diffColorSlider = value;
                    return true;
                } else {
                    return false;
                }

            case "Brightness":
                if (diffBrightnessSlider != value) {
                    diffBrightnessSlider = value;
                    return true;
                } else {
                    return false;
                }

            default:
                break;
        }
        return false;
    }

    private void doSlider(string type, int value)
    {
        if (!isSliderChanged(type, value) || 255 < value || (63 < value && value < 81) || (178 < value && value < 208)) {
            return;
        }

        Debug.Log(OrderConstants.GETCODE_ZONE_1_COLOR(Convert.ToString(value, 16)));

        switch(type)
        {
            case "Color":
                switch(zoneID) {
                    case 1:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_1_COLOR(Convert.ToString(value, 16))), this);
                        lights[1].Color(value);
                        break;
                    case 2:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_2_COLOR(Convert.ToString(value, 16))), this);
                        lights[2].Color(value);
                        break;
                    case 3:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_3_COLOR(Convert.ToString(value, 16))), this);
                        lights[3].Color(value);
                        break;
                    case 4:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_4_COLOR(Convert.ToString(value, 16))), this);
                        lights[4].Color(value);
                        break;
                    case 0:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ALL_COLOR(Convert.ToString(value, 16))), this);
                        foreach (var light in lights) {
                            lights[light.Key].Color(value);
                        }
                        break;
                    default:
                        break;
                }
                break;

            case "Brightness":
                switch (zoneID) {
                    case 1:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_1_BRIGHTNESS(Convert.ToString(value, 16))), this);
                        lights[1].Brightness(value);
                        break;
                    case 2:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_2_BRIGHTNESS(Convert.ToString(value, 16))), this);
                        lights[2].Brightness(value);
                        break;
                    case 3:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_3_BRIGHTNESS(Convert.ToString(value, 16))), this);
                        lights[3].Brightness(value);
                        break;
                    case 4:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ZONE_4_BRIGHTNESS(Convert.ToString(value, 16))), this);
                        lights[4].Brightness(value);
                        break;
                    case 0:
                        udp_send.SendCode(OrderConstants.hexCode2byteArray(OrderConstants.GETCODE_ALL_BRIGHTNESS(Convert.ToString(value, 16))), this);
                        foreach (var light in lights) {
                            lights[light.Key].Brightness(value);
                        }
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;

        }
    }
}
