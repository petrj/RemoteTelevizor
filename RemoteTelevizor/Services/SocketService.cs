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
        private RemoteDeviceConnection _connection;

        public SocketService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void SetConnection(RemoteDeviceConnection connection)
        {
            _connection = connection;
        }

        public void SendMessage(string message)
        {
            try
            {
                using (var socket = new System.Net.Sockets.TcpClient(_connection.IP, _connection.Port))
                {
                    using (var stream = socket.GetStream())
                    {
                        using (var streamWriter = new StreamWriter(stream))
                        {
                            streamWriter.WriteLine(message);
                            streamWriter.Flush();
                        }
                    }
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
