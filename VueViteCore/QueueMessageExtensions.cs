using System.Text;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;

namespace VueViteCore;

public static class QueueMessageExtensions
{
    public static string AsString(this QueueMessage message)
    {
        byte[] data = Convert.FromBase64String(message.MessageText);
        return Encoding.UTF8.GetString(data);
    }
 
    public static T As<T>(this QueueMessage message) where T : class
    {
        byte[] data = Convert.FromBase64String(message.MessageText);
        string json = Encoding.UTF8.GetString(data);
        return Deserialize<T>(json, true);
    }
 
    private static T Deserialize<T>(string json, bool ignoreMissingMembersInObject) where T : class
    {
        T deserializedObject;
        MissingMemberHandling missingMemberHandling = MissingMemberHandling.Error;
        if (ignoreMissingMembersInObject)
            missingMemberHandling = MissingMemberHandling.Ignore;
        deserializedObject = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { MissingMemberHandling = missingMemberHandling, });
        return deserializedObject;
    }    
}