// import { NgModule } from '@angular/core';
// import { RouterModule, Routes } from '@angular/router';
// import { ContactListComponent } from '../components/contact-list/contact-list.component';


// const routes: Routes = [
//   { path: '', redirectTo: '/contacts', pathMatch: 'full' },
//   { path: 'contacts', component: ContactListComponent }
// ];

// @NgModule({
//   imports: [RouterModule.forRoot(routes)],
//   exports: [RouterModule]
// })
// export class AppRoutingModule { }


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactListComponent } from '../components/contact-list/contact-list.component';


const routes: Routes = [
  { path: '', component: ContactListComponent },
  { path: '**', redirectTo: '' } // Redirect to home for unrecognized paths
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
