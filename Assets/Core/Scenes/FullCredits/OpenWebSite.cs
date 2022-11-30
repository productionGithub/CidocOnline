using UnityEngine;

/// <summary>
/// Each logo button uses this script to open the related web site.
/// </summary>
public class OpenWebSite : MonoBehaviour
{
    string logoName;

    public void Open()
    {
        logoName = gameObject.name;

        if (logoName != null)
        {
            switch (logoName)
            {
                case "MasaLogo":
                    Application.OpenURL("https://masa.hypotheses.org");
                    break;
                case "HumanumLogo":
                    Application.OpenURL("https://www.huma-num.fr/");
                    break;
                case "TakinLogo":
                    Application.OpenURL("http://takin.solutions");
                    break;
                case "IndytionLogo":
                    Application.OpenURL("http://www.indytion.com");
                    break;
                case "MercedLogo":
                    Application.OpenURL("http://www.ucmerced.edu");
                    break;
                case "CidocGameSite":
                    Application.OpenURL("http://www.cidoc-crm-game.org/");
                    break;
                case "Youtube":
                    Debug.Log("YOUTUBE OPENS");
                    Application.OpenURL("https://youtu.be/lVQFciW7V4I");
                    break;
                case "CidocCRMSite":
                    Application.OpenURL("http://www.cidoc-crm.org/");
                    break;
                default:
                    break;
            }
        }
    }
}
