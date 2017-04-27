using UnityEngine;
using System.Collections;
using System.Net;
using LaoHan.Infrastruture;
using System;
using System.ComponentModel;

public class lhWebClient  {
    
    private static lhWebClient m_instance;
    public static lhWebClient GetInstance()
    {
        if (m_instance != null) return null;
        return m_instance = new lhWebClient();
    }
    lhWebClient() {}
    public void DownloadFileAsync(Uri uri,string filePath,DownloadProgressChangedEventHandler progressHandler=null, AsyncCompletedEventHandler completedHandler=null)
    {
        lhLoom.RunAsync(() =>
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadProgressChanged+=(sender,e) =>{
                    lhLoom.RunMain(() =>
                    {
                        if (progressHandler!=null)
                            progressHandler(sender, e);
                    });
                };
                client.DownloadFileCompleted += (sender,e)=> {
                    lhLoom.RunMain(() =>
                    {
                        if (completedHandler != null)
                            completedHandler(sender, e);
                    });
                };
                client.DownloadFileAsync(uri, filePath);
            }
        });
    }
    public void UploadFileAsync(Uri uri, string filePath, UploadProgressChangedEventHandler progressHandler = null, UploadFileCompletedEventHandler completedHandler = null)
    {
        lhLoom.RunAsync(() =>
        {
            using (WebClient client=new WebClient())
            {
                client.UploadProgressChanged += (sender, e) => {
                    lhLoom.RunMain(() =>
                    {
                        if (progressHandler != null)
                            progressHandler(sender, e);
                    });
                };
                client.UploadFileCompleted += (sender, e) => {
                    lhLoom.RunMain(() =>
                    {
                        if (completedHandler != null)
                            completedHandler(sender, e);
                    });
                };
                client.UploadFileAsync(uri, filePath);
            }

        });
    }
    
}
