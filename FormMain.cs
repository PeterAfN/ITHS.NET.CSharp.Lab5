using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITHS.NET.Peter.Palosaari.Lab5
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void textBoxURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter) return;
            ButtonExtract_Click(sender, EventArgs.Empty);
        }

        private async void ButtonExtract_Click(object sender, EventArgs e)
        {
            Task<string[]> scrapeTask = ImageScraperAsync();
            buttonExtract.Enabled = false;
            textBoxImageLinks.Text = "Downloading image links...";
            await scrapeTask;
            buttonExtract.Enabled = true;
            textBoxImageLinks.Text = string.Join(Environment.NewLine, scrapeTask.Result);
        }

        private async Task<string[]> ImageScraperAsync()
        {
            string urlTXTBox = textBoxURL.Text;
            if (!urlTXTBox.StartsWith("http://")) urlTXTBox = "http://" + urlTXTBox;
            if (!IsValidUrl(urlTXTBox)) return null;

            var client = new HttpClient();
            Task<string> downloadHtml = client.GetStringAsync("http://gp.se");
            Task delay = Task.Delay(TimeSpan.FromSeconds(5));
            labelImagesFound.Text = $"waiting";
            await delay;
            var rgx = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            await downloadHtml;
            var urlMatches = rgx.Matches(downloadHtml.Result);

            var url = new string[urlMatches.Count];
            if (urlMatches.Count == 0) return new [] {"No images could be downloaded."};
            for (var i = 0; i < urlMatches.Count; i++)
            {
                string combUrl = urlTXTBox + urlMatches[i].Groups[1].Value;
                if (IsValidUrl(combUrl)) url[i] = combUrl;
            }

            labelImagesFound.Text = $"Images found: {textBoxImageLinks.Lines.Length}";
            return url;
        }

        private static bool IsValidUrl(string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out var uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        private void ButtonSaveImages_Click(object sender, EventArgs e)
        {
            //            //await when  any

            //downloadHTML.Wait();

            //spara regex matchen i en string[]. Gör en task för varje url i string[]
            //och göra en client.GetByteArrayAsync -> lista med tasks  //await when  any -> async save


            //vid sparning av bilder använd  client.GetByteArrayAsync

            // folderbrowserdialog

            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //dialog.SelectedPath
                }
            }
        }


    }
}
