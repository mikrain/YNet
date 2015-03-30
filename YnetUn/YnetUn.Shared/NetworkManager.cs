using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace YnetUn
{
    public class NetworkManager
    {
        public Task<string> GetContent(string url)
        {
            var tcs = new TaskCompletionSource<string>();

            ThreadPool.RunAsync(async operation =>
            {
                try
                {
                    var client = new HttpClient();
                    var result = await client.GetStringAsync(url);
                    tcs.SetResult(result);
                }
                catch (WebException exception)
                {
                    using (Stream str = exception.Response.GetResponseStream())
                    {
                        if (str != null)
                        {
                            using (StreamReader sr = new StreamReader(str))
                            {
                                var text = sr.ReadToEnd();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    tcs.SetException(new Exception(text));
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    tcs.SetException(new Exception("Something is wrong !"));
                }
            });

            return tcs.Task;
        }
    }
}
