using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDPSender
{
    private const int ORDER_RANGE = 100;
    public const string HOST = "192.168.179.8";
    public const int PORT = 8899;

    // wifi bridge address
    private string host = HOST;
	private int port = PORT;
	private UdpClient client;

    // 
    private bool sendStatus = true;

    // constractor
    public UDPSender()
    {
        this.client = new UdpClient();   
    }

    // set address and connecting in constractor
    public UDPSender(string h, int p)
    {
        this.host = h;
        this.port = p;

        this.client = new UdpClient();
        this.client.Connect(host, port);
    }

    public void SetHostAddress(string h, int p)
    {
        this.host = h;
        this.port = p;
    }

    public void Connect(string h, int p)
    {
        this.client.Connect(h, p);
    }

    public void CloseClient()
    {
        client.Close();
    }

    public void SendCode(List<byte> code)
    {
        if (code.Count == 3)
        {
            client.Send(code.ToArray(), code.Count);
        }
    }

    public void SendCode(List<byte> code, MonoBehaviour obj)
    {
        if (code.Count == 3)
        {
            client.Send(code.ToArray(), code.Count);

        } else if (code.Count == 6) {

            if (!sendStatus) {
                return;
            }
            
            byte[] array1 = code.ToArray();
            byte[] array2 = new byte[array1.Length / 2];           
            Array.Copy(array1, array1.Length / 2, array2, 0, array1.Length / 2);

            client.Send(array1, array1.Length);
            sendStatus = false;
            obj.StartCoroutine(DelayMethod(0.1f, () =>
            {
                client.Send(array2, array2.Length);
                sendStatus = true;
            }));
        }
    }
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}