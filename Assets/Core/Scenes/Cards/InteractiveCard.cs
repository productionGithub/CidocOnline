using System.Collections.Generic;

public abstract class InteractiveCard
{
    public List<string> icons;
    public string id;
    public string about;
    public string label;
    public string comment;
    public List<string> parentsClassList;//List of 'super' entities (Entities or Properties
    public List<string> childrenClassList;//List of 'sub' entities (Entities or Properties)
    public List<string> colors;//List of color names that match a Dictionary name -> Color32 - TODO
}
