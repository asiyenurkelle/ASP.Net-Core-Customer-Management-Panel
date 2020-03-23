import {Component} from '@angular/core';
import {AuthService} from '../users/auth.service';

@Component({
    moduleId: module.id,
    selector: 'app-login',
    template: `
        <div class="container">

            <br/><br/><br/>
                <div class="form-group">
                    <input type="text" class="form-control" 
                    [(ngModel)] ="loginData.userName" placeholder="User Name" style="width:350px;">
                    
                </div>
                  <div class="form-group">
                    <input type="text" class="form-control" 
                    [(ngModel)] ="loginData.password" placeholder="Password" style="width:350px;">
                    
                </div>
                <button type="submit" (click) ="login()">Login</button>
        </div>
    `,
    styles: [`
        .error {
            background-color: #fff0f0
        }
    `]
})

export class LoginComponent {

    constructor(public auth: AuthService) {

    }
    loginData = {
        userName: '',
        password: ''
    };

    login() {
        this.auth.login(this.loginData);
        console.log(this.loginData);
    }
}