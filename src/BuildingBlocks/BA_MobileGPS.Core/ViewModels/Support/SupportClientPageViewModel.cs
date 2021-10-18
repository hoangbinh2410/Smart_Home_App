﻿using BA_MobileGPS.Core.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportClientPageViewModel : ViewModelBase
    {
        #region Contructor
        public ICommand SupportErrorsSignalCommand { get; private set; }
        public ICommand SupportChangePlateCommand { get; private set; }
        public ICommand SupportErrorsCameraCommand { get; private set; }
        public SupportClientPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SupportChangePlateCommand = new DelegateCommand(PushSupportFeePage);

            SupportErrorsCameraCommand = new DelegateCommand(PushSupportDisconnectPage);
            SupportErrorsSignalCommand = new DelegateCommand(PushSupportErrorsSignalPage);
        }
        #endregion Contructor
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
            //SupportErrorsSignalCommand = new DelegateCommand(SupportErrorsSignalClicked);
            //SupportChangePlateCommand = new DelegateCommand(SupportChangePlateClicked);
            //SupportErrorsCameraCommand = new DelegateCommand(SupportErrorsCameraClicked);
        }

        #endregion Lifecycle
        //312312312312312
        #region Property

        #endregion Property
        #region  PrivateMethod
        public void PushSupportErrorsSignalPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportErrorsSignalPage");
            });
        }
        public void PushSupportFeePage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SupportFeePage");
            });
        }
        public void PushSupportDisconnectPage()
        {
            SafeExecute(async () =>
        {
                await NavigationService.NavigateAsync("SupportDisconnectPage");
            });
        }

        #endregion PrivateMethod
    }
}
