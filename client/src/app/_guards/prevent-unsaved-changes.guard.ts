import { CanActivateFn, CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/members-edit/member-edit.component';
export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  if( component.editForm?.dirty){
    return confirm('Are you sure you want to continue? Any unsaved changes wil be lost');
  }
  return true; 
};
