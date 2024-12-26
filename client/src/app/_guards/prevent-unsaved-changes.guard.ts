import { CanActivateFn, CanDeactivateFn } from '@angular/router';
import { MembersEditComponent } from '../members/members-edit/members-edit.component';
export const preventUnsavedChangesGuard: CanDeactivateFn<MembersEditComponent> = (component) => {
  if( component.editForm?.dirty){
    return confirm('Are you sure you want to continue? Any unsaved changes wil be lost');
  }
  return true; 
};
