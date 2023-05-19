﻿using LoggerService;
using RemoteAccess;
using RemoteTelevizor.Models;
using RemoteTelevizor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RemoteTelevizor
{
    public partial class NavPage : ContentPage
    {
        private ILoggingService _loggingService;
        private RemoteDeviceViewModel _viewModel;
        private IAppData _appData;
        private Size _lastAllocatedSize = new Size(-1, -1);

        public NavPage(ILoggingService loggingService, IAppData appData)
        {
            InitializeComponent();

            _loggingService = loggingService;
            _appData = appData;

            BindingContext = _viewModel = new RemoteDeviceViewModel(loggingService);
        }

        public RemoteDeviceConnection Connection
        {
            get
            {
                return _viewModel.Connection;
            }
            set
            {
                _viewModel.SetConnection(value);
            }
        }

        private async void OnButtonDown(object sender, EventArgs e)
        {
            await ButtonDownFrame.ScaleTo(2, 100);
            await ButtonDownFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadDown.ToString());
        }

        private async void OnButtonUp(object sender, EventArgs e)
        {
            await ButtonUpFrame.ScaleTo(2, 100);
            await ButtonUpFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadUp.ToString());
        }

        private async void OnButtonLeft(object sender, EventArgs e)
        {
            await ButtonLeftFrame.ScaleTo(2, 100);
            await ButtonLeftFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadLeft.ToString());
        }

        private async void OnButtonRight(object sender, EventArgs e)
        {
            await ButtonRightFrame.ScaleTo(2, 100);
            await ButtonRightFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.DpadRight.ToString());
        }

        private async void OnButtonOK(object sender, EventArgs e)
        {
            await ButtonOKFrame.ScaleTo(2, 100);
            await ButtonOKFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Enter.ToString());
        }

        private async void OnButtonEscape(object sender, EventArgs e)
        {
            await ButtonBackFrame.ScaleTo(2, 100);
            await ButtonBackFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Escape.ToString());
        }

        private async void OnButtonMenu(object sender, EventArgs e)
        {
            await ButtonMenuFrame.ScaleTo(2, 100);
            await ButtonMenuFrame.ScaleTo(1, 100);

            await _viewModel.SendKey(Android.Views.Keycode.Menu.ToString());
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _loggingService.Info($"OnSizeAllocated: {width}/{height}");

            base.OnSizeAllocated(width, height);

            if (_viewModel == null)
                return;

            if (_lastAllocatedSize.Width == width &&
                _lastAllocatedSize.Height == height)
            {
                // no size changed
                return;
            }

            _lastAllocatedSize.Width = width;
            _lastAllocatedSize.Height = height;

            RemoteDeviceViewModel.SetViewAbsoluteLayoutBySize(RemoteStackLayout, width, height);
        }

    }
}