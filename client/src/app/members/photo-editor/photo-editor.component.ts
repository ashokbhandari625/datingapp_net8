import { Component, inject, input, OnInit, output, Output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {

  private accountService = inject(AccountService);

  member = input.required<Member>();
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiurl;
  memberChange = output<Member>();

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;

  }

  // initalizeUploader() {
  //   this.uploader = new FileUploader({
  //     url: this.baseUrl + 'users/add-photo',
  //     authToken: 'Bearer ' + this.accountservice.currentUser()?.token,
  //     isHTML5: true,
  //     allowedFileType: ['image'],
  //     removeAfterUpload: true,
  //     autoUpload: false,
  //     maxFileSize: 10 * 1024 * 1024,

  //   });

  //   this.uploader.onAfterAddingFile = (file) => {
  //     file.withCredentials = false
  //   }

  //   this.uploader.onSuccessItem = (item, response, status, headers) => {
  //     const photo = JSON.parse(response);

  //     const updatedMember = {
  //       ...this.member()
  //     }
  //     updatedMember.photos.push(photo);
  //     this.memberChange.emit(updatedMember);
  //     const user = this.accountservice.currentUser() ; 
  //     if(photo.isma)
  //     if (user) {
  //       user.photoUrl = photo.url;
  //       this.accountService.setCurrentUser(user)
  //     }
  //   }
  // }


  initializeUploader()
  {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response);
      const updatedMember = {...this.member()}
      updatedMember.photos.push(photo);
      this.memberChange.emit(updatedMember);
      if (photo.isMain) {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user)
        }
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
        this.memberChange.emit(updatedMember)
      }
    }
  }
}
