using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Text_Grab.Properties;

namespace Text_Grab.Utilities;

public class OutputUtilities
{
    public static void HandleTextFromOcr(string grabbedText, bool isSingleLine, bool isTable, TextBox? destinationTextBox = null)
    {
        if (isSingleLine && !isTable)
            grabbedText = grabbedText.MakeStringSingleLine();

        if (destinationTextBox is not null)
        {
            // Do it this way instead of append text because it inserts the text at the cursor
            // Then puts the cursor at the end of the newly added text
            // AppendText() just adds the text to the end no matter what.
            destinationTextBox.SelectedText = grabbedText;
            destinationTextBox.Select(destinationTextBox.SelectionStart + grabbedText.Length, 0);
            destinationTextBox.Focus();
            return;
        }

        if (!Settings.Default.NeverAutoUseClipboard)
            try { Clipboard.SetDataObject(grabbedText, true); } catch { }

        if (Settings.Default.ShowToast)
            NotificationUtilities.ShowToast(grabbedText);

        WindowUtilities.ShouldShutDown();
    }
    static HttpClient? httpClient = null;
    //alex20230808.sn
    public async static void SendTextToDictTango(string text, double x, double y)
    {
        if (httpClient == null)
        {
            httpClient = new HttpClient();
        }
        var url = $"http://localhost:16332/wordlookup.dt?word={Uri.EscapeDataString(text)}&x={x}&y={y}";
        //var request = new HttpRequestMessage(HttpMethod.Get, url);
        await httpClient.GetAsync(url);
        /*
        if (resposne.StatusCode == HttpStatusCode.NotFound)
        {
            MessageBox.Show("Please open the DictTango app first");
        }
        */
    }

    public static void HidDictTangoFloatingWin()
    {
        SendTextToDictTango("", 0, 0);
    }
    //alex20230808.en
}
