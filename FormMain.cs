using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        private async void ButtonExtract_Click(object sender, EventArgs e)
        {
            if (!UrlVerified()) return;

            try
            {
                Task<List<string>> scrapeTask = ImageScraperAsync();                //start Task
                buttonExtract.Enabled = false;
                textBoxImageLinks.Text = "Downloading image links...";
                await scrapeTask;
                buttonExtract.Enabled = true;
                textBoxImageLinks.Text = string.Join(Environment.NewLine, scrapeTask.Result);
                labelImagesFound.Text = $"Images found: {textBoxImageLinks.Lines.Length}";
            }
            catch (HttpRequestException)
            {
                textBoxImageLinks.Text = "Failed to connect to remote site.";
                buttonExtract.Enabled = true;
            }
            catch (Exception)
            {
                textBoxImageLinks.Text = "An unknown error occured.";
                buttonExtract.Enabled = true;
            }
        }

        private async Task<List<string>> ImageScraperAsync()
        {
            var client = new HttpClient();
            Task<string> downloadHtml = client.GetStringAsync(textBoxURL.Text);
            //Task delay = Task.Delay(TimeSpan.FromSeconds(5));

            //await delay;
            var rgx = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            await downloadHtml;
            var urlMatches = rgx.Matches(downloadHtml.Result);

            var url = new List<string>();
            if (urlMatches.Count == 0) return new List<string> { "No images could be downloaded."};

            for (var i = 0; i < urlMatches.Count; i++)
            {
                //if (urlMatches[i].Groups[1].Value.StartsWith("https://")) url.Add(urlMatches[i].Groups[1].Value);
                //else url.Add(textBoxURL.Text + urlMatches[i].Groups[1].Value);
                if (!urlMatches[i].Groups[1].Value.StartsWith("https://")) url.Add(textBoxURL.Text + urlMatches[i].Groups[1].Value);
            }

            return url;
        }

        private static bool IsValidUrl(string source)
        {
            const string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            var rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rgx.IsMatch(source);
        }

        private async void ButtonSaveImages_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;

                var client = new HttpClient();
                var downloads = new List<Task<byte[]>>();
                string[] allLines = textBoxImageLinks.Text.Split(new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < textBoxImageLinks.Lines.Length; i++)
                {
                    downloads.Add(client.GetByteArrayAsync(allLines[i]));
                }

                var counter = 1;
                var fileType = string.Empty;

                while (downloads.Count > 0)
                {
                    Task<byte[]> completedTask = await Task.WhenAny(downloads);
                    switch (GetImageFormat(completedTask.Result))
                    {
                        case ImageFormat.None:
                            break;
                        case ImageFormat.Bmp:
                            fileType = "bmp";
                            break;
                        case ImageFormat.Png:
                            fileType = "png";
                            break;
                        case ImageFormat.Gif:
                            fileType = "gif";
                            break;
                        case ImageFormat.Jpg:
                            fileType = "jpg";
                            break;
                        case ImageFormat.Jpeg:
                            fileType = "jpeg";
                            break;
                    }

                    string filename = $"Image{counter}";
                    string fullPath = $"{dialog.SelectedPath}\\{filename}.{fileType}";
                    await SaveFileAsync(completedTask.Result, fullPath);

                    downloads.Remove(completedTask);
                    labelImagesFound.Text = $"Downloading image: {++counter}";
                }
            }
        }

        private static async Task SaveFileAsync(byte[] data, string fileName)
        {
            using (var sourceStream = new FileStream(fileName, FileMode.Create, FileAccess.Write,
                FileShare.Write, 4096, useAsync: true))
            {
                Task delay = Task.Delay(TimeSpan.FromMilliseconds(30));
                await sourceStream.WriteAsync(data, 0, data.Length);
                await delay;
            }
        }

        private enum ImageFormat
        {
            None, Bmp, Png, Gif, Jpg, Jpeg
        }

        private static ImageFormat GetImageFormat(byte[] bytes)
        {
            byte[] bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            byte[] gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var jpeg = new byte[] { 255, 216, 255, 224 };   // Jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };  // Jpeg2
            var png = new byte[] { 137, 80, 78, 71 };       // PNG

            if (bmp.SequenceEqual(bytes.Take(bmp.Length))) return ImageFormat.Bmp;
            if (gif.SequenceEqual(bytes.Take(gif.Length))) return ImageFormat.Gif;
            if (png.SequenceEqual(bytes.Take(png.Length))) return ImageFormat.Png;
            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length))) return ImageFormat.Jpeg;
            return jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)) ? ImageFormat.Jpeg : ImageFormat.None;
        }

        private bool UrlVerified()
        {
            if (string.IsNullOrEmpty(textBoxURL.Text)) return false;

            string urlTxtBox = textBoxURL.Text;
            var prefix = string.Empty;

            if (urlTxtBox.StartsWith("http://") || urlTxtBox.StartsWith("https://")) prefix = string.Empty;
            else if (!urlTxtBox.StartsWith("http://")) prefix = "http://";
            else if (!urlTxtBox.StartsWith("https://")) prefix = "https://";
            urlTxtBox = prefix + urlTxtBox;
            textBoxURL.Text = urlTxtBox;

            if (IsValidUrl(textBoxURL.Text)) return true;

            textBoxImageLinks.Text = $"'{textBoxURL.Text}' is not a valid Url.";
            return false;
        }
    }
}
