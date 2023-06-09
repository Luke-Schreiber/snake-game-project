﻿using Windows.Gaming.Input;

namespace SnakeGame;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        graphicsView.Invalidate();
        // makes OnFrame the eventhandler for when the controller says it is time for the view to update
        Controller.UpdateFrame += OnFrame;
        Controller.FailedToConnect += ConnectionFailed;
        Controller.ConnectionLost += NetworkErrorHandler;
        Controller.ConnectionSuccess += ConnectionSucceeded;
    }

    void OnTapped(object sender, EventArgs args)
    {
        keyboardHack.Focus();
    }

    void OnTextChanged(object sender, TextChangedEventArgs args)
    {
        Entry entry = (Entry)sender;
        String text = entry.Text.ToLower();
        if (text == "w")
        {
            Controller.MoveUp();
        }
        else if (text == "a")
        {
            Controller.MoveLeft();
        }
        else if (text == "s")
        {
            Controller.MoveDown();
        }
        else if (text == "d")
        {
            Controller.MoveRight();
        }
        entry.Text = "";
    }




    /// <summary>
    /// Event handler for the connect button
    /// We will put the connection attempt interface here in the view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ConnectClick(object sender, EventArgs args)
    {
        if (serverText.Text == "")
        {
            DisplayAlert("Error", "Please enter a server address", "OK");
            return;
        }
        if (nameText.Text == "")
        {
            DisplayAlert("Error", "Please enter a name", "OK");
            return;
        }
        if (nameText.Text.Length > 16)
        {
            DisplayAlert("Error", "Name must be less than 16 characters", "OK");
            return;
        }

        Controller.Connect(serverText.Text, nameText.Text);
        keyboardHack.Focus();
    }

    /// <summary>
    /// Use this method as an event handler for when the controller has updated the world
    /// </summary>
    public void OnFrame()
    {
        Dispatcher.Dispatch( () => graphicsView.Invalidate() );
    }

    /// <summary>
    /// Event handler for when a successful connection is made, greys out the connect button so it cant be clicked while in a lobby
    /// </summary>
    public void ConnectionSucceeded()
    {
        Dispatcher.Dispatch(() => connectButton.IsEnabled = false);
    }

    public void ConnectionFailed()
    {
        Dispatcher.Dispatch(() => DisplayAlert("Error", "Failed to connect to server", "OK"));
        Dispatcher.Dispatch(() => connectButton.IsEnabled = true);
    }

    private void NetworkErrorHandler()
    {
        Dispatcher.Dispatch(() => connectButton.IsEnabled = true);
        Dispatcher.Dispatch(() => DisplayAlert("Error", "Disconnected from server", "OK"));
    }

    private void ControlsButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Controls",
                     "W:\t\t Move up\n" +
                     "A:\t\t Move left\n" +
                     "S:\t\t Move down\n" +
                     "D:\t\t Move right\n",
                     "OK");
    }

    private void AboutButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("About",
      "SnakeGame solution\nArtwork by Jolie Uk and Alex Smith\nGame design by Daniel Kopta and Travis Martin\n" +
      "Implementation by Simon Padgen and Luke Schreiber\n" +
        "CS 3500 Fall 2022, University of Utah", "OK");
    }

    private void ContentPage_Focused(object sender, FocusEventArgs e)
    {
        //if (!connectButton.IsEnabled)
            //keyboardHack.Focus();
    }
}