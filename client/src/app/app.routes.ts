import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';

import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guard/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ListComponent } from './list/list.component';
import { MembersEditComponent } from './members/members-edit/members-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

export const routes: Routes = [

    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [

            { path: 'members', component: MemberListComponent, canActivate: [authGuard] },
            { path: 'members/:username', component: MemberDetailComponent },
            { path: 'member/edit', component: MembersEditComponent, canDeactivate: [preventUnsavedChangesGuard] },
            { path: 'lists', component: ListComponent },
            { path: 'messages', component: MessagesComponent },



        ]
    },

    { path: 'error', component: TestErrorsComponent },

    { path: '**', component: HomeComponent, pathMatch: 'full' },


];
