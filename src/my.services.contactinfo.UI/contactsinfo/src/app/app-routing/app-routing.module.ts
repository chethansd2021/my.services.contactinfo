import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactListComponent } from '../components/contact-list/contact-list.component';
import { ContactFormComponent } from '../components/contact-form/contact-form.component';


const routes: Routes = [
  { path: '', redirectTo: '/contacts', pathMatch: 'full' },
  { path: 'contacts', component: ContactListComponent },
  { path: 'contacts/add', component: ContactFormComponent },
  { path: 'contacts/edit/:id', component: ContactFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
