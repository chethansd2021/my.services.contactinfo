import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact';


@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html'
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  selectedContact: Contact | null = null;
  feedbackMessage: string = '';

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.contactService.getContacts().subscribe(contacts => this.contacts = contacts);
  }

  onAddContact(contact: Contact): void {
    this.contactService.addContact(contact);
    this.feedbackMessage = 'Contact added successfully!';
  }

  onEditContact(contact: Contact): void {
    this.selectedContact = contact;
  }

  onUpdateContact(contact: Contact): void {
    this.contactService.updateContact(contact);
    this.selectedContact = null;
    this.feedbackMessage = 'Contact updated successfully!';
  }

  onDeleteContact(id: number): void {
    this.contactService.deleteContact(id);
    this.feedbackMessage = 'Contact deleted successfully!';
  }
}
