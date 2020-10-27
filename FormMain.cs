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
            buttonSaveImages.Enabled = false;
            labelImagesFound.Text = string.Empty;
            textBoxURL.Focus();
        }

        private async void ButtonExtract_Click(object sender, EventArgs e)
        {
            textBoxImageLinks.Text = string.Empty;
            buttonSaveImages.Enabled = false;
            labelFault.Text = string.Empty;
            if (!UrlVerified()) return;

            try
            {
                var scrapeTask = ImageScraperAsync();                //start Task
                buttonExtract.Enabled = false;
                labelImagesFound.Text = "Trying to download image links...";
                await scrapeTask;
                if (textBoxImageLinks.Lines.Length > 0) buttonSaveImages.Enabled = true;
                buttonExtract.Enabled = true;

            }
            catch (HttpRequestException)
            {
                labelImagesFound.Text = "Failed to connect to remote site.";
                buttonExtract.Enabled = true;
            }
            catch (Exception)
            {
                labelImagesFound.Text = "An unknown error occured.";
                buttonExtract.Enabled = true;
            }
        }

        private async Task ImageScraperAsync()
        {
            var client = new HttpClient();
            Task<string> downloadHtml = client.GetStringAsync(textBoxURL.Text);
            var rgx = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            await downloadHtml;
            var urlMatches = rgx.Matches(downloadHtml.Result);

            if (urlMatches.Count == 0)
            {
                labelImagesFound.Text = "No image links could be downloaded.";
                //buttonSaveImages.Enabled = false;
                return;
            }

            for (var i = 0; i < urlMatches.Count; i++)
            {
                var delay = Task.Delay(TimeSpan.FromMilliseconds(10));
                labelImagesFound.Text = $"Downloading image link {i + 1}";
                await delay;
                if (urlMatches[i].Groups[1].Value.StartsWith("http://") || urlMatches[i].Groups[1].Value.StartsWith("https://"))
                    textBoxImageLinks.Text += urlMatches[i].Groups[1].Value + Environment.NewLine;
                else textBoxImageLinks.Text += textBoxURL.Text + urlMatches[i].Groups[1].Value + Environment.NewLine;
            }
            textBoxImageLinks.Text = textBoxImageLinks.Text.TrimEnd(Environment.NewLine.ToCharArray());
            labelImagesFound.Text = $"Image link scraping finished. {textBoxImageLinks.Lines.Length} image links found";
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
                buttonSaveImages.Enabled = false;
                buttonExtract.Enabled = false;
                labelFault.Text = string.Empty;

                string[] allLines = textBoxImageLinks.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var downloads = new List<Task<byte[]>>();
                var client = new HttpClient();

                try
                {
                    foreach (string t in allLines)
                    {
                        downloads.Add(client.GetByteArrayAsync(t));
                    }
                }
                catch (Exception)
                {
                    labelImagesFound.Text = "An error occured while trying to download image(s).";
                    buttonSaveImages.Enabled = true;
                    buttonExtract.Enabled = true;
                    return;
                }


                var counterDownloaded = 1;
                var counterNotDownloaded = 0;
                var fileType = string.Empty;

                while (downloads.Count > 0)
                {
                    Task<byte[]> completedTask = await Task.WhenAny(downloads);
                    try
                    {
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

                        string filename = $"Image{counterDownloaded}";
                        string fullPath = $"{dialog.SelectedPath}\\{filename}.{fileType}";
                        await SaveFileAsync(completedTask.Result, fullPath);
                        downloads.Remove(completedTask);
                        labelImagesFound.Text = $"Downloading and saving image: {counterDownloaded}";
                        counterDownloaded++;
                    }
                    catch (Exception)
                    {
                        downloads.Remove(completedTask);
                        labelFault.Text = $"Images not saved: {++counterNotDownloaded}";
                    }
                }
                labelImagesFound.Text = $"Finished downloading and saving {counterDownloaded - 1} images.";
                buttonSaveImages.Enabled = true;
                buttonExtract.Enabled = true;
            }
        }

        private static async Task SaveFileAsync(byte[] data, string fileName)
        {
            using (var sourceStream = new FileStream(fileName, FileMode.Create, FileAccess.Write,
                FileShare.Write, 4096, useAsync: true))
            {
                var delay = Task.Delay(TimeSpan.FromMilliseconds(30));
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
            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length))) return ImageFormat.Jpg;
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

            labelImagesFound.Text = $"'{textBoxURL.Text}' is not a valid Url.";
            return false;
        }
    }
}
