import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from './profile.component';
import { AuthenticatorComponent } from './components/authenticator.component';
import { ShowCodesComponent } from './components/show-codes.component';
import { DeleteAccountComponent } from './components/delete-account.component';
import { AuthGuardService } from '../../shared/guards/auth.guard';

const appRoutes: Routes = [
    {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'authenticator',
        component: AuthenticatorComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'show-codes',
        component: ShowCodesComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: 'delete-account',
        component: DeleteAccountComponent,
        canActivate: [AuthGuardService]
    }
];

export const ProfileRouting = RouterModule.forChild(appRoutes);
