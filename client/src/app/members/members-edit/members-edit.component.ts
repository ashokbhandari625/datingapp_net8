import { Component, HostListener, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-members-edit',
  standalone: true,
  imports: [TabsModule, FormsModule],
  templateUrl: './members-edit.component.html',
  styleUrl: './members-edit.component.css'
})
export class MembersEditComponent implements OnInit {


  @ViewChild("editForm") editForm?: NgForm; 
  @HostListener('window:beforeunload', ['$event']  ) notify($event:any){
    if( this.editForm?.dirty){
      $event.returnValue=true; 
    }
  }
  member?: Member;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService); 
  ngOnInit(): void {
this.loadMember();
  }
loadMember(){
  const user = this.accountService.currentUser();
  if(!user) return ;
  this.memberService.getMember(user.username).subscribe({
  next:  member=> this.member = member
  })
}
updateMember(){
 this.memberService.updateMember(this.editForm?.value).subscribe({
  next: _ => {
    this.toastr.success("profile updated successfully "); 
    this.editForm?.reset(this.member) ; 
  }
 })

}

}