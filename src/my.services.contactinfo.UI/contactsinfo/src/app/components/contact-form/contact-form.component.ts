import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
})
export class ContactFormComponent implements OnInit {
  contactForm: FormGroup;
  contactId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.contactId = this.route.snapshot.params['id'];
    if (this.contactId) {
      this.contactService.getContactById(this.contactId).subscribe((contact) => {
        this.contactForm.patchValue(contact);
      });
    }
  }

  onSubmit(): void {
    if (this.contactForm.valid) {
      const contact: Contact = this.contactForm.value;
      if (this.contactId) {
        contact.id = this.contactId;
        this.contactService.updateContact(contact).subscribe(() => {
          this.router.navigate(['/contacts']);
        });
      } else {
        this.contactService.addContact(contact).subscribe(() => {
          this.router.navigate(['/contacts']);
        });
      }
    }
  }
}
