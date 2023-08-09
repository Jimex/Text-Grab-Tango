using System;
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

    //alex20230808.sn
    public async static void SendTextToDictTango(string text, double x, double y)
    {
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"http://wordlookup.localhost:16332/?word={Uri.EscapeDataString(text)}&x={x}&y={y}");
        await httpClient.SendAsync(request);
    }
    //alex20230808.en
}
