using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

// 프로토콜 : 규칙, 규약
// 정해진 규칙에 맞도록 제공하는 클래스 : Socket
// tcp소켓에서 중요한 함수 : Bind, Listen, Poll

/* TCP 클라이언트 동작 순서
 * 1. 소켓 생성
 * 2. 서버 연결
 */

public class TCPClient : MonoBehaviour
{
    Socket client;
    string sendMsg = string.Empty;
    string recvStr = string.Empty;
    string recvMsg = string.Empty;

    private void Update()
    {
        if(client != null && client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLen = client.Receive(buffer);
            if (recvLen > 0)
            {
                recvStr = System.Text.Encoding.UTF8.GetString(buffer);
                recvMsg = recvStr;
            }
        }
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 300, 300), "Connect"))
        {
            Connect("127.0.0.1", 80);
        }

        sendMsg = GUI.TextField(new Rect(0, 400, 500, 100), sendMsg);
        recvMsg = GUI.TextField(new Rect(600, 400, 500, 100), recvMsg);

        if (GUI.Button(new Rect(0, 600, 300, 300), "Send"))
        {
            if (!sendMsg.Equals(string.Empty))
            {
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(sendMsg); // Unicode -> UTF8 -> Byte
                client.Send(msg);
                sendMsg = string.Empty;
            }
        }
    }

    bool Connect(string ipaddress, int port)
    {
        try
        {
            // 소켓 생성
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 서버 연결
            // ※ EndPoint 클래스는 ip address와 port를 관리하는 클래스
            client.Connect(ipaddress, port);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            client = null;
        }

        return false;
    }

    private void OnApplicationQuit()
    {
        client.Shutdown(SocketShutdown.Both); // 소켓을 종료. 양쪽 다
        client.Close(); // 연결을 닫음
        client = null;
    }
}
