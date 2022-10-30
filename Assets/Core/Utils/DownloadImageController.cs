using Cysharp.Threading.Tasks;
using StarterCore.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ImageUtilities;

public class DownloadImageController : MonoBehaviour
{
    /*
    private string firstImageURL = "http://ontomatchgame.huma-num.fr/StreamingAssets/scenarii/MarmoutierFR/Instances/Images/02_archaeologist.jpg";
    private string secondImageURL = "http://ontomatchgame.huma-num.fr/StreamingAssets/scenarii/MarmoutierFR/Instances/Images/24_MarmoutierAbbey.jpg";
    private string spriteImageURL = "http://ontomatchgame.huma-num.fr/StreamingAssets/scenarii/MarmoutierFR/Instances/Images/52_scsmartinus.jpg";

    public RawImage image1;
    public RawImage image2;
    public Image image3;

    public async UniTask DownloadImagesAsync()
    {
        image1.texture = await DownloadJPGImage(firstImageURL, "first");

        image2.texture = await DownloadJPGImage(secondImageURL, "second");

        image3.sprite = await DownloadPNGImage(spriteImageURL, "unity");
    }

    public async UniTask<Texture2D> DownloadJPGImage(string url, string name)
    {
        Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.JPG);
        return img;
    }

    public async UniTask<Sprite> DownloadPNGImage(string url, string name)
    {
        Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.PNG);
        return Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
    }
    */
}
