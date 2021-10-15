using System.Collections.Generic;

public class MsgList
{
    private Dictionary<string, int> protocol = new Dictionary<string,int>();
    private static MsgList instance;
    private MsgList()
    {
        protocol.Add("Item.Use", 10000);
        protocol.Add("Item.Buy", 10001);

    }

    public static int Protocol(string key)
    {
        if (instance == null)
        {
            instance = new MsgList();
        }
        return instance.protocol[key];
    }

}
