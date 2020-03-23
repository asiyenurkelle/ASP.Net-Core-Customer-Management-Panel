import {Component, OnInit} from '@angular/core';
 import {Router, ActivatedRoute} from '@angular/router';
import {DataService} from '../core/data.service';
import {IState, ICustomer, IOrder} from '../shared/interfaces';
@Component({
    moduleId: module.id,
    selector: 'customers-edit',
    templateUrl: 'customers.edit.component.html'
})

export class CustomersEditComponent implements OnInit {
    constructor(private route:ActivatedRoute, private router: Router, 
    private dataService: DataService ) {}


    operationText: string ='Insert';
    states: IState[];
    errorMessage: string;

   deleteMessageEnabled: boolean;

    customer: ICustomer = {
        firstName:'',
        lastName:'',
        gender:'',
        email: '',
        city :'',
        zip: 0,
        address: '',
        stateId: 0 
    };

    ngOnInit() {
        
     let id = this.route.snapshot.params['id'];
     if( id !=='0') {
          this.operationText = 'Update';
          this.getCustomer(id);
          this.getStates();
      
     }
    
     
    }

    getStates() {
        this.dataService.getStates()
            .subscribe((states:IState[]) => this.states = states);
    }
     getCustomer(id: string) {
      this.dataService.getCustomer(id)
        .subscribe((customer: ICustomer) => {
           
          this.customer = customer;  
        },
        (err: any) => console.log(err));
  }

submit() {
    if(this.customer.id){
        this.dataService.updateCustomer(this.customer)
            .subscribe((customer:ICustomer) => {
                if(customer) {
                    this.router.navigate(['/customers']);
                }
                else {
                    this.errorMessage ="Veri güncellemesi yapılamadı";
                }
            },
            (err:any) =>console.log(err));
            
           
    }
   else {
       this.dataService.insertCustomer(this.customer)
         .subscribe((customer: ICustomer) => {
             if(customer) {
                 this.router.navigate(['/customers']);
             }
             else{
                 this.errorMessage = "Müşteri kaydı yapılamadı!";
             }
         },
         (err:any) => console.log(err));
   }

}
cancel(event: Event) {
    event.preventDefault();
    this.router.navigate(['/']);
}
   delete(event: Event) {
       event.preventDefault();
       this.dataService.deleteCustomer(this.customer.id)
        .subscribe((status: boolean) => {
            if(status) {
                this.router.navigate(['/customers']);
            }
            else {
                this.errorMessage ='Kayıt silinemedi!";'
            }
        },
        
        (err: any) => console.log(err));
   }
   
}