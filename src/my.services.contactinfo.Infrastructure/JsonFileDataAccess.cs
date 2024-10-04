using my.services.contactinfo.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class JsonFileDataAccess
{
    private readonly string _filePath;

    public JsonFileDataAccess(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<Contact>> LoadContactsAsync()
    {
        if (!File.Exists(_filePath))
            return new List<Contact>();

        var jsonData = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<Contact>>(jsonData) ?? new List<Contact>();
    }

    public async Task SaveContactsAsync(List<Contact> contacts)
    {
        var jsonData = JsonConvert.SerializeObject(contacts, Formatting.Indented);
        await File.WriteAllTextAsync(_filePath, jsonData);
    }
}
