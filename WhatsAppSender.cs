using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace ScreenshotHelp
{
    public class WhatsAppSender
    {
        private readonly string accessToken;
        private readonly string phoneNumberId;
        private readonly string receiverNumbers;

        public WhatsAppSender()
        {
            accessToken =
                ConfigurationManager.AppSettings["AccessToken"];

            phoneNumberId =
                ConfigurationManager.AppSettings["PhoneNumberId"];

            receiverNumbers =
                ConfigurationManager.AppSettings["ReceiverNumbers"];
        }

        public void SendImage(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("Image file not found:");
                    Console.WriteLine(imagePath);
                    return;
                }

                string[] receivers =
                    receiverNumbers.Split(',');

                foreach (string receiverValue in receivers)
                {
                    string receiver =
                        receiverValue.Trim();

                    if (string.IsNullOrWhiteSpace(receiver))
                        continue;

                    Console.WriteLine();
                    Console.WriteLine(
                        "Sending screenshot to: " + receiver);

                    string mediaId =
                        UploadImage(imagePath);

                    if (string.IsNullOrEmpty(mediaId))
                    {
                        Console.WriteLine(
                            "Image upload failed.");

                        continue;
                    }

                    SendImageMessage(
                        receiver,
                        mediaId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "WhatsApp Sender Error:");

                Console.WriteLine(
                    ex.ToString());
            }
        }

        private string UploadImage(string imagePath)
        {
            try
            {
                using (HttpClient client =
                       new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            accessToken);

                    string url =
                        "https://graph.facebook.com/v23.0/"
                        + phoneNumberId
                        + "/media";

                    using (MultipartFormDataContent form =
                           new MultipartFormDataContent())
                    {
                        form.Add(
                            new StringContent("whatsapp"),
                            "messaging_product");

                        form.Add(
                            new StringContent("image/png"),
                            "type");

                        using (FileStream fileStream =
                               File.OpenRead(imagePath))
                        {
                            StreamContent fileContent =
                                new StreamContent(fileStream);

                            fileContent.Headers.ContentType =
                                new MediaTypeHeaderValue(
                                    "image/png");

                            form.Add(
                                fileContent,
                                "file",
                                Path.GetFileName(imagePath));

                            HttpResponseMessage response =
                                client.PostAsync(
                                    url,
                                    form)
                                .GetAwaiter()
                                .GetResult();

                            string result =
                                response.Content
                                .ReadAsStringAsync()
                                .GetAwaiter()
                                .GetResult();

                            Console.WriteLine(
                                "Upload Response:");

                            Console.WriteLine(result);

                            if (!response.IsSuccessStatusCode)
                            {
                                return null;
                            }

                            // Response se media ID nikalna
                            Match match =
                                Regex.Match(
                                    result,
                                    "\"id\"\\s*:\\s*\"([^\"]+)\"");

                            if (match.Success)
                            {
                                return match.Groups[1].Value;
                            }

                            Console.WriteLine(
                                "Media ID not found.");

                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Upload Error:");

                Console.WriteLine(
                    ex.ToString());

                return null;
            }
        }

        private void SendImageMessage(
            string receiver,
            string mediaId)
        {
            try
            {
                using (HttpClient client =
                       new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            accessToken);

                    string url =
                        "https://graph.facebook.com/v23.0/"
                        + phoneNumberId
                        + "/messages";

                    string json =
                        "{"
                        + "\"messaging_product\":\"whatsapp\","
                        + "\"to\":\"" + receiver + "\","
                        + "\"type\":\"image\","
                        + "\"image\":{"
                        + "\"id\":\"" + mediaId + "\""
                        + "}"
                        + "}";

                    using (StringContent content =
                           new StringContent(
                               json,
                               Encoding.UTF8,
                               "application/json"))
                    {
                        HttpResponseMessage response =
                            client.PostAsync(
                                url,
                                content)
                            .GetAwaiter()
                            .GetResult();

                        string result =
                            response.Content
                            .ReadAsStringAsync()
                            .GetAwaiter()
                            .GetResult();

                        Console.WriteLine(
                            "Send Response:");

                        Console.WriteLine(result);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine(
                                "Image sent successfully to "
                                + receiver);
                        }
                        else
                        {
                            Console.WriteLine(
                                "Image send failed for "
                                + receiver);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Send Image Error:");

                Console.WriteLine(
                    ex.ToString());
            }
        }
    }
}