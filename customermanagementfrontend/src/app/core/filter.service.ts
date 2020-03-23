import {Injectable} from '@angular/core';
import { ICustomer } from '../shared/interfaces';

@Injectable()
export class DataFilterService{
    filter(dataSource:ICustomer[],filterProperties: string[],filterText:string){
        if(dataSource && filterProperties && filterText){
            filterText =filterText.toUpperCase();
            const filtered=dataSource.filter(item =>{
                let match=false;
                for(const prop of filterProperties){
                    let propVal: any='';
                    if(item[prop]) {
                        propVal=item[prop].toUpperCase();
                    }
                    if(propVal.indexOf(filterText) > -1) {
                        match=true;
                        break;
                    }
                };
                return match;
            });
            return filtered;
        }
    }
}