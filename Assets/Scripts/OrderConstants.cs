using System;
using System.Collections.Generic;

public static class OrderConstants
{
    const int NORMAL_LENGTH = 8;
    const int TWOSTEP_LENGTH = 17;

    public const string ALL_OFF            = "41 00 55";
    public const string ALL_ON             = "42 00 55";
    public const string SPEED_DOWN         = "43 00 55";
    public const string SPEED_UP           = "44 00 55";
    public const string ZONE_1_ON          = "45 00 55";
    public const string ZONE_1_OFF         = "46 00 55";
    public const string ZONE_2_ON          = "47 00 55";
    public const string ZONE_2_OFF         = "48 00 55";
    public const string ZONE_3_ON          = "49 00 55";
    public const string ZONE_3_OFF         = "4A 00 55";
    public const string ZONE_4_ON          = "4B 00 55";
    public const string ZONE_4_OFF         = "4C 00 55";
    public const string PRESET_INC         = "4D 00 55";
    public const string ALL_CHANGE_WARM    = "42 00 55 C2 00 55";
    public const string ZONE_1_CHANGE_WARM = "45 00 55 C5 00 55";
    public const string ZONE_2_CHANGE_WARM = "47 00 55 C7 00 55";
    public const string ZONE_3_CHANGE_WARM = "49 00 55 C9 00 55";
    public const string ZONE_4_CHANGE_WARM = "4B 00 55 CB 00 55";

    public static string GETCODE_ALL_BRIGHTNESS(string value)
    {
        return value.Length == 2 ? "42 00 55 4E " + value + " 55" : "42 00 55 4E 02 55";   
    }
    public static string GETCODE_ZONE_1_BRIGHTNESS(string value)
    {
        return value.Length == 2 ? "45 00 55 4E " + value + " 55" : "45 00 55 4E 02 55";
    }
    public static string GETCODE_ZONE_2_BRIGHTNESS(string value)
    {
        return value.Length == 2 ? "47 00 55 4E " + value + " 55" : "47 00 55 4E 02 55";
    }
    public static string GETCODE_ZONE_3_BRIGHTNESS(string value)
    {
        return value.Length == 2 ? "49 00 55 4E " + value + " 55" : "49 00 55 4E 02 55";
    }
    public static string GETCODE_ZONE_4_BRIGHTNESS(string value)
    {
        return value.Length == 2 ? "4B 00 55 4E " + value + " 55" : "4B 00 55 4E 02 55";
    }
    public static string GETCODE_ALL_COLOR(string value)
    {
        return value.Length == 2 ? "42 00 55 40 " + value + " 55" : "42 00 55 40 02 55";
    }
    public static string GETCODE_ZONE_1_COLOR(string value)
    {
        return value.Length == 2 ? "45 00 55 40 " + value + " 55" : "45 00 55 40 02 55";
    }
    public static string GETCODE_ZONE_2_COLOR(string value)
    {
        return value.Length == 2 ? "47 00 55 40 " + value + " 55" : "47 00 55 40 02 55";
    }
    public static string GETCODE_ZONE_3_COLOR(string value)
    {
        return value.Length == 2 ? "49 00 55 40 " + value + " 55" : "49 00 55 40 02 55";
    }
    public static string GETCODE_ZONE_4_COLOR(string value)
    {
        return value.Length == 2 ? "4B 00 55 40 " + value + " 55" : "4B 00 55 40 02 55";
    }

    public static List<byte> hexCode2byteArray(string code)
    {
        var byteList = new List<byte>();

        string[] hexValuesSplit = code.Split(' ');
        foreach (String hex in hexValuesSplit)
        {
            int value = Convert.ToInt32(hex, 16);
            byteList.Add(Convert.ToByte(value));
        }

        return byteList;
    }
}

//41 00 55 - All Off
//42 00 55 - All On
//43 00 55 - Speed Down(One Step Slower Disco)
//44 00 55 - Speed Up(One Step Faster Disco)
//45 00 55 - Zone 1 On
//46 00 55 - Zone 1 Off
//47 00 55 - Zone 2 On
//48 00 55 - Zone 2 Off
//49 00 55 - Zone 3 On
//4A 00 55 - Zone 3 Off
//4B 00 55 - Zone 4 On
//4C 00 55 - Zone 4 Off
//4D 00 55 - One Step Disco Mode Up(20 Disco Modes)
//42 00 55 wait 100ms then send C2 00 55 - All Zones Change back to Warm White.
//45 00 55 wait 100ms then send C5 00 55 - Zone 1 Change back to Warm White.
//47 00 55 wait 100ms then send C7 00 55 - Zone 2 Change back to Warm White.
//49 00 55 wait 100ms then send C9 00 55 - Zone 3 Change back to Warm White.
//4B 00 55 wait 100ms then send CB 00 55 - Zone 4 Change back to Warm White.
//42 00 55 wait 100ms then send 4E XX 55 - Set All to Brightness XX(XX range is 0x02 to 0x1B)
//45 00 55 wait 100ms then send 4E XX 55 - Set Zone 1 to Brightness XX(XX range is 0x02 to 0x1B)
//47 00 55 wait 100ms then send 4E XX 55 - Set Zone 2 to Brightness XX(XX range is 0x02 to 0x1B)
//49 00 55 wait 100ms then send 4E XX 55 - Set Zone 3 to Brightness XX(XX range is 0x02 to 0x1B)
//4B 00 55 wait 100ms then send 4E XX 55 - Set Zone 4 to Brightness XX(XX range is 0x02 to 0x1B)
//42 00 55 wait 100ms then send 40 XX 55 - Set All to Color XX(XX range is 0x00 to 0xFF)
//45 00 55 wait 100ms then send 40 XX 55 - Set Zone 1 to Color XX(XX range is 0x00 to 0xFF)
//47 00 55 wait 100ms then send 40 XX 55 - Set Zone 2 to Color XX(XX range is 0x00 to 0xFF)
//49 00 55 wait 100ms then send 40 XX 55 - Set Zone 3 to Color XX(XX range is 0x00 to 0xFF)
//4B 00 55 wait 100ms then send 40 XX 55 - Set Zone 4 to Color XX(XX range is 0x00 to 0xFF)