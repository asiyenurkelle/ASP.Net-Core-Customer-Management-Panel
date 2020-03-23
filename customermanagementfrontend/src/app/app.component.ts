import { Component } from '@angular/core';

@Component({ 
  selector: 'app-component',
   template: `
   <main class="container">
   <cm-navbar></cm-navbar>
   <router-outlet></router-outlet>
   <br />
   <br />
   </main>
   `
 
})
export class AppComponent {
  
  constructor() { }
  
}