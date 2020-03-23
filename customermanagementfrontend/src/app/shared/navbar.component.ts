import {Component} from '@angular/core';
import {AuthService} from "../users/auth.service";
@Component({
    moduleId: module.id,
    selector: 'cm-navbar',
    templateUrl: 'navbar.component.html'
})

export class NavbarComponent {
    constructor(public auth: AuthService) {

    }
}