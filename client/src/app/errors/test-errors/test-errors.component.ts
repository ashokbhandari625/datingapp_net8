import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl ='https://localhost5001/api/' ;
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
