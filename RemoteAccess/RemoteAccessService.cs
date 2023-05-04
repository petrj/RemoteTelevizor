﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;
using LoggerService;
using Newtonsoft.Json;
using System.Threading;
using System.Data.Common;
using System.IO;

namespace RemoteAccess
{
    public class RemoteAccessService
    {
        private BackgroundWorker _worker;

        private ILoggingService _loggingService;

        private const int BufferSize = 1024;
        private string _ip;
        private int _port;
        private string _securityKey;

        public delegate void MessageReceivedHandler(object myObject, MessageReceivedArgs args);
        public event MessageReceivedHandler OnMessageReceived;

        public RemoteAccessService(ILoggingService loggingService)
        {
            _loggingService = loggingService;

            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += _worker_DoWork;
        }

        public bool ParamsChanged(string ip, int port, string securityKey)
        {
            if (ip != _ip ||
                port != _port ||
                securityKey != _securityKey)
            {
                return true;
            }

            return false;
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _loggingService.Info("[RAS]: Starting Remote Access Service background thread");

            try
            {
                // Data buffer for incoming data.
                var bytes = new Byte[BufferSize];

                IPAddress ipAddress;

                if (string.IsNullOrEmpty(_ip))
                {
                    var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    ipAddress = ipHostInfo.AddressList[0];
                }
                else
                {
                    ipAddress = IPAddress.Parse(_ip);
                }

                _loggingService.Info($"[RAS]: Endpoint: {_ip}:{_port}");

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);

                // Create a TCP/IP socket.

                using (var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    // Start listening for connections.
                    while (!_worker.CancellationPending)
                    {
                        // Program is suspended while waiting for an incoming connection.
                        using (var handler = listener.Accept())
                        {
                            string data = null;

                            var bytesRec = int.MaxValue;
                            while (bytesRec > 0)
                            {
                                bytesRec = handler.Receive(bytes);
                                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            }

                            _loggingService.Debug($"[RAS]: Received data: {data}");

                            //var responseMessage = new RemoteAccessMessage()
                            //{
                            //    command = "responseStatus",
                            //    commandArg1 = "OK"
                            //};

                            try
                            {
                                var decryptedData = CryptographyService.DecryptString(_securityKey, data);

                                var message = JsonConvert.DeserializeObject<RemoteAccessMessage>(decryptedData);

                                OnMessageReceived(this, new MessageReceivedArgs(message));
                            }
                            catch (Exception ex)
                            {
                                _loggingService.Info("[RAS]: unknown message");
                            }

                            //var response = JsonConvert.SerializeObject(responseMessage);
                            //var responseEncrypted = CryptographyService.EncryptString(_securityKey, response);

                            //handler.Send(Encoding.ASCII.GetBytes(responseEncrypted));
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, "[RAS]");
            }
        }

        public bool IsBusy
        {
            get
            {
                return _worker.IsBusy;
            }
        }

        public bool SetConnection(string ip, int port, string securityKey)
        {
            if (_worker.IsBusy)
                return false;

            _ip = ip;
            _port = port;
            _securityKey = securityKey;


            return true;
        }

        public void StartListening()
        {
            if (_worker.IsBusy)
                return;

            _worker.RunWorkerAsync();
        }

        public void StopListening()
        {
            if (!_worker.IsBusy)
                return;

            _worker.CancelAsync();
        }

        public bool SendMessage(RemoteAccessMessage message)
        {
            try
            {
                using (var socket = new System.Net.Sockets.TcpClient(_ip, _port))
                {
                    using (var stream = socket.GetStream())
                    {
                        using (var streamWriter = new StreamWriter(stream))
                        {
                            var messageJSON = JsonConvert.SerializeObject(message);
                            var messageEncrypted = CryptographyService.EncryptString(_securityKey, messageJSON);

                            streamWriter.Write(messageEncrypted);
                            streamWriter.Flush();
                        }
                    }
                    socket.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex);

                return false;
            }
        }
    }
}
