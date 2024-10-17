import { Component, OnInit } from '@angular/core';
import { Contact } from './models/contact.model'; // Define Contact model
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ContactService } from './services/contact.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  contacts: Contact[] = [];
  isEditing: boolean = false;
  showForm: boolean = false;
  currentContact: Contact | null = null;
  contactForm: FormGroup;

  constructor(private fb: FormBuilder, private contactService: ContactService) {
    // Initializing the form
    this.contactForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  ngOnInit() {    
    this.loadContacts();
  }

  loadContacts() {
    this.contactService.getContacts().subscribe((contacts) => {
      this.contacts = contacts;
    });
  }

  showAddForm() {
    this.showForm = true;
    this.isEditing = false;
    this.contactForm.reset();
  }

  onEdit(contact: Contact) {
    this.showForm = true;
    this.isEditing = true;
    this.currentContact = contact;
    this.contactForm.patchValue(contact);
  }

  onSubmit() {
    if (this.contactForm.valid) {
      if (this.isEditing && this.currentContact) {
        this.contactService.updateContact({ ...this.currentContact, ...this.contactForm.value })
          .subscribe(() => this.loadContacts());
      } else {
        this.contactService.addContact(this.contactForm.value)
          .subscribe(() => this.loadContacts());
      }
      this.showForm = false;
    }
  }

  onDelete(id: number) {
    this.contactService.deleteContact(id)
      .subscribe(() => this.loadContacts());
  }

  onCancel() {
    this.showForm = false;
  }
}
