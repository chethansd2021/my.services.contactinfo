import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';


@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  selectedContact?: Contact;
  isFormVisible = false; // Boolean to control form visibility

  constructor(private contactService: ContactService) {}

  ngOnInit() {
    this.loadContacts();
  }

  loadContacts() {
    this.contactService.getContacts().subscribe(contacts => this.contacts = contacts);
  }

  onSave(contact: Contact) {
    if (contact.id) {
      this.contactService.updateContact(contact).subscribe(() => this.loadContacts());
    } else {
      this.contactService.addContact(contact).subscribe(() => this.loadContacts());
    }
    this.isFormVisible = false; // Hide form after save
    this.selectedContact = undefined; // Reset selection
  }

  onEdit(contact: Contact) {
    this.selectedContact = contact;
    this.isFormVisible = true; // Show form when editing
  }

  onAddNew() {
    this.selectedContact = undefined; // Clear selection for adding new contact
    this.isFormVisible = true; // Show form when adding new contact
  }

  onDelete(id: number) {
    this.contactService.deleteContact(id).subscribe(() => this.loadContacts());
  }
}
