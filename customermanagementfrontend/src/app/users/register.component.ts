import {Component} from '@angular/core';
import {AuthService} from '../users/auth.service';
import {FormBuilder, Validators} from '@angular/forms';
@Component({
    moduleId: module.id,
    selector: 'app-register',
    templateUrl:  'register.component.html',
    styles: [' .error { background-color: #fff0f0 } ']
   
})

export class RegisterComponent {

   form:any;
  
    constructor(public auth: AuthService, private fb: FormBuilder) {

        this.form = fb.group({
            userName : ['', Validators.required],
            firstName : ['', Validators.required],
            lastName : ['', Validators.required],
            email : ['', [Validators.required, emailValid()]],
            password : ['', Validators.required],
            confirmPassword : ['', Validators.required],
        },
        { validator: matchingFields('password','confirmPassword')})
    }


   onSubmit() {
       console.log(this.form.errors);
       this.auth.register(this.form.value);
   }
  
  isValid(control: any) {
      return this.form.controls[control].invalid && this.form.controls[control].touched;
  }

}
   function matchingFields(field1: any, field2: any) {
       
    return  form => {
        if (form.controls[field1].value !== form.controls[field2].value)
            return { mismatchedFields: true }
    }
}

function emailValid() {
    return control => {
        var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

        return regex.test(control.value) ? null : { invalidEmail: true }
    }
}
 
