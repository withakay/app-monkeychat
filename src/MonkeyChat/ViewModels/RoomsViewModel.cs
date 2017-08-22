﻿using System;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace MonkeyChat
{
    public class RoomsViewModel : BaseViewModel
    {
        IAblyMessenger ablyMessenger;

        public Command ConnectCommand { get; }

        Page page;

        public RoomsViewModel(Page page)
        {
            this.page = page;
            ablyMessenger = DependencyService.Get<IAblyMessenger>();

            ConnectCommand = new Command(async () =>
            {
                var success = false;
                string message = string.Empty;
                try
                {
                    IsBusy = true;
                    success = await ablyMessenger.InitializeAsync();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                finally
                {
                    IsBusy = false;
                }

                if (success)
                {
                    await page.DisplayAlert("Success", "Now joining #general.", "OK");
                    await page.Navigation.PushAsync(new MainChatPage());
                }
                else
                {
                    await page.DisplayAlert("Sad Monkeys", $"Unable to join #general: {message}" , "OK"); 
                }
            });
        }



    }
}

