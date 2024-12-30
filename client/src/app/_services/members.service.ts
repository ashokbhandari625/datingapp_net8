import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, model, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/UserParams';
import { MemberEditComponent } from '../members/members-edit/member-edit.component';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  private http = inject(HttpClient);
  members = signal<Member[]>([]);
  accountService = inject(AccountService); 

  baseUrl = environment.apiurl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  membercache = new Map();
  user = this.accountService.currentUser() ;

  userParams = signal<UserParams>(new  UserParams(this.user)) ; 
  

  resetUserParams(){
    this.userParams.set(new UserParams(this.user)); 
  }


  getMembers() {
    const response = this.membercache.get(Object.values(this.userParams()).join('-'));
    if (response) return this.setPaginatedResponse(response) ;
    let params = this.setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);
    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('orderBy', this.userParams().orderBy);

    params = params.append('gender', this.userParams().gender);

    return this.http.get<Member[]>(this.baseUrl + "users", { observe: 'response', params }).subscribe({
      next: response => {
      this.setPaginatedResponse(response) ; 
      this.membercache.set(Object.values(this.userParams()).join('-') , response);
      }})
  }


private setPaginatedResponse(response: HttpResponse<Member[]>){
  this.paginatedResult.set({
    items: response.body as Member[],
    pagination: JSON.parse(response.headers.get('Pagination')!)
  })
}


private setPaginationHeaders(pageNumber : number, pageSize : number){
  let params = new HttpParams();
  if (pageNumber && pageSize) {
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
  }
  return params;
}

getMember(username: string) {
const member :Member = [...this.membercache.values()]
.reduce((arr,elem) => arr.concat(elem.body) , [])
.find((m: Member)=>{
  m.username === username ;
} );
if(member)return of(member) ; 
  return this.http.get<Member>(this.baseUrl + "users/" + username);

}

updateMember(member: Member) {
  return this.http.put(this.baseUrl + 'users', member).pipe(
    tap(() => {
      this.members.update(mebers => mebers.map(m => m.username == member.username ? member : m))
    })
  )
}
}
