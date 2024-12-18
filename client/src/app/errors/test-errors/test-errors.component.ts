import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl =environment.apiurl; 
  private http = inject(HttpClient);

  get400Error(){
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response=> console.log( response),
      error: error => console.log(error) 
    })
  }
  get500Error(){
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response=> console.log( response),
      error: error => console.log(error) 
    })
  }
  get400ValidationError(){
    this.http.get(this.baseUrl + 'account/register' , {}).subscribe({
      next: response=> console.log( response),
      error: error => console.log(error) 
    })
  }


}
