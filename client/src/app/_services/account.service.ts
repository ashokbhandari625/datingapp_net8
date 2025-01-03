import { HttpClient } from '@angular/common/http';
import { Injectable, inject , signal} from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
private http = inject(HttpClient) ; 
baseurl = environment.apiurl; 
currentUser = signal<User | null > (null);
login(model : any ){
  return this.http.post<User>(this.baseurl + 'account/login' , model ).pipe(
    map(user=>{
      if( user) {
        this.setCurrentUser(user); 

      }
    })
  )
}

logout(){
  localStorage.removeItem('user');
  this.currentUser.set(null); 
}

register(model : any ){
  return this.http.post<User>(this.baseurl + 'account/register' , model ).pipe(
    map(user=>{
      if( user) {
       this.setCurrentUser(user); 
      }
      return user; 
    })
  )
}

setCurrentUser(user:User){
  localStorage.setItem('user' ,  JSON.stringify(user));
        this.currentUser.set(user) ;
}
  
}
