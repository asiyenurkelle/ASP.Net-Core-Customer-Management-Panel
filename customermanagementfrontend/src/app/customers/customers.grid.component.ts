import {Component, Input} from '@angular/core';
import {ICustomer} from '../shared/interfaces';
@Component ({
    moduleId:module.id,
    selector:'customers-grid',
    templateUrl:'customers.grid.component.html'
})

export class CustomersGridComponent
{
    title: string="Bu Grid Component";
    @Input() customers:ICustomer[]= [];
}