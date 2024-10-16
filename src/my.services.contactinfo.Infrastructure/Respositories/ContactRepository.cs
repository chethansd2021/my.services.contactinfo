using my.services.contactinfo.Application.Services;
using my.services.contactinfo.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace my.services.contactinfo.Infrastructure.Respositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _filePath = "contacts.json";

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            if (!File.Exists(_filePath)) return new List<Contact>();
            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Contact>>(jsonData) ?? new List<Contact>();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddContactAsync(Contact contact)
        {
            var contacts = await GetAllContactsAsync();
            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
            contacts.Add(contact);
            await SaveAllContactsAsync(contacts);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var contacts = await GetAllContactsAsync();
            var existingContact = contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Email = contact.Email;
                await SaveAllContactsAsync(contacts);
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
                await SaveAllContactsAsync(contacts);
            }
        }

        private async Task SaveAllContactsAsync(List<Contact> contacts)
        {
            var jsonData = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, jsonData);
        }
    }
}
