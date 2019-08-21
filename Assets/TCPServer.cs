using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    Socket server;
    Socket client; // 클라이언트 소켓 복사본

    void Start()
    {
        CreateServer();
    }

    void Update()
    {
        // 소켓의 상태 확인
        // 대기 시간, 통신모드 (SelectRead: 읽어들일 데이터가 있는지)
        if(server.Poll(0, SelectMode.SelectRead))
        {
            client = server.Accept();
        }

        if (client != null && client.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLength = client.Receive(buffer); // 사이즈 값을 int 타입으로 리턴
            if (recvLength == 0) // 클라이언트가 종료된 경우
            {
                client = null;
                return;
            }
            else
            {
                client.Send(buffer);
            }

            Debug.Log(buffer);
        }
    }

    void CreateServer()
    {
        try
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, 80));
            server.Listen(1); // 접속 가능 대수를 int 타입으로 받음
        }
        catch (Exception ex)
        {

        }
    }

    private void OnApplicationQuit()
    {
        if(client != null)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client = null;
        }
        if (server != null)
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            server = null;
        }
    }
}
