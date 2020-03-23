import {Component,OnInit} from '@angular/core';
import {ICustomer,IState,IOrder} from '../shared/interfaces';
import {DataService} from '../core/data.service';
import {DataFilterService} from '../core/filter.service';

@Component ({
    moduleId:module.id,
    selector:'customers',
    templateUrl:'customers.component.html'
})

export class CustomersComponent implements OnInit{

    constructor(private dataService:DataService,private filterService:DataFilterService){

    }
    customers:ICustomer[] =[];
    filteredCustomers:ICustomer[]=[];
    title:string= '';
    ngOnInit() {
        this.title='customers list';
        this.dataService.getCustomers()
        .subscribe((customers:ICustomer[]) => {
            this.customers=this.filteredCustomers=customers;
        },
        (err:any) => console.log(err),
        () => console.log('getCustomers() müşterileri getirdi')
        );
    }
    filterChanged(filterText:string){
        if(this.customers &&filterText){
            let props =['firstName','lastName','address','city'];
            this.filteredCustomers=this.filterService.filter(this.customers,props,filterText);
        }
        else{
            this.filteredCustomers=this.customers;
        }
    }
}