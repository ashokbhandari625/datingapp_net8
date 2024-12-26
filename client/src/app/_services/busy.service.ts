import { inject, Injectable } from '@angular/core';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';


@Injectable({
  providedIn: 'root'
})
export class BusyService {


  busyRequestCount = 0;
  private spinnerService  = inject(NgxSpinnerService) ; 

  BusyService(){
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgb(255,255,255,0)',
      color: '#333333'
    })
  }

  Idle(){
    this.busyRequestCount--;
    if( this.busyRequestCount <= 0 ){
      this.busyRequestCount = 0;
      this.spinnerService.hide() ; 
    }
  }
}
