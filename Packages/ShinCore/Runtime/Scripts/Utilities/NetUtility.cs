using System;
using System.Net;

namespace Shin.Core {

    public static class NetUtility {

    static void DownloadSync(string uri, string destPath, Action<string> onSuccess, Action<WebException> onFail) {

        WebClient client = new WebClient();
        try {
            client.DownloadFile(new System.Uri(uri), destPath);
            onSuccess(destPath);
        } catch (WebException e) {
            onFail(e);
        }
    }

//---------------------------------------------------------------------------------------------------------------------

}

} //namespace