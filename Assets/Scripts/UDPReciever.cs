using UnityEngine;
using System.Net.Sockets;
using System.Threading;

public class UDPReciever {

	//localIP and PortNum
	public string localIpString = "127.0.0.1";
	//public int localPort = 22228;
	private UdpClient udp = null;
	private Thread UDP_RCVthread = null;
    private string debug_message = "";

    public UDPReciever(int localPort)
	{
		System.Net.IPAddress localAddress =	System.Net.IPAddress.Parse(localIpString);

		//UdpClientを作成し、ローカルエンドポイントにバインドする
		System.Net.IPEndPoint localEP =	new System.Net.IPEndPoint(localAddress, localPort);
		udp = new System.Net.Sockets.UdpClient(localEP);

		UDP_RCVthread = new Thread(new ThreadStart(UDPRecieve));
        
	}

    public void TStart()
    {
		UDP_RCVthread.Start();
		Debug.Log("受信スレッドスタートしました");
    }

    public string getRcvStr() {
        return debug_message;
    }

	private void UDPRecieve()
	{
		try
		{
			while(true)
			{
				//データを受信する
				System.Net.IPEndPoint remoteEP = null;
				byte[] rcvBytes = udp.Receive(ref remoteEP);
				
				//データを文字列に変換する
				string rcvMsg = System.Text.Encoding.UTF8.GetString(rcvBytes);

                //"start"を受信したらmessage送信開始
				if (rcvMsg.Equals("start\r\n"))
                {
                    UDPCtrl.MessageReceived(true);
                } else
                //"stop"を受信したらmessage送信停止
				if (rcvMsg.Equals("stop\r\n"))
                {
                    UDPCtrl.MessageReceived(false);
                } else
				//"return"を受信したら終了
				if (rcvMsg.Equals("return\r\n"))
				{
					UDPCtrl.ReturnReceived();
				}

				//Debug.Log (rcvMsg);
                debug_message = rcvMsg;
			}
		}
		catch (System.Exception e ){
			Debug.Log(e.ToString());
		}
	} 
	
	public void AppQuit()
	{
		//UdpClientを閉じる
		udp.Close();

		//threadを閉じる
		UDP_RCVthread.Abort();

		Debug.Log("終了しました。");
	}
}
