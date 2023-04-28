using Android.Locations;
using Java.IO;
using LoggerService;
using RemoteTelevizor.Models;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace RemoteTelevizor.Services
{
    public class SocketService
    {
        private ILoggingService _loggingService;
        private SocketConnection _connection;

        public SocketService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void SetConnection(SocketConnection connection)
        {
            _connection = connection;
        }

        public void SendMessage(string message)
        {
            try
            {
                var localEndPoint = new IPEndPoint(_connection.IPAddress, _connection.Port);

                using (var socket = new Socket(_connection.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(localEndPoint);

                    var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);

                    var bytesSent = socket.Send(messageBytes);

                    socket.Close();
                }
            } catch (Exception ex)
            {
                MessagingCenter.Send($"Error: {ex.Message}", MainPageViewModel.MSG_ToastMessage);
                _loggingService.Error(ex);
            }
        }
    }
}
