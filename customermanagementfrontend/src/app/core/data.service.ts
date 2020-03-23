import {Injectable} from '@angular/core';
import {ICustomer,IOrder,IState} from '../shared/interfaces';
import {Http,Response} from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/Rx';
import { strict } from 'assert';
import {AuthService} from '../users/auth.service';
@Injectable()
export class DataService {
    getStates: any;
    
    constructor(private http:Http,private auth:AuthService){

    }
    baseUrl:string = 'http://localhost:5050/api';
    getCustomers():Observable<ICustomer[]> {
        return this.http.get(this.baseUrl+ '/customers',this.auth.tokenHeader)
        .map((res:Response)=>{
            let customers=res.json();
            this.calculateCustomersOrderTotal(customers);
            return customers;
        })
        .catch(this.handleError);
    }
 private handleError(error:any) {
        console.error('server error:',error);
        if(error instanceof Response) {
            let errorMessage ='';
            try {
                errorMessage=error.json().error();
               
            }
            catch(err){
                errorMessage=err.statusText;
            }
            return Observable.throw(errorMessage);
        }
        return Observable.throw(error ||'backend server error');
    }
    private calculateCustomersOrderTotal(customers:ICustomer[]){
        for (let customer of customers){
            if(customer && customer.orders){
                let total=0;
                for(let order of customer.orders){
                    total+=order.quantity * order.price;
                }
                customer.orderTotal=total;
            }
        }
    }
    insertCustomer(customer:ICustomer ):Observable<ICustomer>{
        return this.http.post(this.baseUrl+ '/customers',customer,this.auth.tokenHeader)
            .map((res:Response) => {
                const data =res.json();
                console.log('customer insert status' +data.status);
                return data.customer;
            })
            .catch(this.handleError);
        }
        getCustomer(id: string):Observable <ICustomer>{
            return this.http.get(this.baseUrl + '/customers/'+id,this.auth.tokenHeader)
            .map ((res:Response )=>res.json())
            .catch(this.handleError);
       }
       updateCustomer(customer:ICustomer):Observable<ICustomer>{
           return this.http.put(this.baseUrl +'/customers',customer,this.auth.tokenHeader)
            .map((res:Response) => {
                const data= res.json();
                console.log('updateCustomer status:' +data.status);
                return data.customer;
            })
            .catch(this.handleError);
       }
       deleteCustomer(id:string ): Observable<boolean> {
           return this.http.delete(this.baseUrl + '/customers/'+id,this.auth.tokenHeader)
           .map((res:Response) => res.json().status)
           .catch(this.handleError);
       }


        }
    
  
