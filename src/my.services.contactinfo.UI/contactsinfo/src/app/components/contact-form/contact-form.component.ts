import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Contact } from '../../models/contact';


@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html'
})
export class ContactFormComponent implements OnInit {
  @Input() contact: Contact | null = null;  // To support edit
  @Output() formSubmit = new EventEmitter<Contact>();
  contactForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.contactForm = this.fb.group({
      id: [this.contact ? this.contact.id : null],
      name: [this.contact ? this.contact.name : '', Validators.required],
      email: [this.contact ? this.contact.email : '', [Validators.required, Validators.email]],
      phone: [this.contact ? this.contact.phone : '', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.contactForm.valid) {
      this.formSubmit.emit(this.contactForm.value);
    }
  }
}
