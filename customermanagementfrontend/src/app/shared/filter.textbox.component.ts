import {Component, Output, EventEmitter} from '@angular/core';
@Component({
   selector: 'filter-textbox',
   template: `<form> 
    Filter:
    <input name ="filter" type="text" 
    [(ngModel)] = "model.filter" (keyup)="filterBoxKeyUp($event)" />


    </form>
   `
})

export class FilterTextBoxComponent {

  @Output() changed: EventEmitter<string> = new EventEmitter<string>();

    model: {filter: string } ={filter : null};
    filterBoxKeyUp(event: any) {
        event.preventDefault();
        this.changed.emit(this.model.filter); 
    }
}