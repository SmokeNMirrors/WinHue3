﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Org.BouncyCastle.Crypto.Tls;

namespace WinHue3.Philips_Hue.BridgeObject.Entertainment_API
{
    public class HueDatagramTransport : DatagramTransport
    {

        private readonly Socket socket;

        public HueDatagramTransport(Socket socket)
        {
            this.socket = socket;
        }

        public int GetReceiveLimit()
        {
            return 4096;
        }

        public int GetSendLimit()
        {
            return 4096;
        }

        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {

            return socket.Receive(buf, off, len, SocketFlags.None);
        }

        public void Send(byte[] buf, int off, int len)
        {

            socket.Send(buf, off, len, SocketFlags.None);
                
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
